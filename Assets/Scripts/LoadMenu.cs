using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMenu : MonoBehaviour
{
    private static bool loaded;
    void Start()
    {
        if (loaded) return;
        loaded = true;
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}
