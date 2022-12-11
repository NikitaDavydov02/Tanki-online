using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour, IGameManager {
    public ManagerStatus status { get; private set; }
    public int health { get; private set; }
    public int maxHealth { get; private set; }

    public void Startup()
    {
        Debug.Log("Player manager starting...");
        health = 100;
        maxHealth = 100;
        status = ManagerStatus.Started;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void ChangeHealth(int value)
    {
        health += value;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        else if (health < 0)
        {
            health = 0;
        }
        Debug.Log("Health: " + health + "/" + maxHealth);
    }
}
