using System.Collections;
using DG.Tweening;
using UnityEngine;


public class EndLevelPlant : MonoBehaviour
{
    #region Variables

    [Header("Reference")] 
    [SerializeField] private FadeChangeScene fadeSceneRef;
    [SerializeField] private GameObject interactDisplay;    // Texte d'affichage de la touche 
    [SerializeField] private Animator animator;

    [Header("Values")]
    [SerializeField] private float interactTextFadeDuration = 0.2f; // Temps que va prendre le texte a apparaitre et a disparaitre
    [SerializeField] private int sceneIndex;

    private bool canTake = false;   // Savoir si on peut prendre l'objet

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
        while (canTake)
        {
            if (Input.GetButtonDown("Interact"))
            {
                ChangeScene();
            }

            yield return null;
        }
    }

    void ChangeScene()
    {
        fadeSceneRef.ChangeScene(sceneIndex);
    }
}
