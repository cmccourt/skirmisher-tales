using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    
    [Header("Gold Variables")]
    public int goldAmount;
    public int goldIncrease;
    public int targetGoldNum;

    [Header("Units to Deploy")]
    public GameObject unit;
    public GameObject unitSpawn;

    private void Start()
    {
        StartCoroutine(WaitStartOfLevel());
    }
    public void IncrementGold()
    {
        goldAmount += goldIncrease;
        StartCoroutine(GatherGold());
    }

    public void InstantiateUnit(GameObject unit, int cost)
    {
        Instantiate(unit, unitSpawn.transform.position, unitSpawn.transform.rotation);
        goldAmount -= cost;
    }

    public void AttackPlayer()
    {
        float randomNum = Random.value;
        if (goldAmount >= targetGoldNum && randomNum > 0.6)
        {
            InstantiateUnit(unit, targetGoldNum);
        }
        StartCoroutine(DeployUnit());
    }
    void Fight()
    {
        StartCoroutine(GatherGold());
        StartCoroutine(DeployUnit());
    }
    IEnumerator WaitStartOfLevel()
    {
        yield return new WaitUntil(() => LevelManager.levelStarted);
        Fight();
    }
    
    IEnumerator GatherGold()
    {
        yield return new WaitForSeconds(1);
        IncrementGold();
    }
    IEnumerator DeployUnit()
    {
        yield return new WaitForSeconds(3);
        AttackPlayer();
    }

}
