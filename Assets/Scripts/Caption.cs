
using UnityEngine;
using UnityEngine.Localization;

[System.Serializable]
public struct Caption
{
    public LocalizedString text;
    [Header("If speed is 0, it will be set to 20 (default)")]
    [Min(0)]
    public float speed;
    //[Header("The GameObject below will be enabled when this caption gets run")]
    [Header("The GameObject below will be enabled when this caption gets run\n(Optional, also assuming it's inside Canvas/SpecialUIStuff)")]
    public string captionGameObjectName;

    public Caption(LocalizedString text, string captionGameObjectName = "", float speed = 20)
    {
        this.text = text;
        this.speed = speed;
        this.captionGameObjectName = captionGameObjectName;
    }
}
