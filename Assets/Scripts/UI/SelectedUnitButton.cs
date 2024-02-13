using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedUnitButton : MonoBehaviour
{
    private UnitTasks unitScript;
    [SerializeField]
    private Image healthBar;
    private Button unitButton;
    public void InitialiseButton(GameObject unit, PowerUp powerUp)
    {
        unitButton = GetComponentInChildren<Button>();
        Debug.Log(unit.name);
        unitScript = unit.GetComponent<UnitTasks>();

        unitButton.image.sprite = unitScript.buttonIcon;
        Transform[] childrenObjects = GetComponentsInChildren<Transform>();
        foreach(Transform child in childrenObjects)
        {
            Debug.Log(child.gameObject.name);
            if(child.gameObject.tag == "HealthBar")
            {
                
                healthBar = child.GetComponent<Image>();
            }
        }
        Debug.Log(healthBar.fillAmount);
        DisplayHealthBar();
        
        unitButton.onClick.AddListener(()=>UnitSelected(unit,powerUp));

    }

    public void DisplayHealthBar()
    {
        healthBar.fillAmount = unitScript.health / unitScript.startHealth;
        if (unitScript.health <= 0)
        {
            Destroy(gameObject);
        }
        StartCoroutine(WaitForNextFrame());
    }
    private void UnitSelected(GameObject unitColl,PowerUp powerUp)
    {
        powerUp.SelectSingleUnit(unitColl.GetComponent<Collider2D>());
    }
    IEnumerator WaitForNextFrame()
    {
        yield return new WaitForEndOfFrame();
        DisplayHealthBar();
    }
}
