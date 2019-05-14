using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public enum States
    {
        Patrol,
        Hunt,
        Avoid
    }
    public States FSMState;
    ObstaclesAvoiding obsAvoid;
    ObjectFollowing pathFollow;
    Smell smellSense;
    public Transform huntedObject;
    Vector3 moveDir;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        FSMState = States.Patrol;
        obsAvoid = GetComponent<ObstaclesAvoiding>();
        pathFollow = GetComponent<ObjectFollowing>();
        smellSense = GetComponent<Smell>();
        smellSense.OnDetectSmell += SmellTrigger;
    }

    // Update is called once per frame
    void Update()
    {
        ParseStates();
        switch (FSMState)
        {
            case States.Patrol: UpdatePatrol(); break;
            case States.Hunt: UpdateHuntSmell(); break;
        }
        Steer();
    }

    void ParseStates()
    {
        if (huntedObject != null)
            FSMState = States.Hunt;
        else
            FSMState = States.Patrol;

    }
    void SmellTrigger()
    {
        huntedObject = smellSense.nearbyOdors[0].transform;
    }

    void UpdatePatrol()
    {
        pathFollow.UpdatePathFollow();
        moveDir = pathFollow.PathTarget - transform.position;
        
        Steer();
        //moveDir = pathFollow.PathTarget;
        //AvoidObstacles(moveDir);
    }
    void UpdateHuntSmell()
    {
        moveDir = huntedObject.position - transform.position;
        /*moveDir.Normalize();
        AvoidObstacles();
        var rot = Quaternion.LookRotation(moveDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, obsAvoid.RotationSpeed * Time.deltaTime);
        //transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        transform.position += transform.forward * pathFollow.CurrentSpeed;

        Debug.DrawLine(transform.position, huntedObject.position, Color.red);
        Debug.DrawLine(transform.position, moveDir, Color.blue);*/
    }
    void AvoidObstacles()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, obsAvoid.DistanceToAvoid, obsAvoid.Mask))
        {
            Vector3 hitNormal = hit.normal;
            hitNormal.y = 0.0f;
            moveDir = transform.forward + hitNormal * obsAvoid.RotationForce;
        }
        
    }

    void Steer()
    {
        moveDir.Normalize();
        AvoidObstacles();
        var rot = Quaternion.LookRotation(moveDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, obsAvoid.RotationSpeed * Time.deltaTime);
        //transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        transform.position += transform.forward * pathFollow.CurrentSpeed;
        //Debug.DrawLine(transform.position, huntedObject.position, Color.red);
        //Debug.DrawLine(transform.position, moveDir, Color.blue);
    }
}
