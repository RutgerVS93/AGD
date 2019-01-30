using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour {

    [SerializeField]
    public int maxHealth;

    [SerializeField]
    public int currentHealth;

    public bool dead;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            dead = true;
        }
    }

    IEnumerator FlashOnHit(GameObject obj)
    {        
        SpriteRenderer rend = obj.GetComponent<SpriteRenderer>();
        Color startColor = rend.color;

        rend.color = Color.white;
        yield return new WaitForSeconds(.05f);
        rend.color = startColor;
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (gameObject.tag == "BossPart")
    //    {
    //        if (collision.gameObject.CompareTag("Bullet"))
    //        {
    //            TakeDamage(1);
    //            Destroy(collision.gameObject);
    //        }
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.tag == "BossPart")
        {
            if (collision.gameObject.CompareTag("Bullet"))
            {
                TakeDamage(1);
                StartCoroutine(FlashOnHit(gameObject));
                Destroy(collision.gameObject);
            }
        }
        if (gameObject.tag == "BossVitalPart")
        {
            if (collision.gameObject.CompareTag("Bullet"))
            {
                TakeDamage(1);
                StartCoroutine(FlashOnHit(gameObject));
                Destroy(collision.gameObject);
            }
        }

        if (gameObject.tag == "Destructible")
        {
            if (currentHealth == 0)
            {
                Destroy(gameObject);
            }
            if (collision.gameObject.CompareTag("Killbox"))
            {
                Destroy(gameObject);
            }
        }

        if (gameObject.tag == "Enemy")
        {
            if (currentHealth == 0)
            {
                Destroy(gameObject);
            }
            if (collision.gameObject.CompareTag("Killbox"))
            {
                Destroy(gameObject);
            }
        }
    }
}
