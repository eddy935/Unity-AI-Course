using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarFollowing : MonoBehaviour
{
   // public CreatePath path;
    public float speed = 5.0f;
    public float mass = 5.0f;
    bool isLooping = false;
    MyAStarImplementaion myAstar;

    private float _curSpeed;
    private int _curPathIndex;
    private float _pathLength;
    private Vector3 _targetPoint;
    private Vector3 _velocity;

    private void Start()
    {
        myAstar = GetComponent<MyAStarImplementaion>();

        _curPathIndex = 0;
        _velocity = transform.forward;

        this.enabled = false;
    }

     void Update()
    {
        if(_pathLength != myAstar.Length)
        _pathLength = myAstar.Length;

        _curSpeed = speed * Time.deltaTime;
        _targetPoint = myAstar.GetPoint(_curPathIndex).position;

        if (Vector3.Distance(transform.position, _targetPoint) < 0.3f)
        {
            if (_curPathIndex < _pathLength - 1)
            {
                _curPathIndex++;
            }
            else if (isLooping)
            {
                _curPathIndex = 0;
            }
            else
            {
                return;
            }
        }

        if (_curPathIndex >= _pathLength)
        {
            return;
        }

        if (_curPathIndex >= _pathLength - 1 && !isLooping)
        {
            _velocity += Steer(_targetPoint, true);
        }
        else
        {
            _velocity += Steer(_targetPoint);
        }        

        transform.position += _velocity;

        // Nati
        if (myAstar.revese)
            transform.rotation = Quaternion.LookRotation(-_velocity);
        else
            transform.rotation = Quaternion.LookRotation(_velocity);
    }

    public Vector3 Steer(Vector3 target, bool bFinalPoint = false)
    {
        Vector3 desired_velocity = (target - transform.position);
        float dist = desired_velocity.magnitude;
        desired_velocity.Normalize();

        if (bFinalPoint && dist < 10.0f)
        {
            desired_velocity *= (_curSpeed * (dist / 10.0f));
        }
        else
        {
            desired_velocity *= _curSpeed;
        }

        Vector3 steeringForce = desired_velocity - _velocity;
        Vector3 acceleration = steeringForce / mass;
        return acceleration;
    }

    public void ResetCurrentIndex()
    {
        _curPathIndex = 0;
    }
}
