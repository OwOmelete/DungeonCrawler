using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Heal : MonoBehaviour
{
    #region Variables
    
    [Header("Reference")]
    [SerializeField] private HealthManager HealthManagerReference;    // Reference au light manager
    [SerializeField] private GameObject interactDisplay;    // Texte d'affichage de la touche 
    
    [Header("Values")]
    [SerializeField] private int regen = 5;   // Quantitée de lumière que va regenerer l'objet
    [SerializeField] private float interactTextFadeDuration = 0.2f; // Temps que va prendre le texte a apparaitre et a disparaitre
    [SerializeField] private float timeToDespawn = 0.3f;
    
    private bool canTake = false;   // Savoir si on peut prendre l'objet
    
    #endregion

    #region Triggers
    private void OnTriggerEnter2D(Collider2D other)
    {
        interactDisplay.GetComponent<SpriteRenderer>().DOFade(1f, interactTextFadeDuration);
        canTake = true;
        StartCoroutine(TakeHeal());
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        interactDisplay.GetComponent<SpriteRenderer>().DOFade(0f, interactTextFadeDuration);
        canTake = false;
    }
    #endregion
    
    IEnumerator TakeHeal()
    {
        while (canTake)
        {
            if (Input.GetButtonDown("Interact"))
            {
                HealPlayer();
            }
            yield return null;
        }
    }
    
    void HealPlayer()
    {
        Destroy(GetComponent<Collider2D>());
        gameObject.GetComponent<Transform>().DOScale(Vector3.zero, timeToDespawn).SetEase(Ease.OutCubic);
        if (HealthManagerReference.player.hp + regen >= HealthManagerReference.maxHealth)
        {
            regen = HealthManagerReference.maxHealth - HealthManagerReference.player.hp;
        }
        HealthManagerReference.Heal(regen);
        StartCoroutine(DespawnCoroutine());
    }
    IEnumerator DespawnCoroutine()
    {
        yield return new WaitForSeconds(timeToDespawn);
        Destroy(gameObject);
    }
}
