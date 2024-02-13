using DigitalRuby.LightningBolt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : PowerUp
{
    public float damage;
    public GameObject lightningEffectPrefab;

    private LightningBoltScript lightningScript;
    [SerializeField]
    private GameObject lightningInScene;
 

    protected override void Start()
    {
        base.Start();
        lightningInScene = GameObject.FindGameObjectWithTag("lightning");
        if (lightningInScene == null)
        {
            lightningInScene = Instantiate(lightningEffectPrefab);
        }
        lightningInScene.transform.position = new Vector3(lightningInScene.transform.position.x, lightningInScene.transform.position.y, 1);
    }
    protected override IEnumerator GetUnits()
    {
        StartCoroutine(SelectUnits());
        Debug.Log("Select Units");
        yield return new WaitUntil(() => !selectUnitMode);
        PerformPowerup();
    }
    
    protected override void PerformPowerup()
    {
        selectUnitMode = false;
        Debug.Log("Enemies on map: " + unitColliders.Count);
        if (unitColliders.Count > 0)
        {
            GameObject firstUnit = unitColliders[0].gameObject;
            lightningInScene.SetActive(true);
            Debug.Log(firstUnit.transform.position.z);
            lightningInScene.transform.position = new Vector3(firstUnit.transform.position.x, firstUnit.transform.position.y + 9, firstUnit.transform.position.z);
            lightningScript = lightningInScene.GetComponentInChildren<LightningBoltScript>();
            lightningScript.EndObject = firstUnit;
            StartCoroutine(BoltTimer());
            lightningScript.Trigger();
            resourceManager.orbAmount -= cost;
            for (int i = 0; i < unitColliders.Count; i++)
            {
                Debug.Log("Hit Enemy");
                lightningScript.Trigger();
                StartCoroutine(BoltTimer());
                GameObject unitGO = unitColliders[i].gameObject;
                unitGO.GetComponent<UnitTasks>().TakeDamage(damage);
            }
            StartCoroutine(ResetLightning());
            
        }
    }

    IEnumerator BoltTimer()
    {
        float duration = lightningScript.Duration;
        yield return new WaitForSeconds(duration);
    }

    IEnumerator ResetLightning()
    {
        yield return new WaitForSeconds(1);
        lightningInScene.transform.position = new Vector3(lightningInScene.transform.position.x, lightningInScene.transform.position.y, 1);
    }
}
