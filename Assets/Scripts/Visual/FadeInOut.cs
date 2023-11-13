using System;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{

    public float fadeTime;
    public float _fadeTime;
    public float level;
    public bool stopped = true;

    private Action callback = null;
    // fade time = negative = fade in
    // fade time = positive = fade out

    // Start is called before the first frame update
    void Start()
    {

    }

    public bool IsDone()
    {
        if (fadeTime >= 0) return _fadeTime == fadeTime;
        return _fadeTime == 0;
    }

    public void FadeIn(float fadeTime, Action callback = null)
    {
        if (fadeTime == 0 && callback != null)
        {
            callback();
            gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            return;
        }
        if (fadeTime < 0) Debug.LogError("Fade time is a negative number!");
        this.fadeTime = -fadeTime;
        _fadeTime = fadeTime;
        level = 1;
        stopped = false;
        this.callback = callback;
    }

    public void FadeOut(float fadeTime, Action callback = null)
    {
        if (fadeTime == 0 && callback != null)
        {
            callback();
            gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            return;
        }
        if (fadeTime < 0) Debug.LogError("Fade time is a negative number!");
        this.fadeTime = fadeTime;
        _fadeTime = 0;
        level = 0;
        stopped = false;
        this.callback = callback;
    }

    private void UpdateColor()
    {
        level = _fadeTime / Mathf.Abs(fadeTime);
        level = GameUtils.Math.easeOutExpo(level);

        if (!gameObject.TryGetComponent<SpriteRenderer>(out var spr))
        {
            Color color = gameObject.GetComponent<Image>().color;
            gameObject.GetComponent<Image>().color = new Color(color.r, color.g, color.b, level);
            return;
        }
        spr.color = new Color(0, 0, 0, level);

    }

    private void Finish()
    {
        stopped = true;
        if (callback == null) return;
        callback();
        //callback = null;
        // that assignment is gonna fuck the code
    }

    // Update is called once per frame
    void Update()
    {
        if (stopped) return;

        if (fadeTime >= 0)
        {
            _fadeTime += Time.unscaledDeltaTime;
            if (_fadeTime >= fadeTime)
            {
                _fadeTime = fadeTime;
                Finish();
            }
            UpdateColor();
            return;
        }

        _fadeTime -= Time.unscaledDeltaTime;
        if (_fadeTime <= 0)
        {
            _fadeTime = 0;
            Finish();
        }
        UpdateColor();
    }
}
