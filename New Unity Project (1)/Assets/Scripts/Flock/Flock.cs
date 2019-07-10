using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    internal FlockController controller;

    [HideInInspector]
    public new Rigidbody rb;

   [SerializeField] float counter = 0;
    //    Vector3 randowmizer = new Vector3(Random.Range(5, 10), 0, Random.Range(5, 10));

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        if (controller)
        {
            
            Vector3 relativePos = Steer() * Time.deltaTime;

            if (counter < Random.Range(3,7))
                counter += Time.deltaTime;
            else
            {
                StartCoroutine(RandomMovement());
                counter = 0;
            }


            if (relativePos != Vector3.zero)
            {
                rb.velocity = relativePos;
            }

            float speed = rb.velocity.magnitude;

            if (speed > controller.maxVelocity)
            {
                rb.velocity = rb.velocity.normalized * controller.maxVelocity;
            }
            else if (speed < controller.minVelocity)
            {
                rb.velocity = rb.velocity.normalized * controller.minVelocity;
            }

            transform.LookAt(controller.target);

        }
    }
    

    private Vector3 Steer()
    {
        Vector3 center = controller.flockCenter - transform.localPosition;
        Vector3 velocity = controller.flockVelocity - rb.velocity;
        Vector3 follow = controller.target.localPosition - transform.localPosition;
        Vector3 separation = Vector3.zero;

        foreach (Flock flock in controller.flockList)
        {
            if (flock != this)
            {
                Vector3 relativePos = transform.localPosition -flock.transform.localPosition;
                separation += relativePos / (relativePos.sqrMagnitude);
            }
        }

        Vector3 randomize = new Vector3((Random.value * 2) - 1, (Random.value * 2) - 1, (Random.value * 2) - 1);
        randomize.Normalize();
        return (controller.centerWeight * center +
                controller.velocityWeight * velocity +
                controller.separationWeight * separation +
                controller.followWeight * follow +
                controller.randomizeWeight * randomize);
    }

    void RandomizeSteer(Vector3 relativePos)
    {
        Vector3 randowmizer = new Vector3(Random.Range(5, 10), 0, Random.Range(5, 10));
        relativePos += randowmizer;
    }

    IEnumerator RandomMovement()
    {
        float counterRand = 0;
        Vector3 randowmizer = new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1));
        while (counterRand < 0.5f)
        {
            transform.Translate(randowmizer * Time.deltaTime);
            counterRand += Time.deltaTime;
            

            yield return null;
        }
       
        
    }
}
