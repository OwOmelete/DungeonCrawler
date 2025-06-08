using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClipsTab;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float fadeDuration = 0.5f;

    private void Awake()
    {
        DontDestroyOnLoad(audioSource.gameObject);
    }

    public void SwitchToCombat()
    {
        audioSource.DOFade(0, fadeDuration);
        StartCoroutine(WaitForFade(1, 1f));
    }
    public void SwitchToExplo()
    {
        audioSource.DOFade(0, fadeDuration);
        StartCoroutine(WaitForFade(0, 1f ));
    }

    public void ThirdBiome()
    {
        audioSource.DOFade(0, fadeDuration);
        StartCoroutine(WaitForFade(2, 0.4f));
    }

    IEnumerator WaitForFade(int index, float endValue)
    {
        yield return new WaitForSeconds(fadeDuration);
        audioSource.clip = audioClipsTab[index];
        audioSource.Play();
        audioSource.DOFade(endValue, fadeDuration);
    }

    public void FadeOff(float fade)
    {
        audioSource.DOFade(0, fade);
    }
}
