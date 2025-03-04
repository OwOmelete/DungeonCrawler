using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightManager : MonoBehaviour
{
    [SerializeField] private Light2D playerLight;   // objet light du player pour changer son intensité
    [SerializeField] private float maxLight = 100;  // capacité maximale de lumière
    [SerializeField] private float looseLightValue = 1; // ce que va perdre la jauge de lumiere toutes les secondes
    private GameObject playerReference;
    [SerializeField] private float actualLight;  // valeure actuelle de la lumière
    private bool lightActive = true; // verifie si il reste de la lumière

    private void Start()
    {
        playerReference = GameObject.FindGameObjectWithTag("Player");
        actualLight = maxLight;
        StartCoroutine(LooseLight());
    }

    IEnumerator LooseLight()    // coroutine qui  fait baisser peut a peu la lumiere
    {
        while (lightActive)
        {
            yield return new WaitForSeconds(1f);
            actualLight -= looseLightValue;
            if (actualLight <= 0)
            {
                lightActive = false;
                playerLight.intensity = 0;
                playerReference.GetComponent<Animation>().Play("LightPlayerOff");   // animation de clignotement 
            }
        }
    }

    public void AddLight(float value)   // recharge la lumière
    {
        if (actualLight + value >=maxLight)
        {
            actualLight = maxLight;
        }
        else
        {
            actualLight += value;
        }
        if (!lightActive)
        {
            playerReference.GetComponent<Animation>().Play("LightPlayerOn");
            lightActive = true;
            StartCoroutine(LooseLight());
        }
    }
    
}
