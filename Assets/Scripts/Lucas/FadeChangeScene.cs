using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeChangeScene : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float timeToFade = 1f;
    [SerializeField] private AudioManager audioManagerRef;
    [SerializeField] private bool wantToFadeOff = true;
    void Start()
    {
        fadeImage.DOFade(0f, timeToFade);
        StartCoroutine(WaitForFade());
    }
    IEnumerator WaitForFade()
    {
        yield return new WaitForSeconds(timeToFade);
        fadeImage.gameObject.SetActive(false);
    }

    public void ChangeScene(int index)
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.DOFade(1f,timeToFade);
        if (wantToFadeOff)
        {
            audioManagerRef.FadeOff(timeToFade);
        }
        StartCoroutine(WaitForChangeScene(index));
    }

    IEnumerator WaitForChangeScene(int index)
    {
        yield return new WaitForSeconds(timeToFade);
        SceneManager.LoadSceneAsync(index);
    }

}
