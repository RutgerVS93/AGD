using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    

    public HealthScript healthScript;

    public Gun[] guns;
    public int currentGun;

    public Transform firePoint;

    public float speed;
    public float maxSpeed;
    public Vector2 currentVelocity;
    bool facingRight = true;

    public float jumpForce;
    public float gravMultiplier;

    public bool canFire = true;

    private Rigidbody2D rb;

    void Start ()
    {
        healthScript = GetComponent<HealthScript>();

        currentGun = 0;

        rb = GetComponent<Rigidbody2D>();
        firePoint = GetComponent<Transform>();
	}
	
	void Update ()
    {
        Fire();
	}

    private void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        float h = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(h * maxSpeed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.W) && Grounded())
        {
            rb.AddForce(Vector2.up * jumpForce);
        }

        if (currentVelocity.y < 0 )
        {
            rb.gravityScale *= gravMultiplier;
        }
        else
        {
            rb.gravityScale = 1;
        }

        if (h > 0 && !facingRight)
        {
            Flip();
        }
        else if (h < 0 && facingRight)
        {
            Flip();
        }
    }

    //Ground Check
    bool Grounded()
    {
        if (Physics2D.Raycast(transform.position, Vector2.down, .5f))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale *= -1;
        transform.localScale = scale;
    }

    //Firing / Switching Weps
    void Fire()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentGun += 1;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            currentGun -= 1;
        }
        if (currentGun > 2)
        {
            currentGun = 0;
        }
        if (currentGun < 0)
        {
            currentGun = 2;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            canFire = false;
            StartCoroutine(FireGun());
        }  
    }

    IEnumerator FireGun()
    {
        GameObject bulletClone;
        GameObject impactClone;
        float rotation = 0;

        if (guns[currentGun].spread)
        {
            StartCoroutine(StaticSpreadShot());
        }

        else if (guns[currentGun].randomSpread)
        {
            for (int i = 0; i < guns[currentGun].bulletNumber; i++)
            {
                float randomValue;
                randomValue = Random.Range(guns[currentGun].randomSpreadValue.x, guns[currentGun].randomSpreadValue.y);
                rotation += randomValue;

                bulletClone = Instantiate(guns[currentGun].bulletParticle, firePoint.transform.position, Quaternion.Euler(0, 0, rotation + (i * randomValue)));

                bulletClone.AddComponent<Rigidbody2D>();
                bulletClone.GetComponent<Rigidbody2D>().gravityScale = 0;
                bulletClone.GetComponent<Rigidbody2D>().AddForce(Vector2.right * guns[currentGun].bulletSpeed);                

                Destroy(bulletClone, guns[currentGun].bulletLifeTime);
            }
        }

        else
        {
            bulletClone = Instantiate(guns[currentGun].bulletParticle, firePoint.transform.position, Quaternion.identity);

            bulletClone.AddComponent<Rigidbody2D>();
            bulletClone.GetComponent<Rigidbody2D>().gravityScale = 0;

            //Flip bulletDirection
            if (facingRight)
            {
                bulletClone.GetComponent<Rigidbody2D>().AddForce(Vector2.right * guns[currentGun].bulletSpeed);
            }
            else
            {
                bulletClone.GetComponent<Rigidbody2D>().AddForce(-Vector2.right * guns[currentGun].bulletSpeed);
            }

            Destroy(bulletClone, guns[currentGun].bulletLifeTime);
        }

        yield return new WaitForSeconds(guns[currentGun].fireRate);
        canFire = true;
    }

    IEnumerator StaticSpreadShot()
    {
        GameObject bulletClone;
        GameObject impactClone;
        for (int i = 0; i < guns[currentGun].bulletNumber; i++)
        {
            Quaternion rotation_ = firePoint.transform.rotation;
            //rotation = rotation + guns[currentGun].staticSpreadValue;
            rotation_.z += guns[currentGun].staticSpreadValue * i;
            //bulletClone = Instantiate(guns[currentGun].bulletParticle, firePoint.transform.position, firePoint.transform.rotation) as GameObject;

            bulletClone = Instantiate(guns[currentGun].bulletParticle, firePoint.transform.position, Quaternion.Euler(0, 0, rotation_.z));

            bulletClone.AddComponent<Rigidbody2D>();
            bulletClone.GetComponent<Rigidbody2D>().gravityScale = 0;
            bulletClone.GetComponent<Rigidbody2D>().AddForce(Vector2.right * guns[currentGun].bulletSpeed);

            Destroy(bulletClone, guns[currentGun].bulletLifeTime);
        }
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyProjectile"))
        {
            healthScript.TakeDamage(1);
            if (healthScript.currentHealth <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        if (collision.gameObject.CompareTag("Killbox"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
