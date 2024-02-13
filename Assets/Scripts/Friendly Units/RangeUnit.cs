using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeUnit : UnitTasks
{
    public GameObject projGO;
    public Transform projSpawn;
    private Rigidbody2D projRgb;

    public void FireProjectile()
    {
        Instantiate(projGO, projSpawn.position, projSpawn.rotation);
        
    }
}
