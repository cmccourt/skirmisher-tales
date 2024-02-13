using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    public GameObject projectile;
    public Transform spawn;
    public LayerMask targetLayer;
    public float cooldownLength;
    [Header("Health")]
    public Image healthBar;
    public float startingHealth;
    [Header("Sprites")]
    public Sprite halfDamaged;
    public Sprite completeDestroyed;
    protected SpriteRenderer towerRenderer;

    private float currentHealth;
    private List<GameObject> targetUnits = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
        healthBar.fillAmount = currentHealth / startingHealth;
        towerRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
        else if (currentHealth <= startingHealth / 4)
        {
            if (completeDestroyed != null)
                towerRenderer.sprite = completeDestroyed;
        }
        else if (currentHealth <= startingHealth / 2)
        {
            if (halfDamaged != null)
                towerRenderer.sprite = halfDamaged;
        }
    }

    public void TakeTowerDamage(float amount)
    {
        currentHealth -= amount;
        healthBar.fillAmount = currentHealth / startingHealth;
    }

    public void AttackUnit()
    {
        if(targetUnits.Count > 0)
        {
            GameObject unit = targetUnits[0];
            if (unit) {
                AimAtUnit(unit.transform);
                Instantiate(projectile, spawn.position, spawn.rotation);
                StartCoroutine(Cooldown());
            }
            else
            {
                targetUnits.Remove(unit);
            }

        }
    }

    public void AimAtUnit(Transform target)
    {
        //Source from: https://answers.unity.com/questions/1023987/lookat-only-on-z-axis.html
        Vector3 dirPos = target.transform.position - spawn.transform.position;
        float rotationZ = Mathf.Atan2(dirPos.y, dirPos.x) * Mathf.Rad2Deg;
        spawn.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        Debug.Log(collision.gameObject.layer);
        if(targetLayer == (targetLayer| (1<<collision.gameObject.layer)))
        {
            targetUnits.Add(collision.gameObject);
            Debug.Log(targetUnits.Count);
            if (targetUnits.Count == 1)
            {
                AttackUnit();
            }
        }
        
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldownLength);
        AttackUnit();
    }
}
