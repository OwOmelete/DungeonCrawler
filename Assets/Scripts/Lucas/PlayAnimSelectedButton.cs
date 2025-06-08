using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayAnimSelectedButton : MonoBehaviour
{
    [SerializeField] private Sprite selectedSprite;
    [SerializeField] private Sprite unselectedSprite;
    [SerializeField] private Image spriteRendererRef;
    [SerializeField] private AudioSource audioSourceRef;
    private Coroutine checkCoroutine;
    private bool isAlreadyHere;
    void OnEnable()
    {
        if (checkCoroutine == null)
        {
            checkCoroutine = StartCoroutine(CheckIfSelected());
        }
        
    }

    IEnumerator CheckIfSelected()
    {
        while (true)
        {
            if (EventSystem.current.currentSelectedGameObject == this.gameObject && !isAlreadyHere)
            {
                audioSourceRef.Play();
                spriteRendererRef.sprite = selectedSprite;
                isAlreadyHere = true;
            }
            else if ( EventSystem.current.currentSelectedGameObject != this.gameObject)
            {
                spriteRendererRef.sprite = unselectedSprite;
                isAlreadyHere = false;
            }
            yield return null;
        }
    }

    void OnDisable()
    {
        StopCoroutine(checkCoroutine);
        checkCoroutine = null;
    }
}
