using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Odor : MonoBehaviour
{
    [Range(0, Mathf.Infinity)]
    public float odorStrength;

    public int CompareTo(Odor other)
    {
        throw new NotImplementedException();
    }
}