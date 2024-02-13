using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StructureButton : MonoBehaviour
{
    public int cost;
    public GameObject structure;
    public Text costDisplay;
    private resourceManager resourceMan;
    // Start is called before the first frame update
    void Start()
    {
        resourceMan = FindObjectOfType<resourceManager>();
        costDisplay.text = cost.ToString() + " gold";
    }

    public void OnClick()
    {
        if (resourceManager.goldAmount >= cost)
        {
            resourceMan.PurchaseStructure(structure, cost);
        }
    }
}
