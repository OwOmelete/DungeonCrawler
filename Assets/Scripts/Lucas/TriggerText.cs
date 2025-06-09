using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TriggerText : MonoBehaviour
{
    [SerializeField] private TMP_Text textZoneRef;
    [SerializeField] private Image imageButtonRef;
    [SerializeField] private string text;
    [SerializeField] private float fadeDuration = 0.3f;
    private bool check = true;

    void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(GetComponent<Collider2D>());
        check = true;
        textZoneRef.text = text;
        imageButtonRef.DOFade(1, fadeDuration);
        textZoneRef.DOFade(1, fadeDuration);
        StartCoroutine(Close());
    }

    IEnumerator Close()
    {
        while (check)
        {
            if (Input.GetButtonDown("Interact"))
            {
                EndText();
            }
            yield return null;
        }
    }

    void EndText()
    {
        textZoneRef.DOFade(0, fadeDuration);
        imageButtonRef.DOFade(0, fadeDuration);
        check = false;
        Destroy(gameObject);
    }
}
