using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;


public class RespawnPoint : MonoBehaviour
{
    #region Variables
    
    [Header("Reference")]
    [SerializeField] private GameObject interactDisplay;    // Texte d'affichage de la touche 
    [SerializeField] private DeathManager deathManagerRef;
    [SerializeField] private Animator animator;
    [SerializeField] private TMP_Text textZoneRef;
    
    [Header("Values")]
    [SerializeField] private float interactTextFadeDuration = 0.2f; // Temps que va prendre le texte a apparaitre et a disparaitre
    [SerializeField] private float timeToDespawn = 0.3f;
    [SerializeField] private string text;

    private bool check = false;
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
        while (canTake || check)
        {
            if (Input.GetButtonDown("Interact"))
            {
                if (!check)
                {
                    ChangePoint();
                }
                else
                {
                    EndText();
                }
            }
            yield return null;
        }
    }
    
    void ChangePoint()
    {
        textZoneRef.DOFade(1, 0.2f); 
        textZoneRef.text = text;
        deathManagerRef.respawnPosition = transform.position;
        check = true;
    }

    void EndText()
    {
        textZoneRef.DOFade(0, 0.2f);
        check = false;
    }
    
}
