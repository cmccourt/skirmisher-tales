using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseManager : MonoBehaviour
{
    public Image healthBar;

    public float startingHealth;
    [Header("Damaged Sprites")]
    public Sprite halfDamaged;
    public Sprite completeDestroyed;
    [SerializeField]
    private float health;

    protected LevelManager levelMan;
    protected SpriteRenderer renderer;
    private void Awake()
    {
        health = startingHealth;
        levelMan = FindObjectOfType<LevelManager>();
        renderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        healthBar.fillAmount = health/startingHealth;
    }

    public void TakeBaseDamage(float amount)
    {
        health -= amount;
        healthBar.fillAmount = health / startingHealth;
        if (health <= 0)
        {
            EndGame();
            Destroy(gameObject);
        }
        else if (health <= startingHealth / 4)
        {
            if (completeDestroyed != null)
                renderer.sprite = completeDestroyed;
        }
        else if(health <= startingHealth / 2)
        {
            if (halfDamaged != null)
                renderer.sprite = halfDamaged;
        }
    }

    protected virtual void EndGame() { }
}
