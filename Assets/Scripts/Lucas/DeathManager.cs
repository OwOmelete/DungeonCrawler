using System;
using System.Collections;
using System.Net.Mime;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DeathManager : MonoBehaviour
{
    private bool isAlive = true;
    [SerializeField] Player player;
    [SerializeField] private GameObject playerGO;
    public Vector3 respawnPosition;
    [SerializeField] private Image imageDeath;
    [SerializeField] private float maxOxygen = 100;
    [SerializeField] private float maxLight = 10;
    [SerializeField] private int maxHp = 10;
    [SerializeField] private OxygenManager oxygenRef;
    [SerializeField] private LightManager lightRef;
    private void Start()
    {
        StartCoroutine(WaitFrame());
    }

    IEnumerator WaitFrame()
    {
        yield return new WaitForEndOfFrame();
        StartCoroutine(CheckLife());
    }

    IEnumerator CheckLife()
    {
        while (isAlive)
        {
            if (player.player.hp <= 0 || player.player.oxygen <= 0)
            {
                isAlive = false;
                Death();
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
    public void Death()
    {
        imageDeath.DOFade(1, 0.01f);
        imageDeath.gameObject.SetActive(true);
        playerGO.GetComponent<Player>().rotationReference.canMove = false;
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2f);
        RespawnPlayer();
        imageDeath.DOFade(0, 0.3f);
        yield return new WaitForSeconds(0.3f);
        imageDeath.gameObject.SetActive(false);
    }

    private void RespawnPlayer()
    {
        playerGO.transform.position = respawnPosition;
        playerGO.GetComponent<Player>().rotationReference.canMove = true;
        player.player.hp = maxHp;
        oxygenRef.AddOxygen(maxOxygen);
        lightRef.AddLight(maxLight);
        isAlive = true;
        StartCoroutine(CheckLife());
    }
    
}
