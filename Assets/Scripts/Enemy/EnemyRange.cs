using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRange : EnemyUnit
{ 
    public GameObject projGO;
    public Transform projSpawn;
    private Rigidbody2D projRgb;

    protected override void DealDamage(GameObject pTarget)
    {
        Instantiate(projGO, projSpawn.position, projSpawn.rotation);
        StartCoroutine("AttackCooldown");
    }
}
