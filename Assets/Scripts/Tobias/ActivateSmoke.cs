using System;
using UnityEngine;

public class ActivateSmoke : MonoBehaviour
{

    [SerializeField] private Animator animator;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("playerDetected", true);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("playerDetected", false);
        }
    }
}
