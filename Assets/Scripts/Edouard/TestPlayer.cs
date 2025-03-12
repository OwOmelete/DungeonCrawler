using UnityEngine;
using System.Collections;

public class TestPlayer : MonoBehaviour
{
    // ATTENTION!!! Ce script n'est qu'un test, ne pas le confondre avec le vrai PLAYER!
    public int health;
    private int maxHealth = 100;

    public int oxygen;
    private int maxOxygen = 100;
    public int oxygenLoss = 1; 
    public float oxygenLossInterval = 2f; 

    private void Start()
    {
        health = maxHealth;
        oxygen = maxOxygen;
        StartCoroutine(OxygenLossRoutine()); 
    }

    private IEnumerator OxygenLossRoutine()
    {
        while (oxygen > 0)
        {
            yield return new WaitForSeconds(oxygenLossInterval);
            oxygen -= oxygenLoss;
            oxygen = Mathf.Clamp(oxygen, 0, 100);
            //Debug.Log("Oxygen: " + oxygen);
            Death();
        }
    }

    private void Death()
    {
        if (health <= 0 || oxygen <= 0)
        {
            Debug.Log("DEAD");
            Destroy(gameObject); 
        }
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        health = Mathf.Clamp(health, 0, maxHealth); 
        //Debug.Log("Player took " + damageAmount + " damage. Health: " + health);

        Death(); 
    }
}