using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateMachine : MonoBehaviour
{

    [SerializeField] States currentShipState;
    [SerializeField] bool isAlive;
    [SerializeField] float movementSpeed = 0.3f;
    [SerializeField] bool hasWater;
    [SerializeField] Image waterDropImage;
    [SerializeField] Transform player;
    [SerializeField] Transform mars;
    [SerializeField] Transform earth;
    [SerializeField] Transform sun;
    [SerializeField] ParticleSystem explosionEffect;

    void Start()
    {
        currentShipState = States.ToMasrs;
        isAlive = true;
        waterDropImage.enabled = false;
        StartCoroutine(StateMAchine());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (currentShipState != States.Suicide)
            {
                currentShipState = States.Suicide;
                movementSpeed = 0.5f;
            }
        }

        waterDropImage.transform.LookAt(player.position);
    }

   IEnumerator StateMAchine()
    {
        while (isAlive)
        {
            switch (currentShipState)
            {
                case States.ToMasrs:
                    ToMars();
                    break;

                case States.Pickup:
                    yield return new WaitForSeconds(1);
                    Pickup();
                   
                    break;
                case States.Transport:
                    
                    Transport();
                    break;
                case States.Deliver:
                    yield return new WaitForSeconds(1);
                    Deliver();
                    currentShipState = States.ToMasrs;
                    break;
                case States.Suicide:
                    Suicide();
                    break;
            }

            yield return null;
        }
    }

    void ToMars()
    {
        transform.LookAt(mars);
        transform.position = Vector3.MoveTowards(transform.position, mars.position, movementSpeed);
    }

    void Pickup()
    {
        waterDropImage.enabled = true;
        hasWater = true;
        currentShipState = States.Transport;

    }

    void Transport()
    {
        transform.LookAt(earth);
        transform.position = Vector3.MoveTowards(transform.position, earth.position, movementSpeed);
    }

    void Deliver()
    {
        hasWater = false;
        waterDropImage.enabled = false;
        
    }

    void Suicide()
    {
        transform.LookAt(sun);
        transform.position = Vector3.MoveTowards(transform.position, sun.position, movementSpeed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Mars")
        {
            currentShipState = States.Pickup;
        }
        if (other.tag == "Earth")
        {
            currentShipState = States.Deliver;
        }
        if (other.tag == "Sun")
        {
            StopAllCoroutines();
            isAlive = false;
          ParticleSystem explosion =   Instantiate(explosionEffect, transform.position, transform.rotation);
            Destroy(explosion.gameObject, 3);
            Destroy(this.gameObject);
        }

    }
}

public enum States
{
    ToMasrs,
    Pickup,
    Transport,
    Deliver,
    Suicide
}
