using System;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    //ATTENTION!!! Ce srcipt n'est qu'un test, ne pas le confondre avec le vraie PLAYER!
    public int health;
    int maxHealth = 100;

    private void Start()
    {
        health = maxHealth;
    }

    void Death()
    {
        if (health <= 0)
        {
            Debug.Log("DEAD");
        }
    }
}
