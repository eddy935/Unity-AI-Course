using UnityEngine;
using System.Collections;

public class MyAStarImplementaion : MonoBehaviour
{
    private Transform startPos, endPos;
    public Node startNode { get; set; }
    public Node goalNode { get; set; }

    public ArrayList pathArray;
    public bool revese;
    public int Length{get { return pathArray.Count - 1; } }

    public Node GetPoint(int index)
    {
            return (Node)pathArray[index];     
    }

    [SerializeField] bool followindAstar;

    GameObject objStartCube;
    GameObject[] targets;
    GameObject closestTarget;

    ObjectFollowing objectFollowing;
    AStarFollowing aStarFollowing;

    private float elapsedTime = 0.0f;
    public float intervalTime = 1.0f; //Interval time between path finding

    void OnEnable()
    {
        Target.OnDestroyTarget += SwitchbackToRegularPath;
    }

    void OnDisable()
    {
        Target.OnDestroyTarget -= SwitchbackToRegularPath;
    }

    void Start () 
    {
        objStartCube = GameObject.FindGameObjectWithTag("Start");
        targets = GameObject.FindGameObjectsWithTag("End");

        objectFollowing = GetComponent<ObjectFollowing>();
        aStarFollowing = GetComponent<AStarFollowing>();
        followindAstar = false;
        //AStar Calculated Path
        pathArray = new ArrayList();
      //  FindPath();
	}
	
	// Update is called once per frame
	void Update () 
    {
        /*
        elapsedTime += Time.deltaTime;

        if(elapsedTime >= intervalTime)
        {
            elapsedTime = 0.0f;
            FindPath();
        }
        */

        if (Input.GetKeyDown(KeyCode.Space) && !followindAstar)
        {
           // objectFollowing.enabled = false;
            FindPath();
            followindAstar = true;
            aStarFollowing.enabled = followindAstar;
        }


	}

    void FindPath()
    {

        closestTarget = FindClosestTarget();

        if (closestTarget != null)
        {
            startPos = objStartCube.transform;
            endPos = closestTarget.transform;

            //Assign StartNode and Goal Node
            startNode = new Node(GridManager.instance.GetGridCellCenter(GridManager.instance.GetGridIndex(startPos.position)));
            goalNode = new Node(GridManager.instance.GetGridCellCenter(GridManager.instance.GetGridIndex(endPos.position)));

            pathArray = AStar.FindPath(startNode, goalNode);
        }
        else
        {
            Debug.Log("There are no more targets");
            return;
        }
            
    }

    void OnDrawGizmos()
    {
        if (pathArray == null)
            return;

        if (pathArray.Count > 0)
        {
            int index = 1;
            foreach (Node node in pathArray)
            {
                if (index < pathArray.Count)
                {
                    Node nextNode = (Node)pathArray[index];
                    Debug.DrawLine(node.position, nextNode.position, Color.green);
                    index++;
                }
            };
        }
    }

    GameObject FindClosestTarget()
    {
        
        float closestDis = 100f;
        float distance;
        GameObject closestTar = null;

        foreach(GameObject target in targets)
        {
            if (target != null)
            {
                distance = Vector3.Distance(transform.position, target.transform.position);

                if (distance < closestDis)
                {
                    closestDis = distance;
                    closestTar = target;
                }
            }
        }

        if(closestTar != null)
        Debug.Log("Closest target is: " + closestTar.name);
        else
        {
            Debug.Log("There are no more targets");
            return null;
        }

        return closestTar;
    }

    void SwitchbackToRegularPath()
    {
        followindAstar = false;
        aStarFollowing.ResetCurrentIndex();
        aStarFollowing.enabled = false;
      //  objectFollowing.enabled = true;
    }
   
}