using System.Collections;
using DG.Tweening;
using UnityEngine;

public class UpgradeRessourcesObject : MonoBehaviour
{
    #region Variables
    
    [Header("Reference")]
    [SerializeField] private GameObject interactDisplay;    // Texte d'affichage de la touche 
    [SerializeField] private Animator animator;
    [SerializeField] private UpgradeRessourcesManager upgradeRessourcesReference;
    
    [Header("Values")]
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
                AddRessource();
            }
            yield return null;
        }
    }
    
    void AddRessource()
    {
        Destroy(GetComponent<Collider2D>());
        gameObject.transform.DOScale(Vector3.zero, timeToDespawn).SetEase(Ease.OutCubic);
        upgradeRessourcesReference.AddRessources();
        StartCoroutine(DespawnCoroutine());
    }
    IEnumerator DespawnCoroutine()
    {
        yield return new WaitForSeconds(timeToDespawn);
        Destroy(gameObject);
    }
}
