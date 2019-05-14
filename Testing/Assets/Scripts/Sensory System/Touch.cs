using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch : Sense
{
    private void OnTriggerEnter(Collider other)
    {
        Aspect aspect = other.GetComponent<Aspect>();
        if (aspect != null)
        {
            if (aspect.aspectName == Aspect.AspectType.Player)
            {
                print("Player Touch Detected");
            }
        }
    }
}
