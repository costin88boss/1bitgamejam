using UnityEngine;

[CreateAssetMenu(fileName = "NewGameScene", menuName = "Story Scene/New game scene", order = 0)]
public class Scene : ScriptableObject
{
    public Sprite scene;
    [Header("Unused in some derived classes!")]
    public Scene nextScene = null;
    public Caption[] captions = new Caption[0];

    public float fadeStartDuration;
    public float fadeEndDuration;
    public float startDelay;
}
