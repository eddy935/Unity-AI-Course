using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{

     GameObject player;

    public delegate void Action();
    public static event Action OnDestroyTarget;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Start");
    }

    void Update()
    {
        if(Vector3.Distance(transform.position,player.transform.position) < 5f)
        {
            Destroy(this.gameObject);
        }
    }
    
    void OnDestroy()
    {
        if (OnDestroyTarget != null)
            OnDestroyTarget();
    }
    
}
