using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayAnimSelectedButton : MonoBehaviour
{
    [SerializeField] private Animator animatorRef;
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
                isAlreadyHere = true;
                animatorRef.SetBool("isSelected", true);
            }
            else if ( EventSystem.current.currentSelectedGameObject != this.gameObject)
            {
                isAlreadyHere = false;
                animatorRef.SetBool("isSelected",false);
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
