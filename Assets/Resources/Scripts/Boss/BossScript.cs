using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour {

    public HealthScript headHealth, leftHandHealth, rightHandHealth;

    public GameObject leftHand, rightHand;
    public Transform leftFirePoint, rightFirePoint, headFirePoint;
    public GameObject head;

    public GameObject debris;
    public int numberOfDebris;
    public float debrisDelay;

    public float groundFireDelay;
    public float airFireDelay;
    public float headFireDelay;

    public bool debrisActive = false;
    public bool debrisAttack = false;

    public bool groundActive = false;
    public bool groundAttack = false;

    public bool airActive = false;
    public bool airAttack = false;

    public bool headActive = false;
    public bool headAttack = false;

    public static bool reset = true;

    public GameObject smallProjectile;
    private GameObject smallProjectileClone;

    public GameObject bigProjectile;
    private GameObject bigProjectileClone;
    private GameObject bigProjectileClone2;

    public int numberOfBigProjectiles;
    public float bigProjectileSpeed;

    public int numberOfSmallProjectiles;
    public float smallProjectileSpeed;

    public Vector2[] targetPoints;
    public Vector3[] rotationValues;

    public float attackCooldown;
    public float handSpeed;
    public float rotationSpeed;

    private IEnumerator dropDebrisAttack;
    private IEnumerator fireOnGroundAttack;

    public bool leftHandAlive = true;
    public bool rightHandAlive = true;
    public bool headAlive = true;

	void Start ()
    {
        headHealth = head.GetComponent<HealthScript>();
        leftHandHealth = leftHand.GetComponent<HealthScript>();
        rightHandHealth = rightHand.GetComponent<HealthScript>();

        dropDebrisAttack = DropDebris();
        fireOnGroundAttack = FireOnGround();

        reset = true;
	}

    void FixedUpdate ()
    {
        if (debrisAttack)
        {
            MoveToDebrisAttack();
        }

        if (groundAttack)
        {
            MoveToGroundShootAttack();
        }

        if (headAttack)
        {
            HeadAttack();
        }

        if (airAttack)
        {
            FireInAirAttack();
        }

        if (reset)
        {
            ResetHands();
            if (leftHand.transform.position.x >= -3.8)
            {
                reset = false;
                Invoke("SelectNewAttack", attackCooldown);
            }
        }
    }

    //Move Hand to point
    void MoveToPoint(GameObject hand, Vector2 targetPos)
    {
        //hand.transform.position = Vector2.MoveTowards(hand.transform.position, targetPos, handSpeed * Time.deltaTime);
        hand.transform.position = Vector2.Lerp(hand.transform.position, targetPos, Time.deltaTime * handSpeed);
    }

    //Rotate Hand to rotation
    void RotateToTargetRotation(GameObject hand, Vector3 targetRotation)
    {
        hand.transform.rotation = Quaternion.RotateTowards(hand.transform.rotation, Quaternion.Euler(targetRotation), rotationSpeed * Time.deltaTime);
        //hand.transform.rotation = Quaternion.Lerp(hand.transform.rotation, Quaternion.Euler(targetRotation), rotationSpeed * Time.fixedDeltaTime);
    }

    void MoveToDebrisAttack()
    {
        RotateToTargetRotation(leftHand, rotationValues[0]);
        RotateToTargetRotation(rightHand, rotationValues[1]);

        //Slam
        if (leftHand.transform.position.x <= -5.999 && rightHand.transform.position.x >= 5.999)
        {
            handSpeed = 10;
            MoveToPoint(leftHand, targetPoints[6]);
            MoveToPoint(rightHand, targetPoints[7]);
        }
        else
        {
            MoveToPoint(leftHand, targetPoints[2]);
            MoveToPoint(rightHand, targetPoints[3]);
        }

        //Start Attack
        if (leftHand.transform.position.x <= -5.8 && leftHand.transform.position.y <= -2.99 && debrisActive)
        {
            StartCoroutine(DropDebris());
            StopCoroutine(DropDebris());
            handSpeed = 3;
        }
    }

    void MoveToGroundShootAttack()
    {
        MoveToPoint(leftHand, targetPoints[4]);
        MoveToPoint(rightHand, targetPoints[5]);
        RotateToTargetRotation(leftHand, rotationValues[2]);
        RotateToTargetRotation(rightHand, rotationValues[3]);

        if (leftHand.transform.position.x <= -5.999 && leftHand.transform.position.y <= -2.999 && groundActive)
        {
            StartCoroutine(FireOnGround());
            StopCoroutine(FireOnGround());
        }
    }

    void HeadAttack()
    {
        if (headActive)
        {
            StartCoroutine(FireFromHead());
            StopCoroutine(FireFromHead());
        }
    }

    void FireInAirAttack()
    {
        RotateToTargetRotation(leftHand, rotationValues[0]);
        RotateToTargetRotation(rightHand, rotationValues[1]);
        handSpeed = 1;

        if (leftHand.transform.position.y >= 1.9)
        {
            leftHand.transform.position = Vector2.MoveTowards(leftHand.transform.position, targetPoints[9], handSpeed * Time.deltaTime);
            rightHand.transform.position = Vector2.MoveTowards(rightHand.transform.position, targetPoints[8], handSpeed * Time.deltaTime);
            if (airActive)
            {
                StartCoroutine(FireInAir());
                StopCoroutine(FireInAir());
            }            
        }
        else
        {
            MoveToPoint(leftHand, targetPoints[8]);
            MoveToPoint(rightHand, targetPoints[9]);
        }
        
    }

    void ResetHands()
    {
        debrisActive = false;
        debrisAttack = false;
        groundActive = false;
        groundAttack = false;
        airActive = false;
        airAttack = false;
        headActive = false;
        headAttack = false;

        StopAllCoroutines();
        MoveToPoint(leftHand, targetPoints[0]);
        MoveToPoint(rightHand, targetPoints[1]);
        handSpeed = 3;
    }

    void SelectNewAttack()
    {
        int rngNumber = Random.Range(0, 4);
        switch (rngNumber)
        {
            case 0:
                groundAttack = true;
                groundActive = true;
                break;

            case 1:
                debrisAttack = true;
                debrisActive = true;
                break;

            case 2:
                headAttack = true;
                headActive = true;
                break;

            case 3:
                airAttack = true;
                airActive = true;
                break;
        }
    }

    IEnumerator DropDebris()
    {
        debrisActive = false;
        for (int i = 0; i < numberOfDebris; i++)
        {
            GameObject debrisClone = Instantiate(debris);

            float randomPos = Random.Range(-7, 7);
            debrisClone.transform.position = new Vector2(randomPos, 6);
            yield return new WaitForSeconds(debrisDelay);
        }
        debrisAttack = false;
        reset = true;
    }

    IEnumerator FireOnGround()
    {
        groundActive = false;
        for (int i = 0; i < numberOfBigProjectiles; i++)
        {
            if (leftHand.activeSelf)
            {
                bigProjectileClone = Instantiate(bigProjectile, leftFirePoint.position, leftFirePoint.rotation);
                bigProjectileClone.AddComponent<Rigidbody2D>();
                bigProjectileClone.GetComponent<Rigidbody2D>().gravityScale = 0;
                bigProjectileClone.GetComponent<Rigidbody2D>().AddForce(Vector2.right * bigProjectileSpeed);

                Destroy(bigProjectileClone, 5f);
            }

            if (rightHand.activeSelf)
            {
                bigProjectileClone2 = Instantiate(bigProjectile, rightFirePoint.position, rightFirePoint.rotation);
                bigProjectileClone2.AddComponent<Rigidbody2D>();
                bigProjectileClone2.GetComponent<Rigidbody2D>().gravityScale = 0;
                bigProjectileClone2.GetComponent<Rigidbody2D>().AddForce(Vector2.left * bigProjectileSpeed);

                Destroy(bigProjectileClone2, 5f);
            }       
            
            yield return new WaitForSeconds(groundFireDelay);
        }
        groundAttack = false;
        reset = true;
    }

    IEnumerator FireInAir()
    {
        airActive = false;
        for (int i = 0; i < numberOfBigProjectiles; i++)
        {
            if (leftHand.activeSelf)
            {
                GameObject bigProjectileClone1 = Instantiate(bigProjectile, leftFirePoint.position, leftFirePoint.rotation);
                bigProjectileClone1.AddComponent<Rigidbody2D>();
                bigProjectileClone1.GetComponent<Rigidbody2D>().gravityScale = 0;
                bigProjectileClone1.GetComponent<Rigidbody2D>().AddForce(Vector2.down * bigProjectileSpeed);

                Destroy(bigProjectileClone1, 5f);
            }

            if (rightHand.activeSelf)
            {
                GameObject bigProjectileClone2 = Instantiate(bigProjectile, rightFirePoint.position, rightFirePoint.rotation);
                bigProjectileClone2.AddComponent<Rigidbody2D>();
                bigProjectileClone2.GetComponent<Rigidbody2D>().gravityScale = 0;
                bigProjectileClone2.GetComponent<Rigidbody2D>().AddForce(Vector2.down * bigProjectileSpeed);

                Destroy(bigProjectileClone2, 5f);
            }    

            yield return new WaitForSeconds(airFireDelay);
        }
        airAttack = false;
        reset = true;
    }

    IEnumerator FireFromHead()
    {
        headActive = false;
        for (int i = 0; i < numberOfSmallProjectiles; i++)
        {
            smallProjectileClone = Instantiate(smallProjectile, headFirePoint.position, leftFirePoint.rotation);
            smallProjectileClone.AddComponent<Rigidbody2D>();
            smallProjectileClone.GetComponent<Rigidbody2D>().gravityScale = 0;
            GameObject target = GameObject.FindGameObjectWithTag("Player");
            Vector2 moveDirection = (target.transform.position - smallProjectileClone.transform.position).normalized * smallProjectileSpeed;
            smallProjectileClone.GetComponent<Rigidbody2D>().velocity = new Vector2(moveDirection.x, moveDirection.y);
            Destroy(smallProjectileClone, 5f);
            yield return new WaitForSeconds(headFireDelay);
        }
        headAttack = false;
        reset = true;
    }
}
