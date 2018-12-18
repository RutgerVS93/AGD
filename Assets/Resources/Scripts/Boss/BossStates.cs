using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStates : MonoBehaviour {

    public HealthScript health;
    public BossScript bossScript;

    public GameObject boss;
    public GameObject head;

    public enum State
    {
        Phase1,
        Phase2,
        Phase3
    }

    public State state;

	void Start ()
    {
        health = head.GetComponent<HealthScript>();
        bossScript = boss.GetComponent<BossScript>();
	}
	
	void Update ()
    {
        CheckHealth();
        AdjustVariablesToPhase();
	}

    //Checks health and adjusts state
    void CheckHealth()
    {
        if (health.currentHealth == health.maxHealth)
        {
            state = State.Phase1;
        }
        else if (health.currentHealth == health.maxHealth / 2)
        {
            state = State.Phase2;
        }
        else if (health.currentHealth == health.maxHealth / 4)
        {
            state = State.Phase3;
        }
    }

    void AdjustVariablesToPhase()
    {
        if (state == State.Phase1)
        {
            bossScript.numberOfDebris = 10;

            bossScript.debrisDelay = .7f;
            bossScript.groundFireDelay = 2f;
            bossScript.airFireDelay = 2f;
            bossScript.headFireDelay = 2f;

            bossScript.numberOfSmallProjectiles = 10;
            bossScript.numberOfBigProjectiles = 5;

            bossScript.smallProjectileSpeed = 7;
            bossScript.bigProjectileSpeed = 200;

            bossScript.attackCooldown = 2f;
        }
        else if (state == State.Phase2)
        {
            bossScript.numberOfDebris = 15;

            bossScript.debrisDelay = .7f;
            bossScript.groundFireDelay = 1.5f;
            bossScript.airFireDelay = 1.5f;
            bossScript.headFireDelay = 1.5f;

            bossScript.numberOfSmallProjectiles = 10;
            bossScript.numberOfBigProjectiles = 5;

            bossScript.smallProjectileSpeed = 8;
            bossScript.bigProjectileSpeed = 225;

            bossScript.attackCooldown = 1.5f;
        }
        else if (state == State.Phase3)
        {
            bossScript.numberOfDebris = 20;

            bossScript.debrisDelay = .7f;
            bossScript.groundFireDelay = 1f;
            bossScript.airFireDelay = 1f;
            bossScript.headFireDelay = 1f;

            bossScript.numberOfSmallProjectiles = 10;
            bossScript.numberOfBigProjectiles = 5;

            bossScript.smallProjectileSpeed = 10;
            bossScript.bigProjectileSpeed = 250;

            bossScript.attackCooldown = 1f;
        }
    }
}
