using UnityEngine;

[CreateAssetMenu(fileName = "NewScenario", menuName = "ScnearioData/New Scenario")]
public class ScriptableData : ScriptableObject
{
    public ScenarioAction[] scenarioSteps;
}
