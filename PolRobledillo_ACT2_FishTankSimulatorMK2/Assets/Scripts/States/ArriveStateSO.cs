using UnityEngine;
[CreateAssetMenu(fileName = "ArriveState", menuName = "ScriptableObjects/States/ArriveStateSO")]

public class ArriveStateSO : StateSO
{
    public float slowingRadius = 1f;
    public override void OnStateEnter(FishStateMachine fish)
    {
        fish.currentStateEnum = FishEnums.FishState.Seek;
        fish.fastParticles.Play();
    }
    public override void OnStateUpdate(FishStateMachine fish)
    {
        fish.velocity = Vector3.ClampMagnitude(fish.velocity + Arrive(fish) + fish.CollisionAvoidance(), fish.maxSpeed);
    }
    public override void OnStateExit(FishStateMachine fish)
    {
        fish.fastParticles.Stop();
    }
    Vector3 Arrive(FishStateMachine fish)
    {
        Vector3 steering;

        Vector3 desiredVelocity = fish.target.position - fish.transform.position;
        float distance = desiredVelocity.magnitude;
        if (distance < slowingRadius)
        {
            desiredVelocity = desiredVelocity.normalized * fish.maxVelocity * (distance / slowingRadius);
            steering = desiredVelocity - fish.velocity;
        }
        else
        {
            steering = fish.Seek(fish.target.position);
        }

        return steering;
    }
}
