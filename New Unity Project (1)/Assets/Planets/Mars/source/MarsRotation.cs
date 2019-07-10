using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarsRotation : MonoBehaviour
{

    [SerializeField] float rotationSpeed = 10;
    [SerializeField] RotationDirection rotationDirection;
    Vector3 rotateDirection;

    void Start()
    {
        switch (rotationDirection)
        {
            case RotationDirection.Right:
                rotateDirection = Vector3.up;
                break;
            case RotationDirection.Left:
                rotateDirection = -Vector3.up;
                break;
            case RotationDirection.Up:
                rotateDirection = Vector3.forward;
                break;
            case RotationDirection.Down:
                rotateDirection = -Vector3.forward;
                break;
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotateDirection * rotationSpeed * Time.deltaTime);
    }
}

enum RotationDirection
{
    Right,
    Left,
    Up,
    Down
}
