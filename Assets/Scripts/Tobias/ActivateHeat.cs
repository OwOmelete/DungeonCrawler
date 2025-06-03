using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class ActivateHeat : MonoBehaviour
{

    public Material heat;
    private float lastTimeChanged;
    public float delayChange=1f;
    [SerializeField] private float step;

    private void Start()
    {
        heat.SetFloat("_Activate", 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StopCoroutine(TransitionOut());
            StartCoroutine(TransitionIn());
        } 

    }  
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StopCoroutine(TransitionIn());
            StartCoroutine(TransitionOut());
        } 

    }
    
    IEnumerator TransitionOut()
    {
        while (heat.GetFloat("_Active") > 0)
        {
            heat.SetFloat("_Active", (heat.GetFloat("_Active") - step));
            yield return new WaitForSeconds(delayChange);
        }
    }

    IEnumerator TransitionIn()
    {
        while (heat.GetFloat("_Active") < 1)
        {
            heat.SetFloat("_Active", (heat.GetFloat("_Active") + step));
            yield return new WaitForSeconds(delayChange);
        }
    }
    
}
