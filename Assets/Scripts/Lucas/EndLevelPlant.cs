using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class EndLevelPlant : MonoBehaviour
{
    #region Variables

    [Header("Reference")] 
    [SerializeField] private FadeChangeScene fadeSceneRef;
    [SerializeField] private GameObject interactDisplay;
    [SerializeField] private Animator animator;
    [SerializeField] private TMP_Text textZoneRef;
    [SerializeField] private Image imageButtonRef;
    [SerializeField] private Image bgRef;

    [Header("Values")]
    [SerializeField] private float interactTextFadeDuration = 0.2f;
    [SerializeField] private int sceneIndex;
    [SerializeField] private float fadeDuration = 0.3f;
    [SerializeField] private string[] texts;

    private bool waitForText = true;
    private bool canTake = false;
    private bool check = false;
    private int lineCount = 0;

    #endregion

    #region Triggers
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
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
            if (!waitForText && !check)
            {
                animator.SetBool("isHere", true);
                check = true;
            }
            else if (!waitForText)
            {
                if (Input.GetButtonDown("Interact"))
                {
                    StartText();
                }
            }
            yield return null;
        }
    }

    void StartText()
    {
        canTake = false;
        textZoneRef.text = texts[0];
        bgRef.DOFade(1, fadeDuration);
        imageButtonRef.DOFade(1, fadeDuration);
        textZoneRef.DOFade(1, fadeDuration);
        StartCoroutine(Close());
    }
    IEnumerator Close()
    {
        while (check)
        {
            if (Input.GetButtonDown("Interact"))
            {
                lineCount++; 
                if (lineCount >= texts.Length)
                {
                    ChangeScene();
                }
                else
                {
                    ChangeText(lineCount);
                }
                
            }
            yield return null;
        }
    }
    void ChangeScene()
    {
        fadeSceneRef.ChangeScene(sceneIndex);
    }

    public void WaitForTextFalse()
    {
        waitForText = false;
    }
    
    void ChangeText(int index)
    {
        textZoneRef.DOFade(0, fadeDuration);
        StartCoroutine(WaitForChangeText(index));
    }
    IEnumerator WaitForChangeText(int index)
    {
        yield return new WaitForSeconds(fadeDuration);
        textZoneRef.text = texts[index];
        textZoneRef.DOFade(1, fadeDuration);
    }
}
