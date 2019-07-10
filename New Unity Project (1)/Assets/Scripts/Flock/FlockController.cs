using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockController : MonoBehaviour
{
    public float minVelocity = 1;
    public float maxVelocity = 8;
    public int flockSize = 20;
    public float centerWeight = 1;
    public float velocityWeight = 1;
    public float separationWeight = 1;
    public float followWeight = 1;
    public float randomizeWeight = 1;
    public Flock prefab;
    public Transform target;

    [HideInInspector]
    public Vector3 flockCenter;
    [HideInInspector]
    public Vector3 flockVelocity;

    public ArrayList flockList = new ArrayList();

    private void Start()
    {
        for (int i = 0; i < flockSize; i++)
        {
            Vector3 nextPosition = new Vector3(Random.Range(5, 10), 0, Random.Range(5, 10));
            Flock flock = Instantiate(prefab, transform.position + nextPosition, transform.rotation) as Flock;
            flock.transform.parent = transform;
            flock.controller = this;
            flockList.Add(flock);
        }
    }

    private void Update()
    {
        Vector3 center = Vector3.zero;
        Vector3 velocity = Vector3.zero;
        foreach (Flock flock in flockList)
        {
            center += flock.transform.localPosition;
            velocity += flock.rb.velocity;
        }

        flockCenter = center / flockSize;
        flockVelocity = velocity / flockSize;
    }
}
