using UnityEngine;

public class ObstaclesAvoiding : MonoBehaviour
{
    [SerializeField]
    private LayerMask _mask;

    [SerializeField]
    private float _rotSpeed = 5.0f;
    [SerializeField]
    private float _mass = 5.0f;
    [SerializeField]
    private float _force = 10.0f;
    [SerializeField]
    private float _minimumDistToAvoid = 10.0f;

    private float _curSpeed;
    private Vector3 _targetPoint;

    public LayerMask Mask { get { return _mask; } }
    public float DistanceToAvoid { get { return _minimumDistToAvoid; } }
    public float RotationForce { get { return _force; } }
    public float RotationSpeed { get { return _rotSpeed; } }

    public void UpdateObstacleAvoid()
    {
        Vector3 dir = (_targetPoint - transform.position);
        dir.Normalize();
        AvoidObstacles(ref dir);

        if (Vector3.Distance(_targetPoint, transform.position) < 3.0f)
        {
            return;
        }

        var rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, _rotSpeed * Time.deltaTime);
        transform.position += transform.forward * _curSpeed;
    }

    public void AvoidObstacles(ref Vector3 dir)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, _minimumDistToAvoid, _mask))
        {
            Vector3 hitNormal = hit.normal;
            hitNormal.y = 0.0f;
            dir = transform.forward + hitNormal * _force;
        }
    }
}
