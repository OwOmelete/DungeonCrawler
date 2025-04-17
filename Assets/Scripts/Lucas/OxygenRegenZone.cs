using System;
using System.Collections;
using UnityEngine;

public class OxygenRegenZone : MonoBehaviour
{
    [SerializeField] private OxygenManager oxygenManagerReference; 
    private bool isInZone = false;
    [SerializeField] private int oxygenRegen = 5; 
    [SerializeField] private float regenInterval = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInZone = true;
            oxygenManagerReference.canLooseOxygen = false; 
            StartCoroutine(RegenOxygenOverTime());
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInZone = false;
            oxygenManagerReference.canLooseOxygen = true;
            oxygenManagerReference.RestartCoroutine();
        }
    }
    private IEnumerator RegenOxygenOverTime()
    {
        while (isInZone)
        {
            oxygenManagerReference.AddOxygen(oxygenRegen);
            oxygenManagerReference.UpdateUi();
            yield return new WaitForSeconds(regenInterval);
        }
    }
}