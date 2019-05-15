using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PickUpSensor : Sense
{
    [SerializeField]
    float detectionRange;
    [SerializeField]
    float detectionOffset;

    public List<PickUp> nearPickUps;

    public delegate void OnDetection();
    public event OnDetection OnDetectItem
        ;
    Vector3 DetectionCenterPos { get { return transform.position + transform.forward * detectionOffset; } }

    Transform player, target;

    protected override void UpdateSense()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= DetectionRate)
        {
            SearchForNearPickUps();
            elapsedTime = 0;
        }
    }

    void SearchForNearPickUps()
    {
        nearPickUps = PickUp.pickupList.Where(odor => Vector3.Distance(odor.transform.position, DetectionCenterPos) <= detectionRange).ToList();
        if (nearPickUps.Count > 0 && OnDetectItem != null)
            OnDetectItem();
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(DetectionCenterPos, detectionRange);
        Gizmos.color = Color.red;
        if (target != null)
            Gizmos.DrawLine(transform.position, target.position);
    }
}
