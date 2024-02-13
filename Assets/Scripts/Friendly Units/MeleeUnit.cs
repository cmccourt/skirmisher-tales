using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeUnit : Unit
{
    protected override void AttackEnemy(GameObject pEnemy)
    {
        if(pEnemy.tag == "Enemy")
        {
            pEnemy.GetComponent<EnemyUnit>().TakeDamage(damage);
        }
        else
        {
            pEnemy.GetComponent<BaseManager>().TakeBaseDamage(damage);
        }
        base.AttackEnemy(pEnemy);

    }
}
