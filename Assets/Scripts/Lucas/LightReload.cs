using System.Collections;
using DG.Tweening;
using UnityEngine;


public class LightReload : MonoBehaviour
{
    #region Variables
    
    [Header("Reference")]
    [SerializeField] private LightManager lightManagerReference;
    [SerializeField] private GameObject interactDisplay;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject triggerTextRef;
    
    [Header("Values")]
    [SerializeField] private float regen = 5; 
    [SerializeField] private float interactTextFadeDuration = 0.2f;
    
    private bool canTake = false; 
    
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
        lightManagerReference.canLooseLight = false;
        if (lightManagerReference.player.light + regen >= lightManagerReference.maxLight)
        {
            regen = lightManagerReference.maxLight - lightManagerReference.player.light;
        }
        lightManagerReference.AddLight(regen);
        if (InteractTextManager.INSTANCE.firstPlant)
        {
            InteractTextManager.INSTANCE.firstPlant = false;
            triggerTextRef.SetActive(true);
        }
        gameObject.SetActive(false);
    }
}
