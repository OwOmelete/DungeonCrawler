using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{

    private Animator animator;

    void Start()
    {
        animator = GetComponent<animator>();
        animator.SetBool("playerDetected", false);
    }

    private void OnTriggerEnter2D()
    {
       if(other.CompareTag("Player"))
        {
            Debug.Log("Joueur détecté!");
            animator.setBool("playerDetected",  true);
        }
    }

}
