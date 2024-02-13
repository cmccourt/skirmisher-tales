using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthBoost : PowerUp
{
    public int healthIncrease;
    public int powerUpDuration;

    protected override IEnumerator GetUnits()
    {
        StartCoroutine(SelectUnits());
        Debug.Log("Select Units");
        yield return new WaitUntil(() => !selectUnitMode);
        
        if (unitColliders.Count > 1)
        {
            uIManager.UpdateSelectUnitPanel(unitColliders.ToArray(),this);
            yield return new WaitUntil(() => unitColliders.Count == 1);
        }
        PerformPowerup();
    }


    
    protected override void PerformPowerup()
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
        if(unitColliders.Count != 0 && resourceManager.orbAmount >= cost) {
            resourceManager.orbAmount -= cost;
            UnitTasks unitScript = unitColliders[0].gameObject.GetComponent<UnitTasks>();
            Debug.Log("Health before: " + unitScript.health);
            StartCoroutine(increaseHealth(1, powerUpDuration, unitScript));
        }
        
    }

    IEnumerator increaseHealth(int interval, int count, UnitTasks unitScript)
    {
        for(int i = 0; i <= count; i++)
        {
            Debug.Log(unitScript.health);
            unitScript.HealPlayer(healthIncrease);
            yield return new WaitForSeconds(interval);
        }
    }
}
