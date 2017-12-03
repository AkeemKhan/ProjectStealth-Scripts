using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour {


    public float health;
    public float maxHealth;
    public float maxSpeed;
    public float sneakSpeed;
    public PlayerController playerController;

    public bool dead = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        CheckHealth();
    }

    public void DamagerPlayer(float change)
    {
        health -= change;
    }

    public void HealPlayer(float change)
    {
        health += change;
        if (health >= maxHealth)
            health = maxHealth;
    }

    public void CheckHealth()
    {
        if(health <= 0)
        {
            if (!dead) SignalDeath();
        }
    }

    public void SignalDeath()
    {
        playerController.SignalDefeat();
    }


}
