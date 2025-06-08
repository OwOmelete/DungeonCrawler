using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class RespawnPoint : MonoBehaviour
{
    #region Variables
    
    [Header("Reference")]
    [SerializeField] private GameObject interactDisplay;
    [SerializeField] private DeathManager deathManagerRef;
    [SerializeField] private Animator animator;
    [SerializeField] private TMP_Text textZoneRef;
    [SerializeField] private Image imageButtonRef;
    
    [Header("Values")]
    [SerializeField] private float interactTextFadeDuration = 0.5f;
    [SerializeField] private float timeToDespawn = 0.3f;
    [SerializeField] private string text;
    [SerializeField] private GameObject balise;

    private bool check = false;
    private bool canTake = false;
    
    #endregion

    #region Triggers
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("isHere", true);
            canTake = true;
            StartCoroutine(Take());
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
    
    IEnumerator Take()
    {
        while (canTake || check)
        {
            if (Input.GetButtonDown("Interact"))
            {
                if (!check)
                {
                    Destroy(balise);
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
        textZoneRef.text = text;
        imageButtonRef.DOFade(1, interactTextFadeDuration);
        textZoneRef.DOFade(1, interactTextFadeDuration); 
        deathManagerRef.respawnPosition = transform.position;
        check = true;
    }

    void EndText()
    {
        textZoneRef.DOFade(0, timeToDespawn);
        imageButtonRef.DOFade(0, timeToDespawn);
        check = false;
    }
}
