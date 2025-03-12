using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LightReload : MonoBehaviour
{
    #region Variables
    
    [Header("Reference")]
    [SerializeField] private LightManager lightManagerReference;    // Reference au light manager
    [SerializeField] private GameObject interactDisplay;    // Texte d'affichage de la touche 
    
    [Header("Values")]
    [SerializeField] private float regen = 5;   // Quantitée de lumière que va regenerer l'objet
    [SerializeField] private float lerpDuration = 0.5f; // Temps que va prendre la lumière a se restaurer totalement
    [SerializeField] private float interactTextFadeDuration = 0.2f; // Temps que va prendre le texte a apparaitre et a disparaitre
    
    private bool canTake = false;   // Savoir si on peut prendre l'objet
    
    #endregion

    #region Triggers
    private void OnTriggerEnter2D(Collider2D other)
    {
        interactDisplay.GetComponent<SpriteRenderer>().DOFade(1f, interactTextFadeDuration);
        canTake = true;
        StartCoroutine(TakeLight());
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        interactDisplay.GetComponent<SpriteRenderer>().DOFade(0f, interactTextFadeDuration);
        canTake = false;
    }
    #endregion
    
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
    
    void AddLight()
    {
        Destroy(GetComponent<Collider2D>());
        Destroy(GetComponent<SpriteRenderer>());
        lightManagerReference.canLooseLight = false;
        if (lightManagerReference.actualLight + regen >= lightManagerReference.maxLight)
        {
            regen = lightManagerReference.maxLight - lightManagerReference.actualLight;
        }
        lightManagerReference.AddLight(regen);
    }
    
}
