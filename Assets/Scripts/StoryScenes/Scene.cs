using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewGameScene", menuName = "Story Scene/New game scene", order = 0)]
public class Scene : ScriptableObject
{
    public SceneSpecialTypes specialType = SceneSpecialTypes.NormalScene;

    public Sprite scene;
    public Scene nextScene = null;
    public LocalizedString[] subtitles = null;

    public float fadeStartDuration;
    public float fadeEndDuration;
    public float startDelay;
}
