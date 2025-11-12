using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "FoodAround", menuName = "ScriptableObjects/Conditions/FoodAroundSO", order = 3)]

public class FoodAroundSO : ConditionSO
{
    public override bool CheckCondition(FishStateMachine fish)
    {
        GameObject closestFood = null;
        if (ObjectsManager.instance.food.Count != 0)
        {
            foreach (Food food in ObjectsManager.instance.food)
            {
                float distance = Vector3.Distance(fish.transform.position, food.transform.position);
                if (distance <= fish.detectionRadius)
                {
                    if (closestFood == null || distance < Vector3.Distance(fish.transform.position, closestFood.transform.position))
                    {
                        closestFood = food.gameObject;
                    }
                }
            }
        }
        if (closestFood != null)
        {
            fish.target = closestFood.transform;
        }
        return closestFood != null;
    }
}
