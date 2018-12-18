using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHandScript : MonoBehaviour {

    public bool isAlive;
    public BossScript bossScript;
    public HealthScript healthScript;
    
    public Rigidbody2D rb;

    public GameObject deadHand;

    public int reviveTime;

    SpriteRenderer rend;
    Color startColor;

	void Start ()
    {
        bossScript = GetComponent<BossScript>();
        healthScript = GetComponent<HealthScript>();
        isAlive = true;

        Instantiate(deadHand);
        deadHand.SetActive(false);

        rend = GetComponent<SpriteRenderer>();
        startColor = rend.color;
    }
	
	void Update ()
    {
        deadHand.transform.position = transform.position;
        deadHand.transform.rotation = transform.rotation;

        KillHand();
	}

    void KillHand()
    {
        if (healthScript.dead)
        {
            deadHand.SetActive(true);
            gameObject.SetActive(false);
            Invoke("ReviveHand", reviveTime);
        }
    }

    void ReviveHand()
    {
        rend.color = startColor;

        BossScript.reset = true;
        healthScript.currentHealth = healthScript.maxHealth;
        healthScript.dead = false;
        deadHand.SetActive(false);
        gameObject.SetActive(true);
        transform.position = deadHand.transform.position;
    }
}
