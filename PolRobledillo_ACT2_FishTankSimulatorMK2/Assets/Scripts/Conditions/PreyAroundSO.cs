using UnityEngine;
[CreateAssetMenu(fileName = "PreyAround", menuName = "ScriptableObjects/Conditions/PreyAroundSO", order = 2)]

public class PreyAroundSO : ConditionSO
{
    public override bool CheckCondition(FishStateMachine fish)
    {
        GameObject closestPrey = null;

        if (ObjectsManager.instance.preys.Count != 0)
        {
            foreach (FishStateMachine prey in ObjectsManager.instance.preys)
            {
                float distance = Vector3.Distance(fish.transform.position, prey.transform.position);
                if (distance <= fish.detectionRadius)
                {
                    if (closestPrey == null || distance < Vector3.Distance(fish.transform.position, closestPrey.transform.position))
                    {
                        closestPrey = prey.gameObject;
                    }
                }
            }
        }

        if (closestPrey != null)
        {
            fish.target = closestPrey.transform;
        }

        return closestPrey != null;
    }
}
