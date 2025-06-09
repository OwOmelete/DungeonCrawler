using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TriggerText : MonoBehaviour
{
    [SerializeField] private TMP_Text textZoneRef;
    [SerializeField] private Image imageButtonRef;
    [SerializeField] private string[] texts;
    [SerializeField] private float fadeDuration = 0.3f;
    [SerializeField] private Image bgRef;
    [SerializeField] private bool isLast;
    [SerializeField] private EndLevelPlant endLevel;
    private bool check = true;
    private int lineCount = 0;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(GetComponent<Collider2D>());
            if (InteractTextManager.INSTANCE.actualTrigger != null)
            
            {
                Destroy(InteractTextManager.INSTANCE.actualTrigger);
            }
            InteractTextManager.INSTANCE.actualTrigger = gameObject;
            check = true;
            textZoneRef.text = texts[0];
            bgRef.DOFade(1, fadeDuration);
            imageButtonRef.DOFade(1, fadeDuration);
            textZoneRef.DOFade(1, fadeDuration);
            StartCoroutine(Close());
        }
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

    IEnumerator Close()
    {
        while (check)
        {
            if (Input.GetButtonDown("Interact"))
            {
                lineCount++; 
                if (lineCount >= texts.Length)
                {
                    EndText();
                }
                else
                {
                    ChangeText(lineCount);
                }
                
            }
            yield return null;
        }
    }

    void EndText()
    {
        if (isLast)
        {
            endLevel.WaitForTextFalse();
        }
        check = false;
        StartCoroutine(CloseText());
    }
    
    
    IEnumerator CloseText()
    {
        bgRef.DOFade(0, fadeDuration);
        textZoneRef.DOFade(0, fadeDuration);
        imageButtonRef.DOFade(0, fadeDuration);
        yield return new WaitForSeconds(fadeDuration);
        InteractTextManager.INSTANCE.actualTrigger = null;
        Destroy(gameObject);
    }
}
