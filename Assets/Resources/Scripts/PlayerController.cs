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
    public float fallMultiplier;
    public float lowJumpMultiplier;

    public bool canFire = true;

    private Rigidbody2D rb;
    public float knockbackVertical, knockbackHorizontal;

    [Header ("Bash")]
    public float reachRadius;
    RaycastHit2D[] objectsNear;
    public float launchSpeed;
    GameObject launchPos;
    public GameObject arrow;
    bool canLaunch;
    Vector3 direction;

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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, reachRadius);
    }

    private void FixedUpdate()
    {
        Movement();
        Launch();
    }

    void Movement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        rb.velocity = new Vector2(h * maxSpeed, rb.velocity.y);

        //ALTERNATE MOVEMENT
        //if (h > 0)
        //{
        //    rb.AddForce(Vector2.right * speed * Time.deltaTime);
        //}
        //else if (h < 0)
        //{
        //    rb.AddForce(Vector2.left * speed * Time.deltaTime);
        //}
        //if (rb.velocity.x > maxSpeed)
        //{
        //    rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
        //}
        //else if (rb.velocity.x < -maxSpeed)
        //{
        //    rb.velocity = new Vector2(-maxSpeed, rb.velocity.y);
        //}
        //if (h == 0)
        //{
        //    rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, Time.deltaTime);
        //}

        if (Input.GetKeyDown(KeyCode.W) && Grounded() || Input.GetButtonDown("A_Button") && Grounded() && v >= 0)
        {
            rb.AddForce(Vector2.up * jumpForce);
        }

        //Smoother Jump
        if (rb.velocity.y <= 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y >= 0  && !Input.GetButton("A_Button"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        if (rb.velocity.y <= -10)
        {
            rb.velocity = new Vector2(rb.velocity.x, -10);
        }
        else if (rb.velocity.y >= 15)
        {
            rb.velocity = new Vector2(rb.velocity.x, 15);
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

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("X_Button"))
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (collision.gameObject.CompareTag("Ground") && rb.velocity.y < 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyProjectile"))
        {
            healthScript.TakeDamage(1);
            StartCoroutine(FlashOnHit(gameObject));

            if (transform.position.x < collision.gameObject.transform.position.x)
            {
                rb.AddForce(new Vector2(knockbackHorizontal, knockbackVertical));
            }
            else if (transform.position.x > collision.gameObject.transform.position.x)
            {
                rb.AddForce(new Vector2(knockbackHorizontal, knockbackVertical));
            }

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

    void Launch()
    {
        float y = Input.GetAxis("Vertical");
        float x = Input.GetAxis("Horizontal");

        if (Input.GetButton("Y_Button"))
        {
            objectsNear = Physics2D.CircleCastAll(transform.position, reachRadius, Vector3.forward);
            foreach (RaycastHit2D obj in objectsNear)
            {
                if (obj.collider.gameObject.GetComponent<ScreenShake>() != null)
                {
                    launchPos = obj.collider.gameObject;
                    canLaunch = true;

                    arrow.SetActive(true);
                    arrow.transform.position = launchPos.transform.position;
                    float rot_z = Mathf.Atan2(-x, y) * Mathf.Rad2Deg;
                    arrow.transform.rotation = Quaternion.Euler(0f, 0f, rot_z);

                    direction = new Vector3(x, y, 0);
                    //direction = new Vector3(x, y, 0) - launchPos.transform.position;
                    direction.Normalize();
                    Debug.Log(direction);

                    Time.timeScale = 0.2f;
                    Time.fixedDeltaTime = 0.02f * Time.timeScale;
                }
            }
        }
        else if (Input.GetButtonUp("Y_Button") && canLaunch)
        {
            //rb.velocity = Vector2.zero;
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f;

            canLaunch = false;
            arrow.SetActive(false);

            transform.position = launchPos.transform.position;
            //rb.velocity = new Vector2(launchSpeed * x, launchSpeed * y);
            //rb.AddForce(new Vector2(x * launchSpeed * 2, y * launchSpeed));

            rb.velocity = direction * launchSpeed;
        }
        else if (Input.GetButtonUp("Y_Button") && !canLaunch)
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f;
        }
    }

    IEnumerator FlashOnHit(GameObject obj)
    {
        SpriteRenderer rend = obj.GetComponent<SpriteRenderer>();
        Color startColor = rend.color;

        rend.color = Color.red;
        yield return new WaitForSeconds(.05f);
        rend.color = startColor;
    }
}
