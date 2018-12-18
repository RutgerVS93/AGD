using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun")]
public class Gun : ScriptableObject {

    public string gunName;

    public int damage;
    public float bulletSpeed;
    public float fireRate;
    public float bulletLifeTime;

    public int bulletNumber;

    public bool spread;
    public float staticSpreadValue;

    public bool randomSpread;
    public Vector2 randomSpreadValue;

    public GameObject bulletParticle;
    public GameObject impactParticle;

    public AudioClip shootSFX;
    public AudioClip impactSFX;
}
