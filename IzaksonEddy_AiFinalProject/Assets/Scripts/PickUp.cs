using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public static List<PickUp> pickupList;
    float posY;
    void Start()
    {
        if (pickupList == null)
            pickupList = new List<PickUp>();

        posY = transform.position.y;
        pickupList.Add(this);
    }
    void Update()
    {
        transform.Rotate(0, 0, transform.eulerAngles.x + 1);
        float newPosY = posY + Mathf.Sin(Time.time) * 0.5f;
        transform.localPosition = new Vector3(transform.position.x, newPosY, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        pickupList.Remove(this);
        Destroy(gameObject);
    }
}
