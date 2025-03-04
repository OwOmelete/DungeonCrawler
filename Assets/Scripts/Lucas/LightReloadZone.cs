using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightReloadZone : MonoBehaviour
{
    [SerializeField] private float regenPerSeconds = 5;
    [SerializeField] private LightManager lightManagerReference;
    private bool isHere;
    private void OnTriggerEnter2D(Collider2D other)
    {
        isHere = true;
        StartCoroutine(GiveLightToPlayer());
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        isHere = false;
    }

    IEnumerator GiveLightToPlayer()
    {
        while (isHere)
        {
            yield return new WaitForSeconds(1f);
            lightManagerReference.AddLight(regenPerSeconds);
        }
    }

    
}
