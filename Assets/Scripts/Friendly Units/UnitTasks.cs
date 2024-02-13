using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Panda;
using UnityEditor;

public class UnitTasks : MonoBehaviour
{
    [Header("UI")]
    public Image healthBar;
    public Sprite buttonIcon;
    public Color mouseHoverColour;
    protected Color defaultColour;

    [Header("Attack Stats")]
    public float attackRange = 20;
    public float damage = 10;
    public float attackDelay = 10f;
    private bool onCooldown = false;
    private float orgDamage;

    [Header("Health")]
    public float startHealth = 100;
    [HideInInspector]
    public float health;


    [Header("Additional Info")]
    public bool isStationary;
    public int killScore;
    public int cost;
    public LayerMask enemyLayer;
    public bool belongToEnemy;
    public GameObject rayOrigin;
    public AnimationClip deathAnimClip;

    [Header("Navigation")]
    public float m_speed;
    public bool isFrozen;
    [SerializeField]
    protected Transform target;
    protected Transform[] pathWaypoints;
    protected int targetIndex;
    protected int pathIndex;


    protected Animator unitAnimator;
    protected Rigidbody2D gORgb2D;
    [SerializeField]
    protected Transform enemyBase;
    protected GameObject enemy;
    protected RouteManager routeMan;
    
    // Use this for initialization
    protected void Start()
    {
        unitAnimator = GetComponent<Animator>();
        unitAnimator.SetBool("isDead", false);
        defaultColour = gameObject.GetComponent<Renderer>().material.color;
        gORgb2D = GetComponent<Rigidbody2D>();
        routeMan = FindObjectOfType<RouteManager>();
        health = startHealth;
        orgDamage = damage;
        Debug.Log("Unit health is: " + health);
        isFrozen = false;
        //Rotate the unit to face the direction of the player
        if (belongToEnemy)
        {
            transform.Rotate(0f, 180f, 0f);
        }
        BaseManager[] bases = FindObjectsOfType<BaseManager>();
        for (int i=0; i < bases.Length; i++)
        {
            if (enemyLayer == (enemyLayer.value| 1 << bases[i].gameObject.layer))
            {
                enemyBase = bases[i].gameObject.transform;
                break;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        unitAnimator.SetFloat("health", health);
        healthBar.fillAmount = health / startHealth;
    }

    
    public void HealPlayer(float amount)
    {
        health += amount;
        if(health > startHealth)
        {
            health = startHealth;
        }
    }
    public void IncreaseAttackStats(float amount)
    {
        damage += amount;
    }
    public void ResetAttackStats()
    {
        damage = orgDamage;
    }
    public void TakeDamage(float amount)
    {
        health -= amount;
        //Debug.Log("Unit been hurt! Health is: " + health);
        healthBar.fillAmount = health / startHealth;
    }

    [Task]
    protected bool StopMoving()
    {
        gORgb2D.velocity = Vector3.zero;
        unitAnimator.SetBool("isWalking", false);
        return true;
    }

    [Task]
    protected bool IsFrozen()
    {
        return isFrozen;
    }

    [Task]
    protected bool IsStationary()
    {
        return isStationary;
    }

    [Task]
    protected bool EnemyInRange() {
        enemy = null;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin.transform.position, rayOrigin.transform.right, attackRange, enemyLayer);
       
        if (hit.collider != null)
        {
            enemy = hit.collider.gameObject;
        }
        else
        {
            unitAnimator.SetBool("attackMode", false);
        }
        return enemy != null;
    }

    [Task]
    protected bool UnitOnCooldown()
    {
        return onCooldown;
    }

    [Task]
    protected bool IsHealthLessThan(float value)
    {
        return health < value;
    }

    [Task]
    protected bool Die()
    {
        unitAnimator.SetBool("attackMode", false);
        unitAnimator.SetBool("isDead", true);
        UIManager uiManager = FindObjectOfType<UIManager>();
        uiManager.IncreaseScore(killScore);
        Destroy(gameObject, deathAnimClip.length);
        return true;
    }

    [Task]
    protected virtual bool Attack()
    {
        
        unitAnimator.SetBool("attackMode", true);
        unitAnimator.SetTrigger("attack");
        onCooldown = true;
        StartCoroutine(AttackCooldown());
        return true;
    }
        
    [Task]
    protected virtual bool SetDestination()
    {  
        if (target == null)
        {
            if (routeMan != null)
            {
                pathIndex = 0;
                targetIndex = 0;
                pathWaypoints = routeMan.activeWaypoints[pathIndex];
                routeMan.SetCollisions(gameObject.GetComponent<Collider2D>(), pathIndex);
                target = pathWaypoints[targetIndex];
            }
            else
            {
                if (enemyBase != null)
                    target = enemyBase;
                else
                    target = FindObjectOfType<LevelManager>().transform;
            }
        }
        if(routeMan != null)
        {
            Debug.Log(target.name);
            if (Vector2.Distance(gameObject.transform.position, target.position) < 1)
            {
                targetIndex++;
                if(targetIndex == pathWaypoints.Length)
                {
                    targetIndex = 0;
                    pathIndex++;
                   
                    if (pathIndex >= routeMan.activePaths.Count)
                    {
                        if (enemyBase != null)
                            target = enemyBase;
                        else
                            target = FindObjectOfType<LevelManager>().transform;
                        Debug.Log("level maanger target");
                        return true;
                    }
                    else
                    {
                        pathWaypoints = routeMan.activeWaypoints[pathIndex];
                        routeMan.SetCollisions(gameObject.GetComponent<Collider2D>(), pathIndex);
                        Debug.Log("no hang on this is the target");
                        target = pathWaypoints[targetIndex];
                    }
                }
            }
        }
        else
        {
            if (enemyBase != null)
                target = enemyBase;
            else
                target = FindObjectOfType<LevelManager>().transform;
        }
        return true;
    }
    
    [Task]
    protected bool MoveToDestination()
    {
        unitAnimator.SetBool("isWalking", true);
        transform.position = Vector3.MoveTowards(new Vector3(transform.position.x, transform.position.y, transform.position.z),
            (new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z)), m_speed * Time.deltaTime);
        return true;
    }

    [Task]
    protected bool ReachedDestination()    
    {
        if (Vector2.Distance(target.transform.position, transform.position) < 1)
        {
            unitAnimator.SetBool("isWalking", false);
        }
        
        //Debug.Log(Vector2.Distance(target.transform.position, transform.position));
        return Vector2.Distance(target.transform.position, transform.position) < 1;
        
    }

    [Task]
    protected bool IsLevelComplete()
    {
        return LevelManager.isLevelCompleted;
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


    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(collision.collider, gORgb2D.GetComponent<Collider2D>());
        }
    }

    protected IEnumerator AttackCooldown()
    {
        unitAnimator.SetTrigger("attack");
        yield return new WaitForSeconds(attackDelay);
        onCooldown = false;
        
    }
    
    public void Freeze(Color frozenColour)
    {
        isFrozen = true;
        Renderer spriteRenderer = GetComponent<Renderer>();
        spriteRenderer.material.color = frozenColour;
    }
    public void UnFreeze()
    {
        isFrozen = false;
        Renderer spriteRenderer = GetComponent<Renderer>();
        spriteRenderer.material.color = defaultColour;
    }
}
