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

    private void Start()
    {
        InvokeRepeating("DisappearingPlatform", 1f, delay);
    }

    void Update()
    {
        if (rotatePlatform)
        {
            RotatePlatform();
        }
    }

    void RotatePlatform()
    {
        transform.Rotate(Vector3.forward * rotSpeed);
    }


    //Add Fade in / Fade out effect
    void DisappearingPlatform()
    {
        isActive = !isActive;
        transform.gameObject.SetActive(isActive);
    }
}
