using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitHazard : MonoBehaviour
{
    [SerializeField] private GameObject orbitHazard;

    [SerializeField] private Vector3 targetDistance;

    [SerializeField] private float speed, radius;
    [Space]
    [SerializeField] private bool changingDistance;

    [SerializeField] private int minDistance, maxDistance;
    [SerializeField] private float distanceSpeed;

    private Vector3 parentPos;
    private Vector3 hazardPos;

    void Start()
    {
        parentPos = transform.position;
        orbitHazard.transform.position = (orbitHazard.transform.position - parentPos).normalized * radius + parentPos;
    }

    void Update()
    {
        RotateAround();
    }

    void RotateAround()
    {
        orbitHazard.transform.RotateAround(parentPos, Vector3.forward, speed * Time.deltaTime);
        targetDistance = (orbitHazard.transform.position - parentPos).normalized * radius + parentPos;
        orbitHazard.transform.position = Vector3.MoveTowards(orbitHazard.transform.position, targetDistance, Time.deltaTime * distanceSpeed);
        //orbitHazard.transform.position = hazardPos;
    }
}
