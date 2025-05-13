using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class OxygenRegenArea : MonoBehaviour
{
    /*private TestPlayer player; 
    private bool isInZone = false;
    private Coroutine regenCoroutine;
    
    [Header("OxygenRegen options")]
    public int oxygen = 5; 
    public float regenInterval = 1f; 

    

    private void OnTriggerEnter2D(Collider2D other)
    {
        player = other.GetComponent<TestPlayer>(); 
        if (player != null)
        {
            isInZone = true;
            player.oxygenLoss = 0; 
            
            if (regenCoroutine == null)
            {
                regenCoroutine = StartCoroutine(RegenOxygenOverTime());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (player != null && other.gameObject == player.gameObject)
        {
            isInZone = false;
            player.oxygenLoss = 1; 
            
            if (regenCoroutine != null)
            {
                StopCoroutine(regenCoroutine);
                regenCoroutine = null;
            }
        }
    }

    private IEnumerator RegenOxygenOverTime()
    {
        while (isInZone && player != null)
        {
            player.oxygen += oxygen;
            player.oxygen = Mathf.Clamp(player.oxygen, 0, 100); 
            //Debug.Log("Oxygen regenerated: " + player.oxygen);
            yield return new WaitForSeconds(regenInterval);
        }
        regenCoroutine = null; 
    }*/
}