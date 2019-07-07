using UnityEngine;

public class AIFsm : FSM
{
    public enum FSMState
    {
        None,
        Patrol,
        DetectResource,
        CollectResource,
        Dead,
    }

    public FSMState curState;

    [SerializeField]
    private float _moveSpeed;

    [SerializeField]
    private float _curRotSpeed;

    [SerializeField]
    private float _fromResourceRange;

    private int _health;
    private Rigidbody _rigidbody;

    protected override void Initialize()
    {
        //Personal variables
        curState = FSMState.Patrol;
        indexOfWayPoints = 0;
        _health = 100;
        //Variables inherited
        elapsedTime = 0.0f;
        shootRate = 3.0f;
        wayPoints = GameObject.FindGameObjectsWithTag("Way Point");
        resourceTransform = GameObject.FindGameObjectWithTag("Resource").transform;
        _rigidbody = GetComponent<Rigidbody>();
    }



    protected override void FSMUpdate()
    {


        switch (curState)
        {
            case FSMState.Patrol:
                UpdatePatrolState();
                break;
            case FSMState.DetectResource:
                UpdateDetectResourceState();
                break;

            case FSMState.CollectResource:
                UpdateCollectResourceState();
                break;
            default:
                break;
        }


    }

    //Patroling
    private void UpdatePatrolState()
    {
        if (Mathf.Abs(transform.position.x - wayPoints[indexOfWayPoints].transform.position.x) < 1 && Mathf.Abs(transform.position.z - wayPoints[indexOfWayPoints].transform.position.z) < 1)
        {
            FindNextWayPoint();
        }
        else if (Mathf.Abs(transform.position.x - resourceTransform.position.x) <= _fromResourceRange && Mathf.Abs(transform.position.z - resourceTransform.position.z) <= _fromResourceRange)
        {
            curState = FSMState.DetectResource;
        }

        Quaternion targetRot = Quaternion.LookRotation(wayPoints[indexOfWayPoints].transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, _curRotSpeed * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, wayPoints[indexOfWayPoints].transform.position, _moveSpeed * Time.deltaTime);
    }

    protected void FindNextWayPoint()
    {
        if (indexOfWayPoints == wayPoints.Length - 1)
        {
            indexOfWayPoints = 0;
        }
        else
        {
            indexOfWayPoints++;
        }
    }


    //Resource Detection & Collection
    private void UpdateDetectResourceState()
    {
        if (Mathf.Abs(transform.position.x - resourceTransform.position.x) <= _fromResourceRange && Mathf.Abs(transform.position.z - resourceTransform.position.z) <= _fromResourceRange)
        {
            curState = FSMState.CollectResource;
        }
        else if (Mathf.Abs(transform.position.x - resourceTransform.position.x) >= _fromResourceRange || Mathf.Abs(transform.position.z - resourceTransform.position.z) >= _fromResourceRange)
        {
            curState = FSMState.Patrol;
        }
        Quaternion targetRot = Quaternion.LookRotation(resourceTransform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, _curRotSpeed * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, resourceTransform.position, _moveSpeed * Time.deltaTime);
    }

    private void UpdateCollectResourceState()
    {
        if ((Mathf.Abs(transform.position.x - resourceTransform.position.x) >= _fromResourceRange
                                                                                  && Mathf.Abs(transform.position.z - resourceTransform.position.z) >= _fromResourceRange)
                                                                                   && (Mathf.Abs(transform.position.x - resourceTransform.position.x) <= _fromResourceRange
                                                                                    && Mathf.Abs(transform.position.z - resourceTransform.position.z) <= _fromResourceRange))
        {
            Quaternion targetRot = Quaternion.LookRotation(resourceTransform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, _curRotSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, resourceTransform.position, _moveSpeed * Time.deltaTime);
            curState = FSMState.CollectResource;
        }
        else if (Mathf.Abs(transform.position.x - resourceTransform.position.x) >= _fromResourceRange || Mathf.Abs(transform.position.z - resourceTransform.position.z) >= _fromResourceRange)
        {
            curState = FSMState.Patrol;
        }

    }


}





