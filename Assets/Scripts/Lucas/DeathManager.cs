using System;
using System.Collections;
using UnityEngine;

public class DeathManager : MonoBehaviour
{
    private bool isAlive = true;
    [SerializeField] Player player;
    [SerializeField] private GameObject playerGO;
    public Vector3 respawnPosition;
    [SerializeField] private GameObject canvasDeath;
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
        canvasDeath.SetActive(true);
        playerGO.GetComponent<Player>().rotationReference.canMove = false;
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2f);
        RespawnPlayer();
    }

    private void RespawnPlayer()
    {
        playerGO.transform.position = respawnPosition;
        playerGO.GetComponent<Player>().rotationReference.canMove = true;
        canvasDeath.SetActive(false);
        player.player.hp = maxHp;
        oxygenRef.AddOxygen(maxOxygen);
        lightRef.AddLight(maxLight);
        canvasDeath.SetActive(false);
        isAlive = true;
        StartCoroutine(CheckLife());
    }
}
