using UnityEngine;

public class Eatable : MonoBehaviour
{
    [SerializeField] private SphereCollider eatableCollider;
    private void OnTriggerEnter(Collider other)
    {
        Fish fish = other.GetComponent<Fish>();
        if (fish != null)
        {
            Fish amIFish = gameObject.GetComponent<Fish>();
            if (amIFish != null)
            {
                if (fish.fishType == FishEnums.FishType.Predator)
                {
                    ObjectsManager.instance.UnregisterFish(amIFish);
                    Destroy(gameObject);
                }
            }
            else
            {
                Food food = GetComponent<Food>();
                if (food != null)
                {
                    ObjectsManager.instance.UnregisterFood(food);
                    Destroy(gameObject);
                }
            }
        }
    }
}
