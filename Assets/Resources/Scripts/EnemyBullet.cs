using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {

    public float bulletSpeed;
    Rigidbody2D rb;

	void Start ()
    {
        //rb = GetComponent<Rigidbody2D>();
        //rb.AddForce(Vector2.right * bulletSpeed);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }

        if (!collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.GetComponent<HealthScript>() != null)
            {
                collision.gameObject.GetComponent<HealthScript>().TakeDamage(1);
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
