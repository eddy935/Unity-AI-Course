using UnityEngine;

public class BadGuyAIFsm : FSM
{
    public enum FSMState
    {
        Patrol,
        EnemyDetected,
        Attack,
        Dead,
    }

    public FSMState curState;
    ObstaclesAvoiding obstacleAvoid;
    ObjectFollowing pathToFollow;

    Radar radar;
    Transform goodGuyTransforms;

    [SerializeField] float fromTargetRangeDetection;

    [SerializeField] protected Transform turret;
    [SerializeField] private GameObject projectile;
    [SerializeField] protected Transform projectileSpwan;

    Vector3 frwDirection;
    public float moveSpeed;

    private void Start()
    {
        curState = FSMState.Patrol;
        obstacleAvoid = GetComponent<ObstaclesAvoiding>();
        pathToFollow = GetComponent<ObjectFollowing>();

        radar = GetComponent<Radar>();
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

            case FSMState.EnemyDetected:
                HandleEnemyDetection();
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


    private void PatrolState()
    {
        pathToFollow.UpdatePathFollow();
        frwDirection = pathToFollow.PathTarget - transform.position;

        Navigate();
    }

    protected void HandleEnemyDetection()
    {
        if (Mathf.Abs(transform.position.x - goodGuyTransforms.position.x) >= fromTargetRangeDetection || Mathf.Abs(transform.position.z - goodGuyTransforms.position.z) >= fromTargetRangeDetection)
        {
            Quaternion targetRot = Quaternion.LookRotation(goodGuyTransforms.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, moveSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, goodGuyTransforms.position, moveSpeed * Time.deltaTime);
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
        goodGuyTransforms = radar.badGuysLocation[0].transform;
    }
}
