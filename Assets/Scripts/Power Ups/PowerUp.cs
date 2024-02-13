using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class PowerUp : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI")]
    public int cost;
    public GameObject pwrInfoDisplay;
    public Text costText;
    protected UIManager uIManager;
    
    [Header("Cursor Info")]
    //Source Cursor.SetCursor Unity documentation
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    [Header("Button")]
    protected bool onCooldown;
    protected Button pwrButton;

    [Header("Audio")]
    public AudioClip soundEffect;
    protected AudioSource goAudioSrc;

    [Header("Attack Options")]
    public static bool selectUnitMode = false;
    public LayerMask targetLayer;
    protected List<Collider2D> unitColliders = new List<Collider2D>();
    
    

    protected virtual void Start()
    {
        pwrButton = GetComponent<Button>();
        goAudioSrc = GetComponentInChildren<AudioSource>();
        uIManager = FindObjectOfType<UIManager>();
        costText.text = "Cost: " + cost;
        pwrInfoDisplay.SetActive(false);
    }
    private void Update()
    {
        if(resourceManager.orbAmount >= cost)
        {
            if(pwrButton != null)
                pwrButton.interactable = true;
        }
        else
        {
            if (pwrButton != null)
                pwrButton.interactable = false;
        }
    }
    public void CastPowerup()
    {
        if (selectUnitMode)
        {
            selectUnitMode = false;
            Cursor.SetCursor(null, Vector2.zero, cursorMode);
        }
        else
        {
            selectUnitMode = true;
            Debug.Log("ITS LIGHTNING TIME");
            Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
            StartCoroutine("GetUnits");
        }
    }

    protected void GetUnitsFromCursor()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 5f;
        Vector2 v = Camera.main.ScreenToWorldPoint(mousePosition);
        unitColliders.Clear();
        unitColliders.AddRange(Physics2D.OverlapPointAll(v, targetLayer));

        Debug.Log(unitColliders.Count);
    }
    
    protected IEnumerator SelectUnits()
    {
        while (true)
        {
            if (Input.GetButtonDown("Fire1") && selectUnitMode)
            {
                Cursor.SetCursor(null, Vector2.zero, cursorMode);
                GetUnitsFromCursor();
                selectUnitMode = false;
                Debug.Log("Enemy been struck");
                yield break;
            }
            else if (Input.GetButtonDown("Fire2") && selectUnitMode)
            {
                Cursor.SetCursor(null, Vector2.zero, cursorMode);
                selectUnitMode = false;
                yield break;
            }
            yield return null;
        }
    }
    
    public void SelectSingleUnit(Collider2D unitColl)
    {
        unitColliders.Clear();
        unitColliders.Add(unitColl);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pwrInfoDisplay.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pwrInfoDisplay.SetActive(false);
    }
    protected abstract IEnumerator GetUnits();
    protected abstract void PerformPowerup();
}
