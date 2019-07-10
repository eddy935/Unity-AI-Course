using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scent : MonoBehaviour
{

    [SerializeField] int _scentStrenth = 10;
    Aspect aspect;

    public int ScentStrenth { get => _scentStrenth; }
    public Aspect Aspect { get => aspect; }

    void Start()
    {

        if (GetComponent<Aspect>() != null)
        {
            aspect = GetComponent<Aspect>();
        }
        else
            aspect = null;

    }

    
}
