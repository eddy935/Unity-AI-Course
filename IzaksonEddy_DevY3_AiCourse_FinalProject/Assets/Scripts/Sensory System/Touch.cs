using UnityEngine;

public class Touch : Sense
{
    private void OnTriggerEnter(Collider other)
    {
        Aspect aspect = other.GetComponent<Aspect>();
        if (aspect != null)
        {
            if (aspect.aspectName == Aspect.AspectType.Resource)
            {
                print("Resource touched !!!");
            }
        }
    }
}
