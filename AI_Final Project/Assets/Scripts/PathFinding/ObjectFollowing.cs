using UnityEngine;

public class ObjectFollowing : MonoBehaviour
{
    public CreatePath path;
    public float speed;
    public float mass;
    public bool isLooping = true;
    private float _curSpeed;
    private int _curPathIndex;
    private int _pathLength;
    private Vector3 _targetPoint;
    private Vector3 _velocity;

    public float PathSpeed { get { return _curSpeed; } }
    public float CurrentSpeed { get { return _curSpeed; } }
    public Vector3 Velocity { get { return _velocity; } }
    public Vector3 PathTarget { get { return _targetPoint; } }
    private void Start()
    {
        _pathLength = path.Length;
        _curPathIndex = 0;
        _velocity = transform.forward;
    }

    public void UpdatePathFollow()
    {
        _curSpeed = speed * Time.deltaTime;
        _targetPoint = path.GetPoint(_curPathIndex);

        if (Vector3.Distance(transform.position, _targetPoint) < path.Radius)
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
   
    }

    void ReverseIndex()
    {
        _curPathIndex = (_pathLength - 1) - _curPathIndex;
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
}
