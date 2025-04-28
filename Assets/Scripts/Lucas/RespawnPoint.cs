using System.Collections;
using DG.Tweening;
using UnityEngine;


public class RespawnPoint : MonoBehaviour
{
    #region Variables
    
    [Header("Reference")]
    [SerializeField] private GameObject interactDisplay;    // Texte d'affichage de la touche 
    [SerializeField] private DeathManager deathManagerRef;
    
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
            interactDisplay.GetComponent<SpriteRenderer>().DOFade(1f, interactTextFadeDuration);
            canTake = true;
            StartCoroutine(TakeLight());
        }
        
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactDisplay.GetComponent<SpriteRenderer>().DOFade(0f, interactTextFadeDuration);
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
                ChangePoint();
            }
            yield return null;
        }
    }
    
    void ChangePoint()
    {
        deathManagerRef.respawnPosition = transform.position;
    }
    IEnumerator DespawnCoroutine()
    {
        yield return new WaitForSeconds(timeToDespawn);
        Destroy(gameObject);
    }
}
