using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeTasks : UnitTasks
{
    protected override bool Attack()
    {
        if(enemy.tag == "Tower")
        {
            enemy.GetComponent<Tower>().TakeTowerDamage(damage);
        }
        else if (!enemy.tag.Contains("Base"))
        {
            enemy.GetComponent<UnitTasks>().TakeDamage(damage);
        }
        else
        {
            Debug.Log("Unit has hit " + enemy.name);
            enemy.GetComponent<BaseManager>().TakeBaseDamage(damage);
        }
        base.Attack();
        return true;
    }
}
