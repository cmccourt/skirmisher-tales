using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : EnemyUnit
{
    protected override void DealDamage(GameObject pTarget)
    {
        if (pTarget.tag == "Player")
        {
            pTarget.GetComponent<Unit>().TakeDamage(damage);
        }
        else
        {
            pTarget.GetComponent<BaseManager>().TakeBaseDamage(damage);
        }
        base.DealDamage(pTarget);
    }
}
