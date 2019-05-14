using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Smell : Sense
{
    [SerializeField]
    float smellDetectionRange;
    [SerializeField]
    float smellDetectionOffset;
    [SerializeField]
    float odorSensitivity;
    public List<Collectible> nearbyOdors;
    Dictionary<Odor, float> odorObjects;

    public delegate void OnSmell();
    public event OnSmell OnDetectSmell
        ;
    Vector3 SmellDetectionCenterPosition { get { return transform.position + transform.forward * smellDetectionOffset; } }

    Transform player, target;

    protected override void UpdateSense()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= DetectionRate)
        {
            DetectNearbyOdors();
            elapsedTime = 0;
        }
    }

    void DetectNearbyOdors()
    {
        nearbyOdors = Collectible.allCoins.Where(odor => Vector3.Distance(odor.transform.position, SmellDetectionCenterPosition) <= smellDetectionRange).ToList();
        if (nearbyOdors.Count > 0 && OnDetectSmell != null)
            OnDetectSmell();
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(SmellDetectionCenterPosition, smellDetectionRange);
        Gizmos.color = Color.red;
        if (target != null)
            Gizmos.DrawLine(transform.position, target.position);
    }
}
