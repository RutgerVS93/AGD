using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyScript : MonoBehaviour {

    public float speed;
    public bool checkForFloor;
    Rigidbody2D rb;

	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
	}
	
	void Update ()
    {
        Movement();
        CheckSides();
	}

    void Movement()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void CheckSides()
    {
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position, Vector2.right);
        float distance1 = Mathf.Abs(hit1.point.x - transform.position.x);
        if (distance1 <= .3f)
        {
            speed *= -1;
        }

        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, Vector2.left);
        float distance2 = Mathf.Abs(hit2.point.x - transform.position.x);
        if (distance2 <= .3f)
        {
            speed *= -1;
        }

        if (checkForFloor)
        {
            RaycastHit2D hit3 = Physics2D.Raycast(transform.position, Vector2.down);
            float distance3 = Mathf.Abs(hit3.point.y - transform.position.y);
            if (distance3 >= 0.3f)
            {
                speed *= -1;
            }
        }
    }
}
