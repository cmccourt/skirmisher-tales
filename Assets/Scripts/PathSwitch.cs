using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathSwitch : MonoBehaviour
{
    public GameObject[] paths;
    public GameObject activePath;
    public Transform[] activeWaypoints;
    public GameObject dirArrow;
    private List<Transform> activeWpList = new List<Transform>();
    
    private RouteManager routeMan;
    private int activePathIndex;
    private SpriteRenderer rend;
    // Start is called before the first frame update
    void Awake()
    {
        routeMan = FindObjectOfType<RouteManager>();
        rend = GetComponent<SpriteRenderer>();

        GenerateActiveWaypoints(activePath);
        activePathIndex = System.Array.IndexOf(paths, activePath);
    }

    private void OnMouseOver()
    {
        rend.color = Color.green;
        if (Input.GetMouseButtonDown(0))
        {
            int orgPathIndex = activePathIndex;
            activePathIndex++;
            if(activePathIndex == paths.Length)
            {
                activePathIndex = 0;
            }
            activePath = paths[activePathIndex];
            GenerateActiveWaypoints(activePath);
            routeMan.ChangeActivePaths(paths[orgPathIndex], activePath, activeWaypoints);
        }
    }

    void GenerateActiveWaypoints(GameObject path)
    {
        activeWpList.Clear();
        for (int i = 0; i < path.transform.childCount; i++)
        {
            activeWpList.Add(path.transform.GetChild(i).GetChild(0).GetComponent<Transform>());
           // Debug.Log(activeWpList[i].name);
        }
        activeWaypoints = activeWpList.ToArray();
    }

    private void Update()
    {
        //Source from: https://answers.unity.com/questions/1023987/lookat-only-on-z-axis.html
        Vector3 dirPos =  activeWaypoints[0].transform.position - dirArrow.transform.position;
        float rotationZ = Mathf.Atan2(dirPos.y, dirPos.x) * Mathf.Rad2Deg;
        dirArrow.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);

    }
    private void OnMouseExit()
    {
        rend.color = Color.white;
    }
}
