using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frozen : PowerUp
{
    public float duration;
    public Color frozenColour;


    protected override void Start()
    {
        base.Start();
    }
    protected override IEnumerator GetUnits()
    {
        StartCoroutine(SelectUnits());
        Debug.Log("Select Units");
        yield return new WaitUntil(() => !selectUnitMode);
        PerformPowerup();
    }

    protected override void PerformPowerup()
    {
        selectUnitMode = false;
        Debug.Log("Enemies on map: " + unitColliders.Count);
        if (unitColliders.Count > 0)
        {
            GameObject firstUnit = unitColliders[0].gameObject;
            resourceManager.orbAmount -= cost;
            for (int i = 0; i < unitColliders.Count; i++)
            {
                Debug.Log("Hit Enemy");
                GameObject unitGO = unitColliders[i].gameObject;
                UnitTasks unitScript = unitGO.GetComponent<UnitTasks>();
                unitScript.Freeze(frozenColour);
                StartCoroutine(FrozenTimer(unitScript));
            }
            
        }
    }

    IEnumerator FrozenTimer(UnitTasks script)
    {
        yield return new WaitForSeconds(duration);
        script.UnFreeze();
    }
}
