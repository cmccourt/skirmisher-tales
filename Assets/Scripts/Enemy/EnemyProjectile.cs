using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 15f;
    public float damage;
    private Rigidbody2D projRgb;

    private void Start()
    {
        projRgb = GetComponent<Rigidbody2D>();
        projRgb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.layer == 13)
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<UnitTasks>().TakeDamage(damage);
            }
            else if (collision.gameObject.tag == "Player Base")
            {
                collision.gameObject.GetComponent<BaseManager>().TakeBaseDamage(damage);
            }
            Destroy(gameObject);
        }


    }

}
