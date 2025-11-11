using UnityEngine;

[CreateAssetMenu(fileName = "WanderState", menuName = "ScriptableObjects/States/WanderStateSO")]

public class WanderStateSO : StateSO
{
    public float wanderTimer = 0f;
    public float wanderEvaluationTime = 5f;
    public float wanderDistanceToTarget = 10f;
    private Vector3 wanderPosition = Vector3.zero;

    public override void OnStateEnter(FishStateMachine fish)
    {
        fish.currentStateEnum = FishEnums.FishState.Wander;
        fish.wanderParticles.Play();
        UpdateTargetPosForWandering(fish);
    }
    public override void OnStateUpdate(FishStateMachine fish)
    {
        fish.velocity = Vector3.ClampMagnitude(fish.velocity + Wander(fish) + fish.CollisionAvoidance(), fish.maxSpeed);
    }
    public override void OnStateExit(FishStateMachine fish)
    {
        fish.wanderParticles.Stop();
    }
    Vector3 Wander(FishStateMachine fish)
    {
        wanderTimer += Time.deltaTime;
        float distanceToWanderPos = Vector3.Distance(fish.transform.position, wanderPosition);
        if (wanderTimer >= wanderEvaluationTime || distanceToWanderPos < wanderDistanceToTarget)
        {
            wanderTimer = 0f;
            UpdateTargetPosForWandering(fish);
        }
        return fish.Seek(wanderPosition);
    }
    void UpdateTargetPosForWandering(FishStateMachine fish)
    {
        Vector3 position = fish.transform.position;
        Vector3 targetPos = ObjectsManager.instance.GetRandomPosition();

        wanderPosition = targetPos;
    }
}
