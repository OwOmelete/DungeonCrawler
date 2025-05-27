using System.Collections;
using DG.Tweening;
using UnityEngine;


public class LightReload : MonoBehaviour
{
    #region Variables
    
    [Header("Reference")]
    [SerializeField] private LightManager lightManagerReference;    // Reference au light manager
    [SerializeField] private GameObject interactDisplay;    // Texte d'affichage de la touche 
    [SerializeField] private Animator animator;
    
    [Header("Values")]
    [SerializeField] private float regen = 5;   // Quantitée de lumière que va regenerer l'objet
    [SerializeField] private float interactTextFadeDuration = 0.2f; // Temps que va prendre le texte a apparaitre et a disparaitre
    [SerializeField] private float timeToDespawn = 0.3f;
    
    private bool canTake = false;   // Savoir si on peut prendre l'objet
    
    #endregion

    #region Triggers
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("isHere", true);
            canTake = true;
            StartCoroutine(TakeLight());
        }
        
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("isHere", false);
            canTake = false;
        }
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
        gameObject.GetComponent<Transform>().DOScale(Vector3.zero, timeToDespawn).SetEase(Ease.OutCubic);
        lightManagerReference.canLooseLight = false;
        if (lightManagerReference.player.light + regen >= lightManagerReference.maxLight)
        {
            regen = lightManagerReference.maxLight - lightManagerReference.player.light;
        }
        lightManagerReference.AddLight(regen);
        //lightManagerReference.UpdateUi();
        StartCoroutine(DespawnCoroutine());
    }
    IEnumerator DespawnCoroutine()
    {
        yield return new WaitForSeconds(timeToDespawn);
        Destroy(gameObject);
    }
}
