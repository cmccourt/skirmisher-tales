using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteManager: MonoBehaviour
{
    
    public GameObject[] switches;
    public List<Transform[]> activeWaypoints = new List<Transform[]>();
    public List<GameObject> activePaths = new List<GameObject>();
    public List<GameObject[]> allPaths = new List<GameObject[]>();
    
    void Start()
    {
        for(int i= 0; i < switches.Length; i++)
        {
           PathSwitch switchPost = switches[i].GetComponent<PathSwitch>();
           activeWaypoints.Add(switchPost.activeWaypoints);
            //Debug.Log("Active Waypoints: " + activeWaypoints[i]);
            allPaths.Add(switchPost.paths);
            activePaths.Add(switchPost.activePath);
        }
    }

    
    public void SetCollisions(Collider2D unitColl, int pathIndex)
    {
        foreach(GameObject path in allPaths[pathIndex])
        {
            if (activePaths.Contains(path))
            {
                foreach (Collider2D groundColl in path.GetComponentsInChildren<Collider2D>())
                {
                    Physics2D.IgnoreCollision(unitColl, groundColl,false);
                }
            }
            else
            {
                foreach(Collider2D groundColl in path.GetComponentsInChildren<Collider2D>())
                {
                    Physics2D.IgnoreCollision(unitColl, groundColl);
                }
            }
        }
    }

    public void ChangeActivePaths(GameObject oldPath, GameObject newPath, Transform[] pActiveWaypoints)
    {
        int index = activePaths.IndexOf(oldPath);
        activePaths[index] = newPath;
        activeWaypoints[index] = pActiveWaypoints;
    }
}
