using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FuzzyLogicAi : MonoBehaviour
{

    public AnimationCurve fuleTank;
    [SerializeField]
    float fule = 100;
    public AnimationCurve tired;
    [SerializeField]
    float sleepy = 100;

    [SerializeField]
    float fulePercent;
    [SerializeField]
    float tiredPercent;


    [SerializeField] float speed = 15;
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;
    [SerializeField] Transform gasStation;
    [SerializeField]  Transform home;
    Vector3 direction;
    public bool goToA;
    public bool needFule;
    public bool needSleep;

    void Start()
    {
        
    }

    
    void Update()
    {
       if(Vector3.Distance(transform.position,pointA.position) < 5)
        {
            goToA = false;
        } 
       else if(Vector3.Distance(transform.position, pointB.position) < 5)
        {
            goToA = true;
        }

        if (needFule)
        {
            GoRefule();
        }
        if (needSleep)
        {
            GoSleep();
        }

        if (goToA && !needFule && !needSleep)
        {
            fule -= Time.deltaTime * 2;
            sleepy -= Time.deltaTime;

            FlyToA();

            EvaluateCurves();
            TestPrint();
        }
        else if (!goToA && !needFule && !needSleep)
        {

            FlyToB();
            EvaluateCurves();
            TestPrint();
        }        
    }

    void EvaluateCurves()
    {
        fulePercent = fuleTank.Evaluate(fule);
        tiredPercent = tired.Evaluate(sleepy);
    }

    void TestPrint()
    {
        
         if(tiredPercent < 0.3f)
        {
            Debug.Log("Go sleep");
            needSleep = true;
            sleepy = 100f;
        }
        else if (fulePercent < 0.3f)
        {
            Debug.Log("Go refule");
            needFule = true;
            fule = 100f;
        }
        else if(fulePercent > 0.5f && tiredPercent <= 0.5f)
        {
            Debug.Log("Might sleep");
        }
        else if(fulePercent < 0.5f && tiredPercent > 0.5f)
        {
            Debug.Log("Might refule");
        }
    }

    void FlyToA()
    {
        fule -= Time.deltaTime * 2;
        sleepy -= Time.deltaTime;

        direction = (pointA.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 1f * Time.deltaTime);
    }

    void FlyToB()
    {
        fule -= Time.deltaTime * 2;
        sleepy -= Time.deltaTime;

        direction = (pointB.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 1f * Time.deltaTime);
    }

    void GoRefule()
    {
        direction = (gasStation.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 1f * Time.deltaTime);

        if (Vector3.Distance(transform.position, gasStation.position) < 3)
        {
            needFule = false;
        }
    }

    void GoSleep()
    {
        direction = (home.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 1f * Time.deltaTime);

        if(Vector3.Distance(transform.position,home.position) < 3)
        {
            needSleep = false;
        }
    }
}
