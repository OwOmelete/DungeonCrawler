using System;
using UnityEngine;

public class PlaySoundOnDisable : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    private void OnDisable()
    {
        audioSource.Play();
    }
}
