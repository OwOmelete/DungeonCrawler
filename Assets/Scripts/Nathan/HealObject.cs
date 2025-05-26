using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealObject : MonoBehaviour
{
    #region Variables
    
    [Header("Reference")]
    [SerializeField] private GameObject interactDisplay;    // Texte d'affichage de la touche
    [SerializeField] private Animator animator;
    
    [Header("Values")]
    [SerializeField] private int regen = 5;   // Quantitée de lumière que va regenerer l'objet
    [SerializeField] private float interactTextFadeDuration = 0.2f; // Temps que va prendre le texte a apparaitre et a disparaitre
    [SerializeField] private float timeToDespawn = 0.3f;
    
    private bool canTake = false;   // Savoir si on peut prendre l'objet
    [SerializeField] private Player playerRef;
    [HideInInspector] public PlayerDataInstance player;
    
    #endregion

    #region Triggers

    private void Start()
    {
        player = playerRef.player;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("isHere", true);
            canTake = true;
            StartCoroutine(TakeHeal());
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
    
    IEnumerator TakeHeal()
    {
        while (canTake)
        {
            if (Input.GetButtonDown("Interact"))
            {
                Heal();
            }
            yield return null;
        }
    }
    
    void Heal()
    {
        Destroy(GetComponent<Collider2D>());
        gameObject.GetComponent<Transform>().DOScale(Vector3.zero, timeToDespawn).SetEase(Ease.OutCubic);
        if (player.hp + regen >= playerRef._playerData.hp)
        {
            player.hp = playerRef._playerData.hp;
        }
        else
        {
            player.hp += regen;
        }
        StartCoroutine(DespawnCoroutine());
    }
    IEnumerator DespawnCoroutine()
    {
        yield return new WaitForSeconds(timeToDespawn);
        Destroy(gameObject);
    }
}
