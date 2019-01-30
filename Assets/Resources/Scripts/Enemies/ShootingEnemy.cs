using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour {

    public GameObject player;

    public GameObject bullet;
    private GameObject bulletClone;

    public Transform firePoint;
    bool facingRight = true;
    bool canFire = true;

	void Start ()
    {
        player = GameObject.Find("Player");
    }	

	void Update ()
    {
        CheckPlayerPos();
        if (canFire)
        {
            StartCoroutine(Fire());
        }
	}

    IEnumerator Fire()
    {
        canFire = false;
        bulletClone = Instantiate(bullet, firePoint.position, firePoint.rotation);
        if (!facingRight)
        {
            bulletClone.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 750);
        }
        else
        {
            bulletClone.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 750);
        }
        yield return new WaitForSeconds(2f);
        canFire = true;
    }

    void CheckPlayerPos()
    {
        if (player.transform.position.x < gameObject.transform.position.x && !facingRight)
        {
            Flip();
        }
        else if (player.transform.position.x > gameObject.transform.position.x && facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale *= -1;
        transform.localScale = scale;
    }
}
