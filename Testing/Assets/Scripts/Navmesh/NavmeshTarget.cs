using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NavmeshTarget : MonoBehaviour
{
    NavMeshAgent[] meshAgents;
    Transform targetTransform;
    void Start()
    {
        meshAgents = FindObjectsOfType<NavMeshAgent>();
        targetTransform = GetComponent<Transform>();
    }

    void UpdateTarget(Vector3 pos)
    {
        foreach(NavMeshAgent agent in meshAgents)
        {
            agent.destination = pos;
        }
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                Vector3 newPos = hit.point;
                UpdateTarget(newPos);
                transform.position = newPos;
            }
        }
    }
}
