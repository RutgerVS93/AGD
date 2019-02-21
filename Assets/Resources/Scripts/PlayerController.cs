using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public GameManager gameManager;

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

    public Vector3 checkpointPos;

    void Start ()
    {
        gameManager = GetComponent<GameManager>();

        currentGun = 0;

        rb = GetComponent<Rigidbody2D>();
        firePoint = GetComponent<Transform>();
	}
	
	void Update ()
    {
        Fire();
        Dash();
	}

    private void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        float h; 
        float v;
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

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

    void Dash()
    {
        Vector2 currentVelocity;
        float h;
        float v;
        
        h = Mathf.RoundToInt(Input.GetAxis("Horizontal"));
        v = Mathf.RoundToInt(Input.GetAxis("Vertical"));

        Vector2 poep = new Vector2(h, v);
        Debug.Log(poep);
        //Left
        if (h == -1 && v == 0)
        {
            
        }
        //Top Left
        if (h == -1 && v == 1)
        {
           
        }
        //Top
        if (h == 0 && v == 1)
        {

        }
        //Top Right
        if (h == 1 && v == 1)
        {

        }
        //Right
        if (h == 1 && v == 0)
        {
        }
        //Bottom Right
        if (h == 1 && v == -1)
        {

        }
        //Bottom
        if (h == 0 && v == -1)
        {

        }
        //Bottom Left
        if (h == -1 && v == -1)
        {

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
            transform.position = checkpointPos;
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (collision.gameObject.CompareTag("Ground") && rb.velocity.y < 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Killbox") || collision.gameObject.CompareTag("EnemyBullet"))
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            transform.position = checkpointPos;
        }

        if (collision.gameObject.CompareTag("CheckPoint"))
        {
            //Set checkpoint pos to new checkpoint pos
            checkpointPos = collision.gameObject.transform.position;
        }

        if (collision.gameObject.CompareTag("LevelEnd"))
        {
            //Increment level + set player to level start pos
            gameManager.currentLevel += 1;
            transform.position = gameManager.startPositions[gameManager.currentLevel].position;

            //Reset checkpoint when advancing level to level start pos
            checkpointPos = gameManager.startPositions[gameManager.currentLevel].position;
        }
    }    
}
