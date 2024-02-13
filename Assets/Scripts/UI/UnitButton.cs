using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitButton : MonoBehaviour {

    //public GameObject resManObject;
    
    public GameObject unitPrefab;
    public GameObject spawnPoint;
    public int deployCooldown;
    public Text costText;

    private resourceManager resManager;
    private int cost;
    private Button unitButton;
    // Use this for initialization
    void Start () {
        resManager = FindObjectOfType<resourceManager>();
        cost = unitPrefab.GetComponent<UnitTasks>().cost;
        costText.text = cost.ToString() + " Gold";
        unitButton = GetComponent<Button>();
    }
	
    public void OnClick()
    {
        if (resourceManager.goldAmount >= cost && unitButton.interactable == true)
        {
            resManager.reduceGoldTotal(cost);
            Instantiate(unitPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
            StartCoroutine(ButtonReset());
        }
    }

    IEnumerator ButtonReset()
    {
        
        unitButton.interactable = false;
        yield return new WaitForSeconds(deployCooldown);
        unitButton.interactable = true;
    }
}
