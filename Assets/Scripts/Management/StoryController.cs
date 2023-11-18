using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryController : MonoBehaviour
{
    public TextMeshProUGUI subtitles;
    public Image sceneImg;

    public Button nextBtn;

    public Button restartBtn;



    public int attemptsRemaining = 3;

    public Scene introScene;
    public Scene noLightsEndScene;
    public Scene withLightsEndScene;

    private Scene scene;
    private int subtitleIndex = -1;

    public TextMeshProUGUI enterCredentials3Attempts;

    public EndingScene noAttemptsScene;
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

    public AudioClip[] typingSounds;
    public AudioSource typingSfx;



    public void SpecialCaptionEnd()
    {
        if (PlayerNameStr.Length == 0)
        {
            if (PlayerName.text.Trim().Length == 0 || PlayerPass.text.Trim().Length == 0)
            {
                if (text.captionGameObjectName == "AccPrompt")
                {
                    nextBtn.enabled = false;
                    specialUIStuffObj.transform.Find("AccPrompt").gameObject.SetActive(true);
                    return;
                }
            }
            PlayerNameStr = PlayerName.text;
            PlayerPassStr = PlayerPass.text;
        }

        if (text.captionGameObjectName == "EnterCredentials3Attempts" && attemptsRemaining > 0)
        {
            if (PlayerPass.text != PlayerPassStr)
            {
                attemptsRemaining--;
                if (attemptsRemaining > 0)
                {
                    nextBtn.enabled = false;
                    enterCredentials3Attempts.text = "Attempts: " + attemptsRemaining;
                    specialUIStuffObj.transform.Find("EnterCredentials3Attempts").gameObject.SetActive(true);
                    return;
                }
            }
            else attemptsRemaining = 3;
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
        if (PlayerNameStr.Length > 0)
        {
            str = str.Replace("{name}", PlayerNameStr);
            str = str.Replace("{pass}", PlayerPassStr);
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
        if (text.speed > 0) speed = text.speed;

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
    [HideInInspector]
    public bool lightsTurnedOn;
    public void ControlPanelDecision(bool turnOn)
    {
        lightsTurnedOn = turnOn;
    }

    public void OnClick(bool forceSkip)
    {
        if (!forceSkip && subtitles.text.Length - str.Length < 3 && str.Length != 0)
        {
            subtitles.text += str;
            str = "";
            return;
        }
        str = "";
        subtitles.text = "";
        CancelInvoke(nameof(WriteChars));

        if (scene == null) return; // should never be true.

        if (attemptsRemaining > 0)
        {
            if (scene.captions.Length > subtitleIndex)
            {
                text = scene.captions[subtitleIndex];
                if (text.text.TableEntryReference.KeyId == 1609563672428581 && !lightsTurnedOn)
                {
                    subtitleIndex++;
                    OnClick(true);
                    return;
                }
                if (text.text != null)
                {
                    WriteText();
                    if (text.captionGameObjectName != "")
                    {
                        nextBtn.enabled = false;
                        specialUIStuffObj.transform.Find(text.captionGameObjectName).gameObject.SetActive(true);
                    }
                    return;
                }
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
        if (scene is EndingScene b)
        {
            if (b.isGoodEnding)
            {
                WriteCustomTextAt20("To be continued..");
                restartBtn.gameObject.SetActive(true);
                nextBtn.enabled = false;
                return;
            }
            WriteCustomTextAt20("Bad ending!");
            nextScene = introScene;
            nextBtn.enabled = true;
        }
        FadeInAndPrepareNextScene();
    }

    public void FadeInAndPrepareNextScene()
    {
        sceneImg.GetComponent<FadeInOut>().FadeIn(scene.fadeEndDuration, () =>
        {
            PrepareAfterClick();
        }, songToPlay, true);
    }

    private void PrepareAfterClick()
    {
        str = "";
        subtitles.text = "";
        CancelInvoke(nameof(WriteChars));
        string sceneName = scene.name;
        scene = scene switch
        {
            DecisionalScene => nextScene,
            EndingScene => nextScene,
            _ => scene.nextScene,
        };
        if (attemptsRemaining <= 0)
        {
            scene = noAttemptsScene;
            attemptsRemaining = 3;
        }
        if (sceneName == "near_ending")
        {
            if (lightsTurnedOn) scene = withLightsEndScene;
            else scene = noLightsEndScene;
        }
        if (scene == null) return;
        sceneImg.sprite = scene.scene;
        Invoke(nameof(StoryDelayHandle), scene.startDelay);
    }

    public AudioSource songToPlay = null;

    public void ResetStuff()
    {
        subtitles.text = "";
        nextBtn.enabled = false;
        subtitleIndex = 0;
        str = "";
    }

    private void StartScene()
    {
        subtitles.text = "";
        nextBtn.enabled = false;
        subtitleIndex = 0;
        if (scene.songToPlay != null)
        {
            songToPlay.clip = scene.songToPlay;
            songToPlay.Play();
            songToPlay.loop = true;
        }
        sceneImg.GetComponent<FadeInOut>().FadeOut(scene.fadeStartDuration, () =>
        {
            nextBtn.enabled = true;
            if (scene.captions.Length > subtitleIndex) text = scene.captions[subtitleIndex];
            else return;
            if (text.text == null) return;
            OnClick(true);
        }, songToPlay, true);
    }

    internal void BeginStory()
    {
        restartBtn.gameObject.SetActive(false);
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
        if ("abcdefghijklmnopqrstuvwxyz0123456789()".Contains(str[0].ToString().ToLower()))
        {
            typingSfx.clip = typingSounds[Random.Range(0, typingSounds.Length)];
            typingSfx.Play();
        }
        str = str[1..];
    }

    private void Start()
    {
        nextBtn.enabled = false;
    }
}
