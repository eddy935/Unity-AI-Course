using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Radar : Sense
{
    [SerializeField]
    float detectionRange;
    [SerializeField]
    float detectionOffset;

    public List<PickUp> nearPickUps;
    public List<Transform> enemyLocations = new List<Transform>();

    public delegate void OnDetection();
    public event OnDetection OnDetectItem;
    public event OnDetection OnEnemyDetected;

    Vector3 DetectionCenterPos { get { return transform.position + transform.forward * detectionOffset; } }

    Transform target;

    private void Start()
    {
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Bad Guys"))
        {
            enemyLocations.Add(enemy.transform);
        }
    }

    protected override void UpdateSense()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= DetectionRate)
        {
            SearchForNearPickUps();
            SearchForNearEnemies();
            elapsedTime = 0;
        }
    }

    void SearchForNearPickUps()
    {
        nearPickUps = PickUp.pickupList.Where(pickUpLocation => Vector3.Distance(pickUpLocation.transform.position, DetectionCenterPos) <= detectionRange).ToList();
        if (nearPickUps.Count > 0 && OnDetectItem != null)
            OnDetectItem();
    }

    void SearchForNearEnemies()
    {
        if (enemyLocations.Count > 0 && OnEnemyDetected != null)
            OnEnemyDetected();
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
