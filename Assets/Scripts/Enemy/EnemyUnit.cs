using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUnit : MonoBehaviour {

    public GameObject rayOrigin;
    public float attackRange = 20;
    public float damage = 10f;
    public Image healthBar;
    public float startHealth = 100;
    public float attackDelay = 2f;
    public float m_speed;
    public bool isUnitStationary;

    private float health;
    private bool attackMode = false;
    private LayerMask targetLayer;
    private GameObject player;
    private Rigidbody2D gORgb2D;


    private void Start()
    {
        gORgb2D = GetComponent<Rigidbody2D>();
        health = startHealth;
        healthBar.fillAmount = health / startHealth;
        targetLayer = LayerMask.GetMask("Player");
        if (!isUnitStationary)
        {
            StartCoroutine("MoveUnitCoroutine");
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (!attackMode)
        {
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin.transform.position, -transform.right, attackRange, targetLayer);

            if (hit.collider != null)
            {
                player = hit.collider.gameObject;
                DealDamage(player);
                attackMode = true;

                //Debug.Log(gameObject.name + " has hit: " + hit.collider.gameObject.tag);
            }
        }
    }

    private void MoveUnit()
    {
        if (!attackMode)
        {
            gORgb2D.velocity = -transform.right * m_speed;
            StartCoroutine("MoveUnitCoroutine");
        }
        else
        {
            gORgb2D.velocity = Vector3.zero;
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        //Debug.Log("Enemy unit been hurt! Health is: " + health);
        healthBar.fillAmount = health / startHealth;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }


    protected virtual void DealDamage(GameObject hit)
    {
        
        StartCoroutine("AttackCooldown");
    
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            
            Physics2D.IgnoreCollision(collision.collider, gameObject.GetComponent<Collider2D>());
        }
    }


    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackDelay);
        if(player != null)
        {
            DealDamage(player);
        }
        else
        {
            player = null;
            attackMode = false;
        }
    }

    IEnumerator MoveUnitCoroutine()
    {
        yield return new WaitForFixedUpdate();
        MoveUnit();
    }
}
