using System;
using TMPro;
using Unity.Properties;
using Unity.VisualScripting;
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
    private int subtitleIndex = -1;

    private Caption text;

    private string str = "";

    private float typeSpeed;

    public GameObject specialUIStuffObj;
    public GameObject biDecisionalScreen;
    public TextMeshProUGUI biDecisionLeft, biDecisionRight;

    // Data for the story
    public TMP_InputField PlayerName { get; set; }
    public TMP_InputField PlayerPass { get; set; }
    public string PlayerNameStr { get; set; } = "";
    public string PlayerPassStr { get; set; } = "";

    public void SpecialCaptionEnd()
    {
        if (PlayerNameStr.Length == 0)
        {
            if (PlayerName.text.Trim().Length == 0 || PlayerPass.text.Trim().Length == 0)
            {
                PlayerName.text = "Dave";
                PlayerPass.text = "Dave";
            }
            PlayerNameStr = PlayerName.text;
            PlayerPassStr = PlayerPass.text;
        }
        nextBtn.enabled = true;
        OnClick(true);
    }

    private string _originalText;

    public void WriteCustomTextAt20(string text)
    {
        if (_originalText == text) return;
        str = text;
        _originalText = text;
        str = TransformStrStuff(str);
        subtitles.text = "";
        CancelInvoke(nameof(WriteChars));
        InvokeRepeating(nameof(WriteChars), 0, 1f / 20);
    }


    private string TransformStrStuff(string str)
    {
        if (PlayerName != null)
        {
            str = str.Replace("{name}", PlayerName.text);
            str = str.Replace("{pass}", PlayerPass.text);
            str = str.Replace("{{", "{"); // Don't question
            str = str.Replace("}}", "}");
        }
        return str;
    }

    private void WriteText(float speed = 20)
    {
        subtitleIndex++;
        // If it gives error, something is wrong.

        if (text.text == null) return; // hopefully doesn't make issues

        str = text.text.GetLocalizedString();
        if(text.speed > 0) speed = text.speed;
        
        str = TransformStrStuff(str);
        this.typeSpeed = speed;
        subtitles.text = "";
        CancelInvoke(nameof(WriteChars));
        InvokeRepeating(nameof(WriteChars), 0, 1f / speed);
    }

    private Scene nextScene = null;

    public void Decision(bool isRight)
    {
        if (scene is DecisionalScene a)
        {
            nextScene = !isRight ? a.sceneA : a.sceneB;
            PrepareAfterClick();
            return;
        }
        Debug.LogError("Function Decision called while on a non-decisional scene");
    }

    public void OnClick(bool forceSkip)
    {
        forceSkip = true;
        if (!forceSkip && subtitles.text.Length - str.Length < 3 && str.Length != 0) return;
        str = "";
        subtitles.text = "";
        CancelInvoke(nameof(WriteChars));

        if (scene == null) return; // should never be true.

        if (scene.captions.Length > subtitleIndex)
        {
            text = scene.captions[subtitleIndex];
            if (text.text != null)
            {
                WriteText();
                if(text.captionGameObjectName != "")
                {
                    nextBtn.enabled = false;
                    specialUIStuffObj.transform.Find(text.captionGameObjectName).gameObject.SetActive(true);
                }
                return;
            }
        }

        nextBtn.enabled = false;

        if (scene is DecisionalScene a)
        {
            biDecisionalScreen.SetActive(true);
            biDecisionLeft.text = a.decisionA.GetLocalizedString();
            biDecisionRight.text = a.decisionB.GetLocalizedString();
            biDecisionLeft.text = TransformStrStuff(biDecisionLeft.text);
            biDecisionRight.text = TransformStrStuff(biDecisionRight.text);
            return;
        }
        FadeInAndPrepareNextScene();
    }

    public void FadeInAndPrepareNextScene()
    {
        sceneImg.GetComponent<FadeInOut>().FadeIn(scene.fadeEndDuration, () =>
        {
            PrepareAfterClick();
        });
    }

    private void PrepareAfterClick()
    {
        str = "";
        subtitles.text = "";
        CancelInvoke(nameof(WriteChars));
        scene = scene switch
        {
            DecisionalScene => nextScene,
            _ => scene.nextScene,
        };
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
            if (scene.captions.Length > subtitleIndex) text = scene.captions[subtitleIndex];
            else return;
            if (text.text == null) return;
            OnClick(true);
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
        if (str.Length == 0) return;
        subtitles.text += str[0];
        str = str[1..];
    }

    private void Start()
    {
        nextBtn.enabled = false;
    }
}
