using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawnImpact : MonoBehaviour {

    public GameObject impactParticle;
    public string ignoreTag1, ignoreTag2;

    private ParticleRenderer pRend;

    private void Start()
    {
        //pRend = GetComponent<ParticleRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "BossVitalPart" || collision.gameObject.tag != "BossPart")
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        GameObject impactParticleClone = Instantiate(impactParticle, transform.position, Quaternion.identity);
        Destroy(impactParticleClone, 2f);
    }
}
