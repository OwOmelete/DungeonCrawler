using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightManager : MonoBehaviour
{
    public float maxLight = 100;  // capacité maximale de lumière
    public Light2D playerLight;   // objet light du player pour changer son intensité
    [HideInInspector] public bool canLooseLight = true; // ici pour stopper la perte de lumière dans certains cas
    [HideInInspector]public float actualLight;  // valeure actuelle de la lumière
    
    [SerializeField] private float minLight = 1;  // capacité minimale de lumière
    [SerializeField] private float looseLightValue = 1; // ce que va perdre la jauge de lumiere toutes les secondes
    [SerializeField] private float lerpSpeed = 0.01f; // ce que va perdre la jauge de lumiere toutes les secondes
    
    private bool haveLight = true; // verifie si il reste de la lumière
    private float t = 0;    // sers a faire une baisse de lumière smooth 

    

    private void Start()
    {
        actualLight = maxLight;
        playerLight.pointLightOuterRadius = actualLight;
        playerLight.pointLightInnerRadius = actualLight / 2;
        StartLooseLight();
    }

    IEnumerator LooseLight()    // coroutine qui fait baisser peut a peu le niveau de lumiere
    {
        while (haveLight && canLooseLight)
        {
            actualLight -= looseLightValue;
            StartCoroutine(LerpLight(lerpSpeed));
            if (actualLight <= minLight)
            {
                haveLight = false;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator LerpLight(float speed)
    {
        t = 0;
        while (playerLight.pointLightOuterRadius != actualLight)
        {
            playerLight.pointLightOuterRadius = Mathf.SmoothStep(actualLight + looseLightValue, actualLight, t);
            playerLight.pointLightInnerRadius = Mathf.SmoothStep(actualLight + looseLightValue, actualLight, t) / 2;
            t += speed * Time.deltaTime;
            yield return null;
        }
        playerLight.pointLightOuterRadius = actualLight;
        playerLight.pointLightInnerRadius = actualLight / 2;
    }

    public void AddLight(float value)   // recharge la lumière
    {
        actualLight += value;
        
        if (!haveLight || !canLooseLight)
        {
            haveLight = true;
            canLooseLight = true;
            StartLooseLight();
        }
    }

    private void StartLooseLight()
    {
        StartCoroutine(LooseLight());
    }

    
    
}
