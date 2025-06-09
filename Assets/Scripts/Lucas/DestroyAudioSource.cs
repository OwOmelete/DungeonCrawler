using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyAudioSource : MonoBehaviour
{
    private bool check = false;
    void Update()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneAt(4) && !check)
        {
            check = true;
            gameObject.GetComponent<AudioSource>().DOFade(0f, 0.8f);
            StartCoroutine(FadeOff());
        }
    }

    IEnumerator FadeOff()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
