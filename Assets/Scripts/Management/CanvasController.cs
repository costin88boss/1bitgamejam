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

    public GameObject visualScene;


    private EventSystem eventSystem;

    public void Start()
    {
        DontDestroyOnLoad(EventSystem.current);
        DontDestroyOnLoad(gameObject);
        eventSystem = EventSystem.current;
    }

    public void NewGame()
    {
        eventSystem.enabled = false;
        cameraController.fadeScript.FadeOut(2f, OnStartGameFadeOutComplete);
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
            cameraController.fadeScript.FadeIn(2f, () => {
                eventSystem.enabled = true;
            });
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
    }
}
