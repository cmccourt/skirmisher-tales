using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 15f;
    public float damage;
    public LayerMask targetLayer;
    private Rigidbody2D projRgb;

    private void Start()
    {
        projRgb = GetComponent<Rigidbody2D>();
        projRgb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Projectile collide " + collision.name);
        
        if (targetLayer == (targetLayer | (1 << collision.gameObject.layer)))
        {
            if (collision.gameObject.tag == "Tower")
            {
                collision.gameObject.GetComponent<Tower>().TakeTowerDamage(damage);
            }
            else if (collision.gameObject.layer == 9)
            {
                if (collision.gameObject.tag == "Enemy")
                {
                    collision.gameObject.GetComponent<UnitTasks>().TakeDamage(damage);
                }
                else if (collision.gameObject.tag == "Enemy Base")
                {
                    collision.gameObject.GetComponent<BaseManager>().TakeBaseDamage(damage);
                }
            }
            else if (collision.gameObject.layer == 13)
            {
                if (collision.gameObject.tag == "Player")
                {
                    collision.gameObject.GetComponent<UnitTasks>().TakeDamage(damage);
                }
                else if (collision.gameObject.tag == "Player Base")
                {
                    collision.gameObject.GetComponent<BaseManager>().TakeBaseDamage(damage);
                }

            }
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject, 8);
        }
        
    }

}
