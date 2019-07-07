using UnityEngine;

public class FSM : MonoBehaviour
{


    protected Transform resourceTransform;
    [SerializeField]
    protected GameObject[] wayPoints;
    [SerializeField]
    protected int indexOfWayPoints;
    protected float shootRate;
    protected float elapsedTime;

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
