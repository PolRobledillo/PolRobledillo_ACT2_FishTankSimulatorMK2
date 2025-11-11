using UnityEngine;
[CreateAssetMenu(fileName = "ChaseState", menuName = "ScriptableObjects/States/ChaseStateSO")]

public class ChaseStateSO : StateSO
{
    public float slowingRadius = 1f;
    public override void OnStateEnter(FishStateMachine fish)
    {
        fish.currentStateEnum = FishEnums.FishState.Seek;
        fish.fastParticles.Play();
    }
    public override void OnStateUpdate(FishStateMachine fish)
    {
        fish.velocity = Vector3.ClampMagnitude(fish.velocity + fish.Seek(fish.target.position) + fish.CollisionAvoidance(), fish.maxSpeed);
    }
    public override void OnStateExit(FishStateMachine fish)
    {
        fish.fastParticles.Stop();
    }
}
