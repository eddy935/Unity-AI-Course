using System;
using UnityEngine;

public class AIFsm : MonoBehaviour
{
    public enum FSMState
    {
        Patrol,
        PickUpItem,
    }

    public FSMState curState;
    ObstaclesAvoiding obstacleAvoid;
    ObjectFollowing pathToFollow;

    PickUpSensor pickUpSensor;

    public Transform pickUpSensed;
    Vector3 frwDirection;
    public float moveSpeed;

    private void Start()
    {
        curState = FSMState.Patrol;
        obstacleAvoid = GetComponent<ObstaclesAvoiding>();
        pathToFollow = GetComponent<ObjectFollowing>();

        pickUpSensor = GetComponent<PickUpSensor>();
        pickUpSensor.OnDetectItem += DetectionTrigger;
    }
    // Update is called once per frame
    void Update()
    {
        CheckStates();
        switch (curState)
        {
            case FSMState.Patrol:
                PatrolState();
                break;

            case FSMState.PickUpItem:
                MoveToPickUp();
                break;
        }

        Navigate();
    }

    private void MoveToPickUp()
    {
        frwDirection = pickUpSensed.position - transform.position;
    }

    private void PatrolState()
    {
        pathToFollow.UpdatePathFollow();
        frwDirection = pathToFollow.PathTarget - transform.position;

        Navigate();
    }

    void CheckStates()
    {
        if (pickUpSensed != null)
            curState = FSMState.PickUpItem;
        else
            curState = FSMState.Patrol;

    }
  

    private void Navigate()
    {
        AvoidObstacle();
        var lookRotation = Quaternion.LookRotation(frwDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, obstacleAvoid.RotationSpeed * Time.deltaTime);
        transform.position += transform.forward * pathToFollow.CurrentSpeed;
    }

    private void AvoidObstacle()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, obstacleAvoid.DistanceToAvoid, obstacleAvoid.Mask))
        {
            Vector3 hitNormal = hit.normal;
            hitNormal.y = 0.0f;
            frwDirection = transform.forward + hitNormal * obstacleAvoid.RotationForce;
        }
    }

    void DetectionTrigger()
    {
        pickUpSensed = pickUpSensor.nearPickUps[0].transform;
    }
}
