using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public abstract class StateSO : ScriptableObject
{
    public ConditionSO startCondition;
    public List<ConditionSO> endConditions;
    public abstract void OnStateEnter(FishStateMachine fish);
    public abstract void OnStateUpdate(FishStateMachine fish);
    public abstract void OnStateExit(FishStateMachine fish);
}
