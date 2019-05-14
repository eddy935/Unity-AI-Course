using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSimpleControl : MonoBehaviour
{
    [SerializeField]
    private Transform _target;
    [SerializeField]
    private float _movementSpeed;
    [SerializeField]
    private float _rotSpeed;


    private void Update()
    {
        if (Vector3.Distance(transform.position,_target.position) < 5.0f)
        {
            return;
        }

        Vector3 targetPos = _target.position;
        targetPos.y = transform.position.y;
        Vector3 dirRot = targetPos - transform.position;
        Quaternion targetRot = Quaternion.LookRotation(dirRot);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, _rotSpeed * Time.deltaTime);
        transform.Translate(new Vector3(0, 0, _movementSpeed * Time.deltaTime));
    }
}
