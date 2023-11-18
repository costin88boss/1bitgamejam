using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{


    public CameraController cameraController;
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject pauseMenu;
    public GameObject gameHud;
    public GameObject bg;

    public GameObject visualScene;


    private EventSystem eventSystem;

    public void Start()
    {
        DontDestroyOnLoad(EventSystem.current);
        DontDestroyOnLoad(gameObject);
        eventSystem = EventSystem.current;
        eventSystem.enabled = false;
        GetComponent<AudioSource>().volume = 0;
        cameraController.fadeScript.gameObject.GetComponent<SpriteRenderer>().color = new Color(0,0,0,1);
        Invoke(nameof(StartLast), 0.1f);
    } 

    private void StartLast()
    {
        cameraController.fadeScript.FadeIn(2, () =>
        {
            eventSystem.enabled = true;
        }, gameObject.GetComponent<AudioSource>());
    }

    public void NewGame()
    {
        eventSystem.enabled = false;
        cameraController.fadeScript.FadeOut(2f, OnStartGameFadeOutComplete, gameObject.GetComponent<AudioSource>());
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    public void UnpauseGame()
    {
        Time.timeScale = 1;
    }

    public void BackToMainMenu()
    {
        eventSystem.enabled = false;
        cameraController.fadeScript.FadeOut(2f, () =>
        {
            visualScene.SetActive(false);
            mainMenu.SetActive(true);
            gameHud.SetActive(false);
            pauseMenu.SetActive(false);
            bg.SetActive(true);
            cameraController.fadeScript.FadeIn(2f, () => {
                eventSystem.enabled = true;
            });
            gameObject.GetComponent<AudioSource>().volume = 1;
            gameObject.GetComponent<AudioSource>().Play();
            UnpauseGame();
        });
    }

    void OnStartGameFadeOutComplete()
    {
        visualScene.SetActive(true);
        visualScene.GetComponent<StoryController>().sceneImg.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        cameraController.fadeScript.FadeIn(2f, () =>
        {
            eventSystem.enabled = true;
            visualScene.GetComponent<StoryController>().BeginStory();
        });
        mainMenu.SetActive(false);
        gameHud.SetActive(true);
        bg.SetActive(false);
    }
}
