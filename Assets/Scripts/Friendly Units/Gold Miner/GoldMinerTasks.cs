using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;


public class GoldMinerTasks : UnitTasks
{
    public enum resource { Gold, Orb };
    public resource resourceType;
    public GameObject spawn;
    public int resourceQuantity;
    [SerializeField]
    private bool hasResource;

    private List<object> structures = new List<object>();

    [Task]
    bool SetDestination(string location)
    {
        base.target = null;
        if(location == "base")
        {
            base.target = spawn.transform;
           
        }
        else if(location == "resource")
        {
            if (FindResourceStructure())
            {
                int randomIndex = Random.Range(0, structures.Count);
                GameObject selectedStructure = (GameObject)structures[randomIndex];
                base.target = selectedStructure.transform;
                
            }
        }
        return base.target != null;
    }
    [Task]
    public bool FindResourceStructure()
    {
        if (resourceManager.structureDict.Count > 0)
        {
            structures.Clear();
            foreach (KeyValuePair<object, object> structure in resourceManager.structureDict)
            {
                //Debug.Log(resourceType.ToString());
                //Debug.Log(structure.Key.ToString());
                if (structure.Value.ToString().Contains(resourceType.ToString()))
                {
                    structures.Add(structure.Key);
                }
            }
            return structures.Count > 0;
        }
        return false;
    }
    [Task]
    bool HasResource()
    {
        return hasResource;
    }
    [Task]
    bool CollectResource()
    {
        return hasResource = true;
    }

    [Task]
    void DepositResource()
    {
        if(resourceType == resource.Gold)
        {
            resourceManager.goldAmount += resourceQuantity;
        }
        else
        {
            resourceManager.orbAmount += resourceQuantity;
        }
        hasResource = false;
        Task.current.Succeed();
    }
    
    [Task]
    protected override bool Attack()
    {
        base.enemy.GetComponent<UnitTasks>().TakeDamage(damage);
        base.Attack();
        return true;
    }
}
