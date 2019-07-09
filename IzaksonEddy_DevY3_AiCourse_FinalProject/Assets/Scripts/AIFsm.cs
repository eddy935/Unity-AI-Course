using UnityEngine;

public class AIFsm : FSM
{
    public enum FSMState
    {
        Patrol,
        PickUpItem,
        EnemyDetected,

    }

    public FSMState curState;
    ObstaclesAvoiding obstacleAvoid;
    ObjectFollowing pathToFollow;

    Radar radar;
    private Transform pickUpSensed;

    Transform enemyTransforms;
    [SerializeField]
    float fromEnemyRange;


    Vector3 frwDirection;
    public float moveSpeed;

    private void Start()
    {
        curState = FSMState.Patrol;
        obstacleAvoid = GetComponent<ObstaclesAvoiding>();
        pathToFollow = GetComponent<ObjectFollowing>();

        radar = GetComponent<Radar>();
        radar.OnDetectItem += DetectionTrigger;



    }
    // Update is called once per frame
    void Update()
    {
        switch (curState)
        {
            case FSMState.Patrol:
                PatrolState();
                break;

            case FSMState.PickUpItem:
                MoveToPickUp();
                break;

            case FSMState.EnemyDetected:
                HandleEnemyDetection();
                break;



            default:
                break;
        }

        Navigate();
    }

    private void MoveToPickUp()
    {
        frwDirection = pickUpSensed.position - transform.position;
        moveSpeed = pathToFollow.CurrentSpeed / 2;
    }

    private void PatrolState()
    {
        pathToFollow.UpdatePathFollow();
        frwDirection = pathToFollow.PathTarget - transform.position;

        // if (Mathf.Abs(transform.position.x - enemyTransforms.position.x) <= fromEnemyRange && Mathf.Abs(transform.position.z - enemyTransforms.position.z) <= fromEnemyRange)
        // {
        //     curState = FSMState.EnemyDetected;
        // }
        //
        Navigate();
    }

    protected void HandleEnemyDetection()
    {
        if (Mathf.Abs(transform.position.x - enemyTransforms.position.x) >= fromEnemyRange || Mathf.Abs(transform.position.z - enemyTransforms.position.z) >= fromEnemyRange)
        {
            Quaternion targetRot = Quaternion.LookRotation(-(enemyTransforms.position - transform.position));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, moveSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, -enemyTransforms.position, moveSpeed * Time.deltaTime);
        }

        curState = FSMState.Patrol;
    }


    private void Navigate()
    {
        AvoidObstacle();
        var lookRotation = Quaternion.LookRotation(frwDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, obstacleAvoid.RotationSpeed * Time.deltaTime);
        transform.position += transform.forward * pathToFollow.CurrentSpeed;
    }

    private void AvoidObstacle()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, obstacleAvoid.DistanceToAvoid, obstacleAvoid.Mask))
        {
            Vector3 hitNormal = hit.normal;
            hitNormal.y = 0.0f;
            frwDirection = transform.forward + hitNormal * obstacleAvoid.RotationForce;
        }
    }

    void DetectionTrigger()
    {
        pickUpSensed = radar.nearPickUps[0].transform;
    }
}
