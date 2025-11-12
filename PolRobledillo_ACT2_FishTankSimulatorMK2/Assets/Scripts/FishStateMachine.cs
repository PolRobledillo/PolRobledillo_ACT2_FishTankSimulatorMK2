using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static FishEnums;

public class FishStateMachine : MonoBehaviour
{
    public StateSO currentState;
    public FishEnums.FishState currentStateEnum;
    public List<StateSO> states;

    [Header("Fish Settings")]
    public Transform target;
    public float maxVelocity = 5f;
    public float maxSpeed = 10f;
    public float maxForce = 0.5f;
    public float mass = 1f;
    public Vector3 velocity = Vector3.zero;
    public FishType fishType = FishType.Prey;
    public float detectionRadius = 6f;

    [Header("Collision Avoidance Settings")]
    public float maxVisionDistance = 7.5f;
    public float maxAvoidanceForce = 10f;

    [Header("Wander Settings")]
    public float wanderTimer = 0f;
    public Vector3 wanderPosition = Vector3.zero;

    [Header("Particles Settings")]
    public ParticleSystem wanderParticles;
    public ParticleSystem fastParticles;

    void Start()
    {
        if (currentState != null)
        {
            currentState.OnStateEnter(this);
        }
    }

    void Update()
    {
        currentState.OnStateUpdate(this);
        transform.position += velocity * Time.deltaTime;
        ApplyRotation();
        CheckEndingConditions();
    }
    void CheckEndingConditions()
    {
        foreach (ConditionSO condition in currentState.endConditions)
        {
            if (condition.CheckCondition(this) == condition.answer)
            {
                ExitCurrentNode();
                break;
            }
        }
    }
    void ExitCurrentNode()
    {
        foreach (StateSO state in states)
        {
            if (state.startCondition == null)
            {
                EnterState(state);
                break;
            }
            else
            {
                if (state.startCondition.CheckCondition(this) == state.startCondition.answer)
                {
                    EnterState(state);
                    break;
                }
            }
        }
    }
    void EnterState(StateSO newState)
    {
        currentState.OnStateExit(this);
        currentState = newState;
        currentState.OnStateEnter(this);
    }
    public Vector3 Seek(Vector3 target)
    {
        Vector3 desiredVelocity = (target - transform.position).normalized * maxVelocity;
        Vector3 steering = desiredVelocity - velocity;
        steering = Vector3.ClampMagnitude(steering, maxForce);
        return steering / mass;
    }
    public Vector3 CollisionAvoidance()
    {
        Vector3 steering = Vector3.zero;
        Vector3 ahead = transform.position + velocity.normalized * maxVisionDistance;
        Vector3 obstacle = Vector3.zero;
        Vector3[] startPositions = {transform.position + (transform.right * 0.25f),
                                  transform.position - (transform.right * 0.25f),
                                  transform.position + (transform.up * 0.25f),
                                  transform.position - (transform.up * 0.25f)};


        RaycastHit hit;
        for (int i = 0; i < 4; i++)
        {
            if (Physics.Raycast(startPositions[i], transform.forward, out hit, maxVisionDistance) && (hit.transform.gameObject.tag == "Obstacle" || hit.transform.gameObject.tag == "Surface"))
            {
                obstacle = hit.point;
                steering.x = ahead.x - obstacle.x;
                steering.y = ahead.y - obstacle.y;
                steering.z = ahead.z - obstacle.z;
                steering = steering.normalized * maxAvoidanceForce;
                i = 4;
            }
            else
            {
                steering = Vector3.zero;
            }
        }

        return steering;
    }
    void ApplyRotation()
    {
        Quaternion rotation = Quaternion.LookRotation(velocity);
        transform.rotation = rotation;
    }
}
