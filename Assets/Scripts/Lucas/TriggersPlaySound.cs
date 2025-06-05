using System;
using UnityEngine;

public class TriggersPlaySound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    private void OnTriggerEnter2D(Collider2D other)
    {
        audioSource.Play();
    }
}
