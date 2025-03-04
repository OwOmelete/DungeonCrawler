using System;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    //ATTENTION!!! Ce srcipt n'est qu'un test, ne pas le confondre avec le vraie PLAYER!
    public int health;
    int maxHealth = 100;

    public int oxygen;
    int maxOxygen = 100;
    public int oxygenLoss;

    private void Start()
    {
        health = maxHealth;
        oxygen = maxOxygen;
    }

    private void Update()
    {
        oxygen -= oxygenLoss;
    }
    void Death()
    {
        if (health <= 0)
        {
            Debug.Log("DEAD");
        }
    }
}
