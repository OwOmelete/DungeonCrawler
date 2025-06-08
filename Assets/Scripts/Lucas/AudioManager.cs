using System.Collections;
using DG.Tweening;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClipsTab;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float fadeDuration = 0.5f;

    public void SwitchToCombat()
    {
        audioSource.DOFade(0, fadeDuration);
        StartCoroutine(WaitForFade(1));
    }
    public void SwitchToExplo()
    {
        audioSource.DOFade(0, fadeDuration);
        StartCoroutine(WaitForFade(0));
    }

    IEnumerator WaitForFade(int index)
    {
        yield return new WaitForSeconds(fadeDuration);
        audioSource.clip = audioClipsTab[index];
        audioSource.Play();
        audioSource.DOFade(1, fadeDuration);
    }

    public void FadeOff(float fade)
    {
        audioSource.DOFade(0, fade);
    }
}
