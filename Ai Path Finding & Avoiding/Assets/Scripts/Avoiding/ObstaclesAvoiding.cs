using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesAvoiding : MonoBehaviour
{
    [SerializeField]
    private LayerMask _mask;
    [SerializeField]
    private float _speed = 20.0f;
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

    private void Update()
    {
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit, 100.0f))
        {
            _targetPoint = hit.point;
        }

        Vector3 dir = (_targetPoint - transform.position);
        dir.Normalize();
        AvoidObstacles(ref dir);

        if (Vector3.Distance(_targetPoint, transform.position) < 3.0f)
        {
            return;
        }

        _curSpeed = _speed * Time.deltaTime;
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

