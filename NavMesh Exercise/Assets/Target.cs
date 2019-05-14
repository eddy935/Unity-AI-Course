using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Target : MonoBehaviour
{
    NavMeshAgent[] meshAgents;
    Transform targetTransform; 
    
        // Start is called before the first frame update
    void Start()
    {
        meshAgents = FindObjectsOfType(typeof(NavMeshAgent)) as NavMeshAgent[];
        targetTransform = GetComponent<Transform>();
    }

    void UpdateTarget(Vector3 pos)
    {
        foreach (NavMeshAgent navMeshAgent in meshAgents)
        {
            navMeshAgent.destination = pos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;

            if (Physics.Raycast(ray.origin, ray.direction, out raycastHit))
            {
                Vector3 newPos = raycastHit.point;
                UpdateTarget(newPos);
                targetTransform.position = newPos;
            }
        }
    }
}
