using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMutators : MonoBehaviour
{
    [Header("Rotating Platform")]
    [SerializeField]
    private float rotSpeed;
    [SerializeField]
    private bool rotatePlatform;

    [Header("Disappearing Platform")]
    [SerializeField]
    private bool disappearingPlatform;
    [SerializeField]
    private bool isActive;
    [SerializeField]
    private int delay;
    public float decreaseAmount;

    [Header("Falling Platform")]
    public bool fallingPlatform;
    public Transform startPos;
    public Transform endPos;
    public float fallSpeed;

    private void Start()
    {
        if (disappearingPlatform)
        {
            InvokeRepeating("DisappearingPlatform", 1f, delay);
        }
    }

    void Update()
    {
        if (rotatePlatform)
        {
            RotatePlatform();
        }

        if (fallingPlatform)
        {
            FallingPlatform();
        }
    }

    void RotatePlatform()
    {
        transform.Rotate(Vector3.forward * rotSpeed);
    }

    void FallingPlatform()
    {
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
        if (transform.position.y <= endPos.position.y)
        {
            transform.position = startPos.position;
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
