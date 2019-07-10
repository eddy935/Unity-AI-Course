using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObstacle : MonoBehaviour {

    public Vector3 movementDirection;
    [SerializeField] private float speed;
    private Vector3 startPosition;
    private bool backToStartPosition;
    private Vector3 moveToPosition;
    private float minDistance = 0.3f;

    void Start()
    {
        startPosition = transform.position;
        backToStartPosition = false;
        moveToPosition = startPosition + movementDirection;
    }
	
	void Update ()
    {
        if (Vector3.Distance(transform.position, moveToPosition) < minDistance)
            backToStartPosition = true;
        else if (Vector3.Distance(transform.position, startPosition) < minDistance)
            backToStartPosition = false;


        if (backToStartPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, moveToPosition, speed * Time.deltaTime);
        }
	}
}
