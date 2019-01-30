using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
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
