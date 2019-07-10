using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridFollow : MonoBehaviour
{

    public Transform player;

    float gridToPlayerDis;
  public  float movementSpeed = 0.5f;

    void Start()
    {
        gridToPlayerDis = Vector3.Distance(transform.position, player.position);
    }

    void Update()
    {

        if (Vector3.Distance(transform.position, player.position) > gridToPlayerDis)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, movementSpeed);
        }
        else
            return;
    }
}
