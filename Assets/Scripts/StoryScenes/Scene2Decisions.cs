
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameScene", menuName = "Story Scene/New 2 decisions scene", order = 0)]
public class Scene2Decisions : Scene
{
    public string firstDecision;
    public string secondDecision;

    internal bool wasLeft;

    private void Awake()
    {
        this.specialType = SceneSpecialTypes.TwoDecisions;
    }
}
