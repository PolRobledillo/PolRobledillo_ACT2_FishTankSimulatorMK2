using UnityEngine;
[CreateAssetMenu(fileName = "TargetIsNull", menuName = "ScriptableObjects/Conditions/TargetIsNullSO", order = 4)]

public class TargetIsNullSO : ConditionSO
{
    public override bool CheckCondition(FishStateMachine fish)
    {
        return fish.target == null;
    }
}
