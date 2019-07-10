using UnityEngine;

public class FSM : MonoBehaviour
{

    protected Transform holderTransform;
    [SerializeField] protected GameObject[] wayPoints;
    //  [SerializeField] protected int indexOfWayPoints;
    protected float elapsedTime;

    [SerializeField] protected int health;
    protected bool isDead;

    protected virtual void Initialize() { }
    protected virtual void FSMUpdate() { }
    protected virtual void FSMFixedUpdate() { }

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        FSMUpdate();
    }

    private void FixedUpdate()
    {
        FSMFixedUpdate();
    }

}
