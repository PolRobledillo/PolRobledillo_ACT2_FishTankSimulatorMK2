using UnityEngine;

public abstract class ConditionSO : ScriptableObject
{
    public bool answer;
    public abstract bool CheckCondition(FishStateMachine fish);
}
