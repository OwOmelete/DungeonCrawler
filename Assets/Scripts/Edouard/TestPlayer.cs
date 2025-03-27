using UnityEngine;
using System.Collections;

public class TestPlayer : MonoBehaviour
{
    public int health;
    private int maxHealth = 100;
    
    private void Death()
    {
        if (health <= 0)
        {
            Destroy(gameObject); 
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