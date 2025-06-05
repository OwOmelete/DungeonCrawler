using System.Collections;
using UnityEngine;

public class RandomSoundPlay : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float minimumTime = 20f;
    [SerializeField] private float maximumTime = 40f;

    private void Start()
    {
        StartCoroutine(RandomPlayTime());
    }

    IEnumerator RandomPlayTime()
    {
        yield return new WaitForSeconds(Random.Range(minimumTime, maximumTime));
    }
}
