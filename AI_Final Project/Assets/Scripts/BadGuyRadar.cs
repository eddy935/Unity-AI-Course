using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BadGuyRadar : Sense
{
    [SerializeField]
    float detectionRange;
    [SerializeField]
    float detectionOffset;

    public List<Transform> goodGuysLocation = new List<Transform>();

    public delegate void OnDetection();
    public event OnDetection OnGoodGuyDetected;

    Vector3 DetectionCenterPos { get { return transform.position + transform.forward * detectionOffset; } }

    Transform target;

    private void Start()
    {


    }

    protected override void UpdateSense()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= DetectionRate)
        {
            SearchForGoodGuys();
            elapsedTime = 0;
        }
    }


    void SearchForGoodGuys()
    {
        goodGuysLocation = goodGuysLocation.Where(goodGuysLocation => Vector3.Distance(goodGuysLocation.transform.position, DetectionCenterPos) <= detectionRange).ToList();
        if (goodGuysLocation.Count > 0 && OnGoodGuyDetected != null)
            OnGoodGuyDetected();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(DetectionCenterPos, detectionRange);
        Gizmos.color = Color.red;
        if (target != null)
            Gizmos.DrawLine(transform.position, target.position);
    }

}
