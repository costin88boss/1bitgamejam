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
    private int subtitleIndex;

    private string text = "";
    private float typeSpeed;

    // Data for the story
    public TMP_InputField PlayerName { get; set; }
    public TMP_InputField PlayerPass { get; set; }

    public string playerName { get; set; }
    public string playerPass { get; set; }

    private bool specialDone;

    [Header("For special scenes")]
    public GameObject accPrompt;


    // Special functions for special scenes.
    private void SceneSpecialEnd()
    {
        nextBtn.enabled = true;
        specialDone = true;
        OnClick();
    }

    public void SceneAccPrompt()
    {
        if (PlayerName.text.Trim().Length == 0 || PlayerPass.text.Trim().Length == 0) return;

        playerName = PlayerName.text;
        playerPass = PlayerPass.text;

        accPrompt.SetActive(false);
        SceneSpecialEnd();
    }


    private void WriteText(float speed = 20)
    {
        // If it gives error, something is wrong.
        text = scene.subtitles[subtitleIndex].GetLocalizedString();

        // transform shit
        if (PlayerName != null) {
            text = text.Replace("{name}", playerName);
            text = text.Replace("{pass}", playerPass);
        }

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

        if(specialDone)
        {
            FadeInAndPrepareScene();
            specialDone = false;
            return;
        }

        switch (scene.specialType)
        {
            // TODO TWO DECISIONS
            case SceneSpecialTypes.NormalScene:
                FadeInAndPrepareScene();
                break;
            case SceneSpecialTypes.AccPrompt:
                accPrompt.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void FadeInAndPrepareScene()
    {
        sceneImg.GetComponent<FadeInOut>().FadeIn(scene.fadeEndDuration, () =>
        {
            PrepareAfterClick();
        });
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
}
