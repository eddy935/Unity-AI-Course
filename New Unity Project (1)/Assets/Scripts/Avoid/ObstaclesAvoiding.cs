using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesAvoiding : MonoBehaviour {

    [SerializeField]
    private LayerMask _mask;
    [SerializeField]
    private float _speed = 20.0f;
    [SerializeField]
    private float _rotSpeed = 5.0f;
    [SerializeField]
    private float _mass = 5.0f;
    [SerializeField]
    private float _force = 10.0f;
    [SerializeField]
    private float _minimumDistToAvoid = 10.0f;

    private float _curSpeed;
    private Vector3 _targetPoint;

    ObjectFollowing objectFollowing;

    void Start()
    {
        objectFollowing = GetComponent<ObjectFollowing>();
    }

    private void FixedUpdate()
    {
      /*  
        RaycastHit hit;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit, 100.0f))
        {
           
            _targetPoint = new Vector3(hit.point.x,transform.position.y,hit.point.z);
        }

        Vector3 dir = (_targetPoint - transform.position);
        dir.Normalize();
        
        if (Vector3.Distance(_targetPoint, transform.position) < 3.0f)
        {
            return;
        }
        */

        AvoidObstacles();

        /*
        _curSpeed = _speed * Time.deltaTime;
        var rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, _rotSpeed * Time.deltaTime);
        transform.position += transform.forward * _curSpeed;
        */
        
    }

    public void AvoidObstacles()
    {
        RaycastHit hit;
        /*
        if (Physics.Raycast(transform.position, transform.forward, out hit, _minimumDistToAvoid, _mask))
        {
            Debug.DrawRay(transform.position, transform.forward * _minimumDistToAvoid, Color.red);
            Vector3 hitNormal = hit.normal;
            hitNormal.y = 0.0f;
            dir = transform.forward + hitNormal * _force;
        }
        */
        
        if (Physics.SphereCast(transform.position,_minimumDistToAvoid,transform.forward,out hit,_minimumDistToAvoid,_mask))
        {
            Debug.Log("Hit obstacle: " + hit.collider.name);
            Vector3 hitNormal = hit.normal;
            hitNormal.y = 0.0f;
            transform.Translate(-transform.right + hitNormal * _force * Time.deltaTime);
            objectFollowing.enabled = false;
        }
        else
        {
            if (!objectFollowing.enabled)
            {
                objectFollowing.CurrentIndex = objectFollowing.GetClosestWaypointIndex();
                objectFollowing.enabled = true;
            }
            
        }
        
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _minimumDistToAvoid);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Obstacle")
        {
            Debug.Log("Hit obstacle: " + other.name);
            GetComponent<Rigidbody>().AddForce(Vector3.up * 20,ForceMode.Impulse);
         //   Debug.Log("current index: " + objectFollowing.CurrentIndex);
        }
    }

}
