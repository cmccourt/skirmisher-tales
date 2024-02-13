using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public GameObject rayOrigin;
    public Image healthBar;
    public Sprite buttonIcon;
    public float startHealth = 100;
    public float attackRange = 20;
    public float damage = 10;
    public float attackDelay = 10f;
    public float m_speed;
    public float health;
    public Color mouseHoverColour;
    public AnimationClip deathAnimClip;
    protected Animator unitAnimator;
    private Color defaultColour;
    private Rigidbody2D gORgb2D;
    private bool attackMode;
    private GameObject enemy;
    private LayerMask enemyLayer;
    // Use this for initialization
    protected void Start()
    {
        unitAnimator = GetComponent<Animator>();
        unitAnimator.SetBool("isDead", false);
        defaultColour = gameObject.GetComponent<Renderer>().material.color;
        attackMode = false;
        gORgb2D = GetComponent<Rigidbody2D>();
        health = startHealth;
        Debug.Log("Unit health is: " + health);
        enemyLayer = LayerMask.GetMask("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        if(attackMode == false)
        {
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin.transform.position, transform.right, attackRange, enemyLayer);
            if (hit.collider != null)
            {
                unitAnimator.SetBool("attackMode", true);
                enemy = hit.collider.gameObject;
                attackMode = true;
                AttackEnemy(enemy);
                //Debug.Log(gameObject.name + " has hit: " + hit.collider.gameObject.tag);
               
            }
        }
        
        if (attackMode == false)
        {
            unitAnimator.SetBool("attackMode", false);
            gORgb2D.velocity = transform.right * m_speed;
        }
        else
        {
            gORgb2D.velocity = Vector3.zero;
        }
        unitAnimator.SetFloat("speed", gORgb2D.velocity.SqrMagnitude());
        unitAnimator.SetFloat("health", health);
    }

    public float GetHealth()
    {
        return health;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log("Player unit been hurt! Health is: " + health);
        healthBar.fillAmount = health / startHealth;
        if (health <= 0)
        {
            unitAnimator.SetBool("attackMode", false);
            unitAnimator.SetBool("isDead", true);
            Destroy(gameObject, deathAnimClip.length);
        }
    }

    protected virtual void AttackEnemy(GameObject pEnemy)
    {
        StartCoroutine("AttackCooldown");
    }

    protected virtual void OnMouseOver()
    {
        if (PowerUp.selectUnitMode)
        {
            Renderer unitRenderer = GetComponent<Renderer>();
            unitRenderer.material.color = mouseHoverColour;
        }       
    }

    protected virtual void OnMouseExit()
    {
        Renderer unitRenderer = GetComponent<Renderer>();
        unitRenderer.material.color = defaultColour;      
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Collision occured");
            Physics2D.IgnoreCollision(collision.collider, gORgb2D.GetComponent<Collider2D>());
        }
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackDelay);
        if (enemy != null)
        {
            AttackEnemy(enemy);
        }
        else
        {
            enemy = null;
            attackMode = false;
        }
    }
}
