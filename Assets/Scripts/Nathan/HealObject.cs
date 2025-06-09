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
    [SerializeField] private GameObject interactDisplay;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject triggerTextRef;
    
    [Header("Values")]
    [SerializeField] private int regen = 5;
    [SerializeField] private float interactTextFadeDuration = 0.2f;
    
    private bool canTake = false;
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
        if (player.hp + regen >= playerRef._playerData.hp)
        {
            player.hp = playerRef._playerData.hp;
        }
        else
        {
            player.hp += regen;
        }
        if (InteractTextManager.INSTANCE.firstPlant)
        {
            InteractTextManager.INSTANCE.firstPlant = false;
            triggerTextRef.SetActive(true);
        }
        gameObject.SetActive(false);
    }
    
}
