using UnityEngine;

public class AIFsm : FSM
{
    public enum FSMState
    {
        None,
        Patrol,
        PickUpItem,
    }

    public FSMState curState;

    [SerializeField]

    private float _curSpeed;
    private float _curRotSpeed;
    private Rigidbody _rigidbody;

    protected override void Initialize()
    {
        //Personal variables
        curState = FSMState.Patrol;
        _curSpeed = 15.0f;
        _curRotSpeed = 2.0f;
        //Variables inherited
        elapsedTime = 0.0f;
        wayPoints = GameObject.FindGameObjectsWithTag("Way Point");
        FindNextPoint();
        pickUpTransform = GameObject.FindGameObjectWithTag("PickUp").transform;
        _rigidbody = GetComponent<Rigidbody>();
    }

    protected override void FSMUpdate()
    {
        switch (curState)
        {
            case FSMState.Patrol:
                UpdatePatrolState();
                break;
            case FSMState.PickUpItem:
                UpdateHuntState();
                break;

            default:
                break;
        }

        elapsedTime += Time.deltaTime;


    }

    protected void UpdatePatrolState()
    {
        if (Mathf.Abs(transform.position.x - wayPoints[indexOfWayPoints].transform.position.x) < 1 && Mathf.Abs(transform.position.z - wayPoints[indexOfWayPoints].transform.position.z) < 1)
        {
            FindNextPoint();
        }
        else if (Mathf.Abs(transform.position.x - pickUpTransform.position.x) <= 5.0f && Mathf.Abs(transform.position.z - pickUpTransform.position.z) <= 5.0f)
        {
            curState = FSMState.PickUpItem;
        }

        Quaternion targetRot = Quaternion.LookRotation(wayPoints[indexOfWayPoints].transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, _curRotSpeed * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, wayPoints[indexOfWayPoints].transform.position, _curSpeed * Time.deltaTime);
    }

    protected void FindNextPoint()
    {
        int randomIndex = GetRandomIndex();

        if (randomIndex == indexOfWayPoints)
        {
            if (indexOfWayPoints == wayPoints.Length - 1)
            {
                indexOfWayPoints--;
            }
            else
            {
                indexOfWayPoints++;
            }
        }
        else
        {
            indexOfWayPoints = randomIndex;
        }
    }

    private int GetRandomIndex()
    {
        return Random.Range(0, wayPoints.Length - 1);
    }

    protected void UpdateHuntState()
    {
        if (Mathf.Abs(transform.position.x - pickUpTransform.position.x) >= 5.0f || Mathf.Abs(transform.position.z - pickUpTransform.position.z) >= 5.0f)
        {
            curState = FSMState.Patrol;
        }
        Quaternion targetRot = Quaternion.LookRotation(pickUpTransform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, _curRotSpeed * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, pickUpTransform.position, _curSpeed * Time.deltaTime);
    }

    protected void UpdateAttackState()
    {
        if ((Mathf.Abs(transform.position.x - pickUpTransform.position.x) >= 3.0f && Mathf.Abs(transform.position.z - pickUpTransform.position.z) >= 3.0f) && (Mathf.Abs(transform.position.x - pickUpTransform.position.x) <= 5.0f && Mathf.Abs(transform.position.z - pickUpTransform.position.z) <= 5.0f))
        {
            Quaternion targetRot = Quaternion.LookRotation(pickUpTransform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, _curRotSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, pickUpTransform.position, _curSpeed * Time.deltaTime);

        }
        else if (Mathf.Abs(transform.position.x - pickUpTransform.position.x) >= 5.0f || Mathf.Abs(transform.position.z - pickUpTransform.position.z) >= 5.0f)
        {
            curState = FSMState.Patrol;
        }
    }


}
