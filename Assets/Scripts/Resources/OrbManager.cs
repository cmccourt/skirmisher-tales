using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbManager : MonoBehaviour
{
    public int orbRate;
    private resourceManager resourceMan;

    private void Awake()
    {
        resourceMan = FindObjectOfType<resourceManager>();
        resourceMan = resourceMan.GetComponent<resourceManager>();
        StartCoroutine("GenerateOrbs");
    }

    
    private void IncreaseOrb()
    {
        resourceManager.orbAmount += orbRate;
        StartCoroutine("GenerateOrbs");
    }

    public void RemoveStructure()
    {
        resourceMan.RemoveStructure(gameObject);
    }

    IEnumerator GenerateOrbs()
    {
        yield return new WaitForSeconds(1);
        IncreaseOrb();
    }
}
