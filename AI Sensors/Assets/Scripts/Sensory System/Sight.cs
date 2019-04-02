using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : Sense
{
    private int _fieldOfView = 45;
    private int _viewDistance = 100;
    private Transform _target;
    private Vector3 _rayDir;

    protected override void Initialize()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected override void UpdateSense()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= DetectionRate)
        {
            DetectAspect();
        }
    }

    private void DetectAspect()
    {
        RaycastHit hit;
        _rayDir = _target.position - transform.position;

        if ((Vector3.Angle(_rayDir, transform.forward)) < _fieldOfView)
        {
            if (Physics.Raycast(transform.position, _rayDir, out hit, _viewDistance))
            {
                Aspect aspect = hit.collider.GetComponent<Aspect>();

                if (aspect != null)
                {
                    //Check the aspect
                    if (aspect.aspectName == Aspect.AspectType.Player)
                    {
                        print("Player Detected");
                    }

                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isEditor || _target == null)
        {
            return;
        }
        Debug.DrawLine(transform.position, _target.position, Color.red);

        Vector3 frontRay = transform.position + (transform.forward * _viewDistance);
        Vector3 leftRay = Quaternion.Euler(0, _fieldOfView * 0.5f, 0) * frontRay;
        Vector3 rightRay = Quaternion.Euler(0, -_fieldOfView * 0.5f, 0) * frontRay;

        Debug.DrawLine(transform.position, frontRay, Color.green);
        Debug.DrawLine(transform.position, leftRay, Color.green);
        Debug.DrawLine(transform.position, rightRay, Color.green);
    }
}

