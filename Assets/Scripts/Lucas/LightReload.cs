using System;
using System.Collections;
using UnityEngine;

public class LightReload : MonoBehaviour
{
    [SerializeField] private float regen = 5;
    [SerializeField] private LightManager lightManagerReference;
    [SerializeField] private GameObject interactDisplay;
    [SerializeField] private float lerpSpeed = 0.1f;
    private bool canTake = false;
    private float t = 0;
    
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        interactDisplay.SetActive(true);
        canTake = true;
        StartCoroutine(TakeLight());
    }

    IEnumerator TakeLight()
    {
        while (canTake)
        {
            if (Input.GetButtonDown("Interact"))
            {
                AddLight();
            }
            yield return null;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        interactDisplay.SetActive(false);
        canTake = false;
    }

    void AddLight()
    {
        Destroy(GetComponent<Collider2D>());
        Destroy(GetComponent<SpriteRenderer>());
        lightManagerReference.canLooseLight = false;
        if (lightManagerReference.actualLight + regen >= lightManagerReference.maxLight)
        {
            regen = lightManagerReference.maxLight - lightManagerReference.actualLight;
        }
        StartCoroutine(LerpLight());
        
    }

    IEnumerator LerpLight()
    {
        t = 0;
        while (t < 1)
        {
            lightManagerReference.playerLight.pointLightOuterRadius = Mathf.SmoothStep(lightManagerReference.actualLight , lightManagerReference.actualLight + regen, t);
            lightManagerReference.playerLight.pointLightInnerRadius = Mathf.SmoothStep(lightManagerReference.actualLight , lightManagerReference.actualLight + regen, t) / 2;
            t += lerpSpeed * Time.deltaTime;
            yield return null;
        }
        
        lightManagerReference.AddLight(regen);
        Destroy(gameObject);
    }
    
}
