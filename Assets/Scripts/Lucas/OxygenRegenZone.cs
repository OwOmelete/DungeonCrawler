using System.Collections;
using DG.Tweening;
using UnityEngine;


public class OxygenRegenZone : MonoBehaviour
{
    #region Variables

    [Header("Reference")] 
    [SerializeField] private OxygenManager oxygenManagerReference;
    [SerializeField] private GameObject interactDisplay;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject triggerTextRef;
    
    [Header("Values")]
    [SerializeField] private float regen = 100;   
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
                AddOxygen();
            }
            
            yield return null;
        }
    }
    
    void AddOxygen()
    {
        
        oxygenManagerReference.canLooseOxygen = false;
        if (oxygenManagerReference.player.light + regen >= oxygenManagerReference.maxOxygen)
        {
            regen = oxygenManagerReference.maxOxygen - oxygenManagerReference.player.light;
        }
        oxygenManagerReference.AddOxygen(regen);
        oxygenManagerReference.UpdateUi();
        if (InteractTextManager.INSTANCE.firstPlant)
        {
            InteractTextManager.INSTANCE.firstPlant = false;
            triggerTextRef.SetActive(true);
        }
        gameObject.SetActive(false);
    }
}
