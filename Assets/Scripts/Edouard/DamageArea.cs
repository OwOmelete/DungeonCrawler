using System;
using UnityEngine;
using UnityEngine.Serialization;

public class DamageOverTimeZone : MonoBehaviour
{
    public TestPlayer testPlayerScriptReference;
    
    bool isInZone = false;
    
    public int damage;
    
    public void DamageOverTime()
    {
        testPlayerScriptReference.health -= damage;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        isInZone = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        isInZone = false;
    }

    private void Update()
    {
        if (isInZone)
        {
            DamageOverTime();
        }
    }
}
