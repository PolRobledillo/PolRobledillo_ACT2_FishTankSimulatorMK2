using UnityEngine;
[CreateAssetMenu(fileName = "FleeState", menuName = "ScriptableObjects/States/FleeStateSO")]

public class FleeStateSO : StateSO
{
    public override void OnStateEnter(FishStateMachine fish)
    {
        fish.currentStateEnum = FishEnums.FishState.Flee;
        fish.fastParticles.Play();
    }
    public override void OnStateUpdate(FishStateMachine fish)
    {
        fish.velocity = Vector3.ClampMagnitude(fish.velocity + Flee(fish) + fish.CollisionAvoidance(), fish.maxSpeed);
    }
    public override void OnStateExit(FishStateMachine fish)
    {
        fish.fastParticles.Stop();
    }
    Vector3 Flee(FishStateMachine fish)
    {
        return fish.Seek(fish.target.position) * -1;
    }
}
