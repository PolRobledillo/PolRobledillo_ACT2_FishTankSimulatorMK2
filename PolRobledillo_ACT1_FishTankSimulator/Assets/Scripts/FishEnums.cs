using UnityEngine;

public class FishEnums : MonoBehaviour
{
    public enum FishType
    {
        Prey,
        Predator
    }
    public enum FishState
    {
        Seek,
        Flee,
        Wander
    }
}
