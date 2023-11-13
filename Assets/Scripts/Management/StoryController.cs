using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class StoryController : MonoBehaviour
{
    public TextMeshProUGUI subtitles;
    public Image sceneImg;

    public Button nextBtn;

    public Scene introScene;

    private Scene scene;
    private int subtitleIndex;

    private string text = "";
    private float typeSpeed;

    private void WriteText(float speed = 20)
    {
        // If it gives error, something is wrong.
        text = scene.subtitles[subtitleIndex].GetLocalizedString();
        subtitleIndex++;
        this.typeSpeed = speed;
        subtitles.text = "";
        CancelInvoke(nameof(WriteChars));
        InvokeRepeating(nameof(WriteChars), 0, 1f / speed);
    }

    public void OnClick()
    {
        if (text.Length > 3) return;

        if (scene == null) return; // should never be true.

        if (scene.subtitles != null && subtitleIndex < scene.subtitles.Length)
        {
            WriteText();
            return;
        }

        nextBtn.enabled = false;
        sceneImg.GetComponent<FadeInOut>().FadeIn(scene.fadeEndDuration, () =>
        {
            PrepareAfterClick();
        });
        return;
    }

    private void PrepareAfterClick()
    {
        subtitles.text = "";
        scene = scene.nextScene;
        if (scene == null) return;
        sceneImg.sprite = scene.scene;
        Invoke(nameof(StoryDelayHandle), scene.startDelay);
    }

    private void StartScene()
    {
        subtitles.text = "";
        nextBtn.enabled = false;
        subtitleIndex = 0;
        sceneImg.GetComponent<FadeInOut>().FadeOut(scene.fadeStartDuration, () =>
        {
            nextBtn.enabled = true;
            if (scene.subtitles == null || scene.subtitles.Length == 0) return;
            WriteText();
        });
    }

    internal void BeginStory()
    {
        scene = introScene;
        sceneImg.sprite = scene.scene;
        sceneImg.color = new Color(1, 1, 1, 0);
        Invoke(nameof(StoryDelayHandle), scene.startDelay);
    }

    private void StoryDelayHandle()
    {
        StartScene();
    }

    private void WriteChars()
    {
        if (text.Length == 0) return;
        subtitles.text += text[0];
        text = text[1..];
    }

    private void Start()
    {
        nextBtn.enabled = false;
    }

    void Update()
    {

    }
}
