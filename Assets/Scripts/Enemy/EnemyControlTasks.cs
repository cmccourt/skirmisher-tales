using System.Collections.Generic;
using UnityEngine;
using Panda;
using System.Collections;
using System;

public enum AttackState
{
    critical,
    cautious,
    peaceful
    
}
public class EnemyControlTasks : MonoBehaviour
{
    public GameObject unitSpawn;
    private bool isIdle = true;
    [Header("Resource Variables")]
    public int goldAmount;
    public int orbAmount;
    public int resIncreaseDelay;
    public int goldIncrease;
    public int orbIncrease;

    [Header("Strucutres")]
    public GameObject goldStructure;
    public GameObject orbStructure;
    public GameObject[] plots; 

    [Header("Units to Deploy")]
    public GameObject[] units;
    public GameObject specialUnit;
    private List<GameObject> ownUnits = new List<GameObject>();

    [Header("Animation Curves")]
    public AnimationCurve weakCurve;
    public AnimationCurve equalCurve;
    public AnimationCurve strongCurve;
    public AttackState attackState;
    private float weakCurveVal;
    private float equalCurveVal;
    private float strongCurveVal;

    private float opponentAttackPower = 0f;
    private float ownAttackPower = 0f;
    private float opponentHealth = 0f;
    private float ownHealth = 0f;

    private float ownEffAttack;
    private float opponentEffAttack;
    private float equality = 50;
    private float attackEvalVal;

    [Header("Enemy Information")]
    public LayerMask opponentLayerMask;
    public string enemyTag;
    private List<GameObject> opponentUnits = new List<GameObject>();


    private void Start()
    {
        
        if(goldIncrease >= 1)
        {
            IncreaseGold();
        }
        if(orbIncrease >= 1)
        {
            IncreaseOrb();
        }
    }
    
    [Task]
    public bool CheckPlots()
    {
        if(plots == null)
        {
            return false;
        }
        for(int i=0; i< plots.Length; i++)
        {
            if (plots[i].tag != "Structure Slot")
            {
                return true;
            }
        }
        return false;
    }
    [Task]
    void BuildStructure(string resource)
    {
        Transform plot = null;
        for (int i = 0; i < plots.Length; i++)
        {
            if (plots[i].tag != "Structure Slot")
            {
                plot = plots[i].transform;
                break;
            }
        }
        if (plot != null)
        {
            if (resource == "Gold")
            {
                Instantiate(goldStructure, plot.position, plot.rotation);
            }
            else
            {
                Instantiate(orbStructure, plot.position, plot.rotation);
            }
            Task.current.Succeed();
        }
        else
        {
            Task.current.Fail();
        }
    }

    [Task]
    bool IsIdle()
    {
        return isIdle;
    }
    [Task]
    void SetIdle(bool condition)
    {
        isIdle = condition;
        Task.current.Succeed();
    }
    [Task]
    bool AnyEnemyUnits()
    {
        return FindUnits(enemyTag) != null;
    }
    [Task]
    public void CompareUnits()
    {
        opponentUnits = FindUnits(enemyTag);
        GetTotalUnitStats(out opponentAttackPower, out opponentHealth, opponentUnits);
        ownUnits = FindUnits(gameObject.tag);
        GetTotalUnitStats(out ownAttackPower, out ownHealth, ownUnits);
        //Get effective attack power
        opponentEffAttack = GetEffectiveAttack(opponentAttackPower, ownHealth);
        
        ownEffAttack = GetEffectiveAttack(ownAttackPower, opponentHealth);
        
        
        attackEvalVal = (ownEffAttack / opponentEffAttack) * 50;
        
        weakCurveVal = weakCurve.Evaluate(attackEvalVal);
        equalCurveVal = equalCurve.Evaluate(attackEvalVal);
        strongCurveVal = strongCurve.Evaluate(attackEvalVal);
        Task.current.Succeed();
    }

    [Task]
    public void ChangeAttackState()
    {
        float stateValue = Mathf.Max(weakCurveVal, equalCurveVal, strongCurveVal);
        if (stateValue == weakCurveVal)
        {
            attackState = AttackState.critical;
        }
        else if (stateValue == equalCurveVal)
        {
            attackState = AttackState.cautious;
        }
        else
        {
            attackState = AttackState.peaceful;
        }
        Task.current.Succeed();
    }
    
    [Task]
    public bool IsState(string stateName)
    {
        AttackState stateToCheck = (AttackState)System.Enum.Parse(typeof(AttackState), stateName);

        return stateToCheck == attackState;
    }

    [Task]
    public void DeployUnit()
    {
        
        int selectedIndex = UnityEngine.Random.Range(0, units.Length);
        if (goldAmount >= units[selectedIndex].GetComponent<UnitTasks>().cost)
        {
            Instantiate(units[selectedIndex], unitSpawn.transform.position, unitSpawn.transform.rotation);
            goldAmount -= units[selectedIndex].GetComponent<UnitTasks>().cost;
            Task.current.Succeed();
        }
        else
        {
            Task.current.Fail();
        }
    }

    [Task]
    public void DeploySpecialUnit()
    {
        if(goldAmount >= specialUnit.GetComponent<UnitTasks>().cost)
        {
            GameObject spawnedUnit = Instantiate(specialUnit, unitSpawn.transform.position, unitSpawn.transform.rotation);
            goldAmount -= spawnedUnit.GetComponent<UnitTasks>().cost;
            Task.current.Succeed();
        }
        Task.current.Fail(); 
    }

    [Task]
    bool IsLevelCompleted()
    {
        return LevelManager.isLevelCompleted;
    }


    public List<GameObject> FindUnits(string searchTag)
    {
        List<GameObject> units = new List<GameObject>();
        GameObject[] opUnitsFound = GameObject.FindGameObjectsWithTag(searchTag);

        if (opUnitsFound.Length > 0)
        {
            for (int i = 0; i < opUnitsFound.Length; i++)
            {
                
                if (opUnitsFound[i].name != gameObject.name)
                {
                    units.Add(opUnitsFound[i]);
                }
            }
            return units;
        }
        return null;
    }

    public void GetTotalUnitStats(out float attackPower, out float health, List<GameObject> units)
    {
        if (units == null || units.Count == 0)
        {
            attackPower = 0.01f;
            health = 0.01f;
        }
        else
        {
            
            attackPower = 0f;
            health = 0f;
            for (int i = 0; i < units.Count; i++)
            {
                
                try
                {
                    MeleeTasks opScript = units[i].GetComponent<MeleeTasks>();
                    attackPower += (opScript.damage * opScript.attackDelay);
                    health += opScript.health;
                }
                catch (NullReferenceException ex)
                {
                    RangeUnit opScript = units[i].GetComponent<RangeUnit>();
                    attackPower += (opScript.damage * opScript.attackDelay);
                    health += opScript.health;
                }
            }
        }
        
    }

    private float GetEffectiveAttack(float attackPower, float health)
    {
        if(attackPower == 0)
        {
            attackPower = 0.01f;
            
        }
        if (health == 0)
        {
            health = 0.01f;
        }
        
        return (attackPower / health) * equality;
        
    }

    void IncreaseGold()
    {
        goldAmount += goldIncrease;
        StartCoroutine(GatherResourceDelay(IncreaseGold));
    }
    void IncreaseOrb()
    {
        orbAmount += orbIncrease;
        StartCoroutine(GatherResourceDelay(IncreaseOrb));
    }
    

    IEnumerator GatherResourceDelay(Action function)
    {
        yield return new WaitForSeconds(resIncreaseDelay);
        function();
    }
}

