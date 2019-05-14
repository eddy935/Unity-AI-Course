using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public static List<Collectible> allCoins;
    float posY;
    void Start()
    {
        if (allCoins == null)
            allCoins = new List<Collectible>();

        posY = transform.position.y;
        allCoins.Add(this);
    }
    void Update()
    {
        transform.Rotate(0, 0, transform.localEulerAngles.z + 1);
        float newPosY = posY + Mathf.Sin(Time.time) * 0.5f;
        transform.localPosition = new Vector3(transform.position.x, newPosY, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        allCoins.Remove(this);
        Destroy(gameObject);
    }
}
