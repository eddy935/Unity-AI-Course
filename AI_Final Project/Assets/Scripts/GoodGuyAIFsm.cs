using UnityEngine;

public class GoodGuyAIFsm : FSM
{
    public enum FSMState
    {
        Patrol,
        PickUpItem,
        EnemyDetected,
        Dead,
    }

    public FSMState curState;

    ObstaclesAvoiding obstacleAvoid;
    ObjectFollowing pathToFollow;

    GoodGuyRadar radar;
    private Transform pickUpTransform;
    Transform enemyTransforms;

    [SerializeField] float fromEnemyRange;

    Vector3 frwDirection;
    [SerializeField] float moveSpeed;
    [SerializeField] float rotSpeed;


    private void Start()
    {
        curState = FSMState.Patrol;
        obstacleAvoid = GetComponent<ObstaclesAvoiding>();
        pathToFollow = GetComponent<ObjectFollowing>();

        radar = GetComponent<GoodGuyRadar>();
        radar.OnDetectItem += DetectionTrigger;
        radar.OnEnemyDetected += DetectionTrigger;
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

            case FSMState.Dead:
                UpdateDeadState();
                break;

            default:
                break;
        }
        elapsedTime += Time.deltaTime;

        if (health <= 0)
        {
            curState = FSMState.Dead;
        }
    }

    private void MoveToPickUp()
    {
        frwDirection = pickUpTransform.position - transform.position;
        moveSpeed = pathToFollow.CurrentSpeed / 2;
    }

    private void PatrolState()
    {
        pathToFollow.UpdatePathFollow();
        frwDirection = pathToFollow.PathTarget - transform.position;

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
        pickUpTransform = radar.nearPickUps[0].transform;
        enemyTransforms = radar.badGuysLocation[0].transform;
    }

    protected void UpdateDeadState()
    {
        if (!isDead)
        {
            isDead = true;
            Destroy(gameObject, 1.5f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // Todo Projectiles health -= collision.gameObject.GetComponent<Bullet>().damage;
        }
    }
}
