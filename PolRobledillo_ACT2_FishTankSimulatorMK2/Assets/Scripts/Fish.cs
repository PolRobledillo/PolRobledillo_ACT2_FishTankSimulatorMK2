using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using static FishEnums;
using UnityEngine.AI;

public class Fish : MonoBehaviour
{
    [Header("Fish Settings")]
    public Transform target;
    public float maxVelocity = 5f;
    public float maxSpeed = 10f;
    public float maxForce = 0.5f;
    public float mass = 1f;
    [SerializeField] private Vector3 velocity = Vector3.zero;
    [SerializeField] private FishState currentState = FishState.Wander;
    public FishType fishType = FishType.Prey;
    public bool predatorDetected = false;
    public float detectionRadius = 6f;

    [Header("Arrival Settings")]
    public float slowingRadius = 1f;

    [Header("Wander Settings")]
    public float wanderTimer = 0f;
    public float wanderEvaluationTime = 5f;
    public float wanderDistanceToTarget = 10f;
    [SerializeField] private Vector3 wanderPosition = Vector3.zero;

    [Header("Collision Avoidance Settings")]
    public float maxVisionDistance = 7.5f;
    public float maxAvoidanceForce = 10f;

    [Header("Particles Settings")]
    public ParticleSystem wanderParticles;
    public ParticleSystem fastParticles;

    private void Start()
    {
        //ObjectsManager.instance.RegisterFish(this);
        UpdateTargetPosForWandering();
    }
    void Update()
    {
        ScanSurroundings();
        PerformMovement();
        SetParticles();
        ApplyRotation();
    }
    void ApplyRotation()
    {
        Quaternion rotation = Quaternion.LookRotation(velocity);
        transform.rotation = rotation;
    }
    void SetParticles()
    {
        if (currentState == FishState.Wander)
        {
            wanderParticles.Play();
            fastParticles.Stop();
        }
        else
        {
            wanderParticles.Stop();
            fastParticles.Play();
        }
    }
    void PerformMovement()
    {
        if (target == null)
        {
            velocity = Vector3.ClampMagnitude(velocity + Wander() + CollisionAvoidance(), maxSpeed);
            currentState = FishState.Wander;
        }
        else if (fishType == FishType.Predator)
        {
            velocity = Vector3.ClampMagnitude(velocity + Seek(target.position) + CollisionAvoidance(), maxSpeed);
            currentState = FishState.Seek;
        }
        else
        {
            if (predatorDetected)
            {
                velocity = Vector3.ClampMagnitude(velocity + Flee(target.position) + CollisionAvoidance(), maxSpeed);
                currentState = FishState.Flee;
            }
            else
            {
                velocity = Vector3.ClampMagnitude(velocity + Arrive(target.position) + CollisionAvoidance(), maxSpeed);
                //currentState = FishState.Arrive;
            }
        }
        transform.position += velocity * Time.deltaTime;
    }
    void ScanSurroundings()
    {
        GameObject priorityTarget = null;

        if (fishType == FishType.Prey)
        {
            //priorityTarget = SearchPredators();
        }
        else
        {
            //priorityTarget = SearchPreys();
        }

        if (priorityTarget == null)
        {
            priorityTarget = SearchFood();
        }

        target = priorityTarget != null ? priorityTarget.transform : null;
    }/*
    private GameObject SearchPredators()
    {
        GameObject closestPredator = null;
        if (ObjectsManager.instance.predators.Count != 0)
        {
            foreach (Fish fish in ObjectsManager.instance.predators)
            {
                float distanceToPredator = Vector3.Distance(transform.position, fish.transform.position);
                if (distanceToPredator <= detectionRadius)
                {
                    if (closestPredator == null || distanceToPredator < Vector3.Distance(transform.position, closestPredator.transform.position))
                    {
                        closestPredator = fish.gameObject;
                        predatorDetected = true;
                    }
                }
            }
        }

        return closestPredator;
    }
    private GameObject SearchPreys()
    {
        GameObject closestPrey = null;
        if (ObjectsManager.instance.preys.Count != 0)
        {
            foreach (Fish fish in ObjectsManager.instance.preys)
            {
                float distanceToPrey = Vector3.Distance(transform.position, fish.transform.position);
                if (distanceToPrey <= detectionRadius)
                {
                    if (closestPrey == null || distanceToPrey < Vector3.Distance(transform.position, closestPrey.transform.position))
                    {
                        closestPrey = fish.gameObject;
                    }
                }
            }
        }
        return closestPrey;
    }*/
    private GameObject SearchFood()
    {
        GameObject closestFood = null;
        if (ObjectsManager.instance.food.Count != 0)
        {
            foreach (Food foodItem in ObjectsManager.instance.food)
            {
                float distanceToFood = Vector3.Distance(transform.position, foodItem.transform.position);
                if (distanceToFood <= detectionRadius)
                {
                    if (closestFood == null || distanceToFood < Vector3.Distance(transform.position, closestFood.transform.position))
                    {
                        closestFood = foodItem.gameObject;
                    }
                }
            }
        }
        return closestFood;
    }
    Vector3 Seek(Vector3 target)
    {
        Vector3 desiredVelocity = (target - transform.position).normalized * maxVelocity;
        Vector3 steering = desiredVelocity - velocity;
        steering = Vector3.ClampMagnitude(steering, maxForce);
        return steering / mass;
    }
    Vector3 Flee(Vector3 target)
    {
        return Seek(target) * -1;
    }
    Vector3 Arrive(Vector3 target)
    {
        Vector3 steering;

        Vector3 desiredVelocity = target - transform.position;
        float distance = desiredVelocity.magnitude;
        if (distance < slowingRadius)
        {
            desiredVelocity = desiredVelocity.normalized * maxVelocity * (distance / slowingRadius);
            steering = desiredVelocity - velocity;
        }
        else
        {
            steering = Seek(target);
        }

        return steering;
    }
    Vector3 Wander()
    {
        wanderTimer += Time.deltaTime;
        float distanceToWanderPos = Vector3.Distance(transform.position, wanderPosition);
        if (wanderTimer >= wanderEvaluationTime || distanceToWanderPos < wanderDistanceToTarget)
        {
            wanderTimer = 0f;
            UpdateTargetPosForWandering();
        }
        return Seek(wanderPosition);
    }
    void UpdateTargetPosForWandering()
    {
        Vector3 position = transform.position;
        Vector3 targetPos = ObjectsManager.instance.GetRandomPosition();

        wanderPosition = targetPos;
    }
    Vector3 CollisionAvoidance()
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
}
