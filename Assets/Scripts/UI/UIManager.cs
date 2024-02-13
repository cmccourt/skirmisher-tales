using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public GameObject unitPanel;
    public GameObject structPanel;
    public GameObject selectedUnitPanel;
    public RectTransform selUnitRectPan;
    public GameObject prefabButton;

    public Text scoretext;
    public int totalScore;
    // Start is called before the first frame update
    void Start()
    {
        SelectUnitPanel();
        totalScore = 0;
    }

    private void Update()
    {
        scoretext.text = "Score: "+ totalScore;
    }

    public void IncreaseScore(int score)
    {
        totalScore += score;
    }

    public void SelectUnitPanel()
    {
        structPanel.SetActive(false);
        selectedUnitPanel.SetActive(false);
        unitPanel.SetActive(true);
        foreach(Button child in unitPanel.transform.GetComponentsInChildren<Button>())
        {
            child.interactable = true;
        }
    }
    public void SelectStructurePanel()
    {
        unitPanel.SetActive(false);
        selectedUnitPanel.SetActive(false);
        structPanel.SetActive(true);
    }
    public void SelectPickedUnitPanel()
    {
        unitPanel.SetActive(false);
        selectedUnitPanel.SetActive(true);
        structPanel.SetActive(false);
    }

    public void UpdateSelectUnitPanel(Collider2D[] unitColliders, PowerUp pwrupScript)
    {
        SelectPickedUnitPanel();
        
        for(int i = 0; i < unitColliders.Length; i++)
        {
            GameObject unitButton = Instantiate(prefabButton);
            unitButton.transform.SetParent(selUnitRectPan, false);
            unitButton.transform.localScale = new Vector3(1, 1, 1);

            SelectedUnitButton selUnitButtonScript = unitButton.GetComponent<SelectedUnitButton>();
            selUnitButtonScript.InitialiseButton(unitColliders[i].gameObject, pwrupScript);
        }
    }
}
