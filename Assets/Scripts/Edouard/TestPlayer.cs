using System;
using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.Serialization;

public class TestPlayer : MonoBehaviour
{
    public int health;
    private int maxHealth = 100;
    public bool debugPauseEditorOnDeath;

    void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        if (health <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        Debug.Log(gameObject.name + " is death");
        if (debugPauseEditorOnDeath)
        {
            Debug.Break();
        }
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        health = Mathf.Clamp(health, 0, maxHealth); 
        Debug.Log("Player took " + damageAmount + " damage. Health: " + health);
        Death(); 
    }
}