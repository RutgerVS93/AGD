using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{ 
    [SerializeField]
    private Transform[] targetPositions;
    [SerializeField]
    private int currentTargetTransform = 0;

    [SerializeField]
    private Transform targetTransform;

    [SerializeField]
    private float speed;

    void Start()
    {
        targetTransform = targetPositions[currentTargetTransform];
    }

    void Update()
    {
        MovePlatform();

        transform.position = transform.position;
    }

    void MovePlatform()
    {
        foreach (Transform t in targetPositions)
        {
            t.transform.position = t.transform.position;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetTransform.transform.position, speed * Time.deltaTime);

        if (currentTargetTransform >= targetPositions.Length)
        {
            currentTargetTransform = 0;
            targetTransform = targetPositions[currentTargetTransform];
        }

        if (transform.position == targetTransform.transform.position)
        {
            currentTargetTransform++;
            targetTransform = targetPositions[currentTargetTransform];
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.transform.SetParent(gameObject.transform);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.gameObject.transform.SetParent(null);
    }
}
