using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldMineManager : MonoBehaviour
{
    public int goldRate;
    private resourceManager resourceMan;

    private void Awake()
    {
        resourceMan = FindObjectOfType<resourceManager>();
        resourceMan = resourceMan.GetComponent<resourceManager>();
        StartCoroutine("GenerateGold");
    }

    private void IncreaseGold()
    {
        resourceManager.goldAmount += goldRate;
        
        
        StartCoroutine("GenerateGold");
    }
   
    public void RemoveStructure()
    {
        resourceMan.RemoveStructure(gameObject);
    }   
    IEnumerator GenerateGold()
    {
        yield return new WaitForSeconds(1);
        IncreaseGold();
    }
        
    
}
