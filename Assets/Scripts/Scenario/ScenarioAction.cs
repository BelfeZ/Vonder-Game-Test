using UnityEngine;

public enum ActionType
{
    PlayDialogueLine,
    TriggerTimelineCutscene,
    ShowBanner
}

[System.Serializable]
public struct ScenarioAction
{
    public ActionType actionType;

    public string characterName;
    public string faceAnimationState;

    [TextArea(3, 5)]
    public string conversationText;

    public string timelineDirectorGameObjectName;

    public string bannerText;
}