using UnityEngine;
using UnityEngine.Serialization;

public class OxigenRegenZone : MonoBehaviour
{
    public TestPlayer testPlayerScriptReference;
    
    bool isInZone = false;
    
    public int oxygenRegen;
    
    public void RegenOxygenOverTime()
    {
        testPlayerScriptReference.oxygen += oxygenRegen;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        isInZone = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        isInZone = false;
    }

    private void FixedUpdate()
    {
        if (isInZone)
        {
            RegenOxygenOverTime();
        }
    }
}
