using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallObstacle : MonoBehaviour {

    public GameObject fireBall;
    public Transform startPoint;
    public float launchForce;
    public float coolDown;

    public Rigidbody2D rb;

	void Start ()
    {
        //rb = GetComponent<Rigidbody2D>();
	}
	
	void Update ()
    {
        if (fireBall.transform.position.y <= startPoint.position.y)
        {
            rb.velocity = Vector2.zero;
            Launch();
        }
        //Launch();
	}

    void Launch()
    {
        rb.velocity = new Vector2(rb.velocity.x, launchForce);
    }
}
