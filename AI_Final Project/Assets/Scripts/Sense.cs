using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sense : MonoBehaviour
{
    protected float elapsedTime = 0.0f;

    protected virtual void Initialize() { }
    protected virtual void UpdateSense() { }

    public Aspect.AspectType aspectName;
    public float DetectionRate { get; private set; }
    
    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = 0.0f;
        DetectionRate = 1;
        aspectName = Aspect.AspectType.BadGuys;
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSense();
    }
}
