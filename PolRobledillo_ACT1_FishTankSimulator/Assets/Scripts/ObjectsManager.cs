using System.Collections.Generic;
using System.Runtime.Serialization;
using TMPro;
using UnityEngine;
using static FishEnums;

public class ObjectsManager : MonoBehaviour
{
    #region Singleton
    static ObjectsManager objectsManager;
    public static ObjectsManager instance
    {
        get
        {
            return RequestSteeringObstacleManager();
        }
    }

    private static ObjectsManager RequestSteeringObstacleManager()
    {
        if (!objectsManager)
        {
            GameObject objectsManagerObj = new GameObject("ObjectsManager");
            objectsManager = objectsManagerObj.AddComponent<ObjectsManager>();
            objectsManager.predators = new List<FishStateMachine>();
            objectsManager.preys = new List<FishStateMachine>();
            objectsManager.food = new List<Food>();
        }
        return objectsManager;
    }

    private void Awake()
    {
        if (objectsManager == null)
        {
            objectsManager = this;
            predators = new List<FishStateMachine>();
            preys = new List<FishStateMachine>();
            food = new List<Food>();
        }
        else if (objectsManager != this)
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    public GameObject preyPrefab;
    public GameObject predatorPrefab;
    public GameObject foodPrefab;
    public List<FishStateMachine> predators = new List<FishStateMachine>();
    public List<FishStateMachine> preys = new List<FishStateMachine>();
    public List<Food> food = new List<Food>();
    public TextMeshProUGUI predatorCount;
    public TextMeshProUGUI preyCount;

    public void RegisterFish(FishStateMachine fish)
    {
        if (fish.fishType == FishType.Predator)
        {
            if (!predators.Contains(fish))
            {
                predators.Add(fish);
                predatorCount.text = predators.Count.ToString();
            }
        }
        else
        {
            if (!preys.Contains(fish))
            {
                preys.Add(fish);
                preyCount.text = preys.Count.ToString();
            }
        }
    }
    public void UnregisterFish(FishStateMachine fish)
    {
        if (fish.fishType == FishType.Predator)
        {
            if (predators.Contains(fish))
            {
                predators.Remove(fish);
                predatorCount.text = predators.Count.ToString();
            }
        }
        else
        {
            if (preys.Contains(fish))
            {
                preys.Remove(fish);
                preyCount.text = preys.Count.ToString(); 
            }
        }
    }
    public void RegisterFood(Food foodItem)
    {
        if (!food.Contains(foodItem))
        {
            food.Add(foodItem);
        }
    }
    public void UnregisterFood(Food foodItem)
    {
        if (food.Contains(foodItem))
        {
            food.Remove(foodItem);
        }
    }
    public void InstantiatePrey()
    {
        Vector3 position = GetRandomPosition();
        Instantiate(preyPrefab, position, Quaternion.identity);
    }
    public void RemovePrey()
    {
        if (preys.Count > 0)
        {
            FishStateMachine preyToRemove = preys[preys.Count - 1];
            UnregisterFish(preyToRemove);
            Destroy(preyToRemove.gameObject);
        }
    }
    public void InstantiatePredator()
    {
        Vector3 position = GetRandomPosition();
        Instantiate(predatorPrefab, position, Quaternion.identity);
    }
    public void RemovePredator()
    {
        if (predators.Count > 0)
        {
            FishStateMachine predatorToRemove = predators[predators.Count - 1];
            UnregisterFish(predatorToRemove);
            Destroy(predatorToRemove.gameObject);
        }
    }
    public void InstantiateFood(Vector3 position)
    {
        Instantiate(foodPrefab, position, Quaternion.identity);
    }
    public Vector3 GetRandomPosition()
    {
        Vector3 cubeCenter = new Vector3(0, 10f, 0);
        Vector3 cubeSize = new Vector3(24, 14, 44);

        float randomX = Random.Range(cubeCenter.x - cubeSize.x / 2, cubeCenter.x + cubeSize.x / 2);
        float randomY = Random.Range(cubeCenter.y - cubeSize.y / 2, cubeCenter.y + cubeSize.y / 2);
        float randomZ = Random.Range(cubeCenter.z - cubeSize.z / 2, cubeCenter.z + cubeSize.z / 2);

        return new Vector3(randomX, randomY, randomZ);
    }
}
