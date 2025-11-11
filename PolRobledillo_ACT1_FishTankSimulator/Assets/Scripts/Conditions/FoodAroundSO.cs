using UnityEngine;
[CreateAssetMenu(fileName = "FoodAround", menuName = "ScriptableObjects/States/FoodAroundSO")]

public class FoodAroundSO : ConditionSO
{
    public override bool CheckCondition(FishStateMachine fish)
    {
        GameObject closestPredator = null;

        if (ObjectsManager.instance.predators.Count != 0)
        {
            foreach (FishStateMachine predator in ObjectsManager.instance.predators)
            {
                float distance = Vector3.Distance(fish.transform.position, predator.transform.position);
                if (distance <= fish.detectionRadius)
                {
                    if (closestPredator == null || distance < Vector3.Distance(fish.transform.position, closestPredator.transform.position))
                    {
                        closestPredator = predator.gameObject;
                    }
                }
            }
        }

        if (closestPredator != null)
        {
            fish.target = closestPredator.transform;
        }

        return closestPredator != null;
    }
}
