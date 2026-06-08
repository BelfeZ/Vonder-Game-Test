using UnityEngine;

public enum ActionType
{
    PlayDialogueLine,
    TriggerTimelineCutscene
}

[System.Serializable]
public struct ScenarioAction
{
    public ActionType actionType;

    [Header("Dialogue Properties (Only used if ActionType is PlayDialogueLine)")]
    public string characterName;
    public string faceAnimationState;
    [TextArea(3, 5)]
    public string conversationText;

    [Header("Timeline Properties (Only used if ActionType is TriggerTimelineCutscene)")]
    public string timelineDirectorGameObjectName;
}