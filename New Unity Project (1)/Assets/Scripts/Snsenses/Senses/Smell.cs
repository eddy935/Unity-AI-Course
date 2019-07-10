using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smell : Sense
{

    SphereCollider sphereCollider;
    float playerDis;
    Vector3 playerDir;


    protected override void Initialize()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    protected override void UpdateSense()
    {
        base.UpdateSense();
    }

    void OnTriggerEnter(Collider other)
    {
        
        Scent scent = other.GetComponent<Scent>();

        if (scent != null)
        {
            playerDis = Vector3.Distance(transform.position, other.transform.position);
            playerDir = other.transform.position - transform.position;

            if (playerDir.normalized.z > 0)
            {
                Debug.Log("Player in front");
            }

            else if (playerDir.normalized.x > 0)
            {
                Debug.Log("Player on the right");
            }

            else if (playerDir.normalized.x < 0)
            {
                Debug.Log("Player on the left");
            }

            else if (playerDir.normalized.z < 0)
            {
                Debug.Log("Player on the back");
            }

            Debug.Log("Is path to " + scent.gameObject.name + " blocked: " + RaycastToPlayer(scent));

            Debug.Log(scent.gameObject.name + "'s scent strenth is: " + scent.ScentStrenth);
        }
       
    }

     bool RaycastToPlayer( Scent scent)
    {
        Ray ray = new Ray(transform.position, playerDir * playerDis);
        RaycastHit hit;
        Debug.DrawRay(transform.position, ray.direction * 100,Color.yellow,2f);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.GetComponent<Aspect>())
            {
                if (hit.collider.GetComponent<Aspect>().aspectName == scent.Aspect.aspectName)
                    return false;
                else
                    return true;
            }
            else
                return true;
        }
        else
            return false;
    }
}
