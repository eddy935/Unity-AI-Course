using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : MonoBehaviour
{
    private Vector3 _targetPos;
    [SerializeField]
    private float _movementSpeed = 5.0f;
    [SerializeField]
    private float _rotSpeed = 2.0f;
    private float _minX, _maxX, _minZ, _maxZ;

    [SerializeField] Transform prefabPos;
    [SerializeField] Vector3 boundsMax;
    [SerializeField] Vector3 boundsMin;

    private void Start()
    {
        _minX = -16.0f;
        _maxX = 16.0f;
        _minZ = -16.0f;
        _maxZ = 16.0f;

        boundsMax = new Vector3(prefabPos.position.x + _maxX, 0.5f, prefabPos.position.z + _maxZ);
        boundsMin = new Vector3(prefabPos.position.x + _minX, 0.5f, prefabPos.position.z + _minZ);

        _targetPos = prefabPos.position;
        GetNextPosition();
       
    }

    private void Update()
    {
        if (Vector3.Distance(_targetPos, transform.position) <= 5.0f)
        {
            GetNextPosition();
        }

        Quaternion tarRot = Quaternion.LookRotation(_targetPos - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, tarRot, _rotSpeed * Time.deltaTime);
        transform.Translate(new Vector3(0, 0, _movementSpeed * Time.deltaTime));
    }

    private void GetNextPosition()
    {
        _targetPos += new Vector3(Random.Range(_minX, _maxX), 0.5f, Random.Range(_minZ, _maxZ));

        if(_targetPos.x > boundsMax.x || _targetPos.z > boundsMax.z)
        {
            _targetPos = boundsMax;
        }
        if (_targetPos.x < boundsMin.x || _targetPos.z < boundsMin.z)
        {
            _targetPos = boundsMin;
        }
    }

}
