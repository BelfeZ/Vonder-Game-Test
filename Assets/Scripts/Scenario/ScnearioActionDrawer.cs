using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ScenarioAction))]
public class ScenarioActionDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        float line = EditorGUIUtility.singleLineHeight;
        float space = EditorGUIUtility.standardVerticalSpacing;

        Rect rect = new Rect(position.x, position.y, position.width, line);

        SerializedProperty actionType = property.FindPropertyRelative("actionType");
        EditorGUI.PropertyField(rect, actionType);

        rect.y += line + space;

        switch ((ActionType)actionType.enumValueIndex)
        {
            case ActionType.PlayDialogueLine:
                DrawField(ref rect, property.FindPropertyRelative("characterName"), line, space);
                DrawField(ref rect, property.FindPropertyRelative("faceAnimationState"), line, space);

                SerializedProperty text = property.FindPropertyRelative("conversationText");
                rect.height = 60f;
                text.stringValue = EditorGUI.TextArea(rect, text.stringValue);
                break;

            case ActionType.TriggerTimelineCutscene:
                DrawField(ref rect, property.FindPropertyRelative("timelineDirectorGameObjectName"), line, space);
                break;

            case ActionType.ShowBanner:
                DrawField(ref rect, property.FindPropertyRelative("bannerText"), line, space);
                break;
        }
    }

    private void DrawField(ref Rect rect, SerializedProperty prop, float line, float space)
    {
        rect.height = line;
        EditorGUI.PropertyField(rect, prop);
        rect.y += line + space;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float line = EditorGUIUtility.singleLineHeight;
        float space = EditorGUIUtility.standardVerticalSpacing;

        SerializedProperty actionType = property.FindPropertyRelative("actionType");

        float height = line + space;

        switch ((ActionType)actionType.enumValueIndex)
        {
            case ActionType.PlayDialogueLine:
                height += line + space;
                height += line + space;
                height += 60f + space;
                break;

            case ActionType.TriggerTimelineCutscene:
                height += line;
                break;

            case ActionType.ShowBanner:
                height += line;
                break;
        }

        return height + 6f;
    }
}