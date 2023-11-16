using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewGameScene", menuName = "Story Scene/New decisional game scene", order = 0)]
public class DecisionalScene : Scene
{
    public Scene sceneA;
    public Scene sceneB;

    public LocalizedString decisionA;
    public LocalizedString decisionB;
        
}
