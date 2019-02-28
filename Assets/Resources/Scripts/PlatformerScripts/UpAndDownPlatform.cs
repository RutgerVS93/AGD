using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpAndDownPlatform : MonoBehaviour
{
    public float speed;
    public Transform[] targetPoints;
    public int currentTarget;
    public float number = .1f;

    void Start()
    {
        currentTarget = 0;
    }

    void Update()
    {
        UpAndDownMovement();
    }

    void UpAndDownMovement()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPoints[currentTarget].transform.position, Time.deltaTime * speed);
        if (transform.position == targetPoints[currentTarget].transform.position)
        {
            currentTarget++;
            if (currentTarget >= targetPoints.Length)
            {
                currentTarget = 0;
            }
        }
    }
}
