using System;
using System.Collections;
using UnityEngine;

public class DeathManager : MonoBehaviour
{
    private bool isAlive = true;
    [SerializeField] private PlayerData _playerData;
    public PlayerDataInstance player;
    [SerializeField] private GameObject playerGO;
    [SerializeField] private Vector3 respawnPosition;
    [SerializeField] private GameObject canvasDeath;
    [SerializeField] private float maxOxygen;
    [SerializeField] private float maxLight;
    private void Start()
    {
        player = (PlayerDataInstance)_playerData.Instance();
        StartCoroutine(CheckLife());
    }

    IEnumerator CheckLife()
    {
        while (isAlive)
        {
            if (player.hp <= 0)
            {
                isAlive = false;
                Death();
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
    private void Death()
    {
        canvasDeath.SetActive(true);
        playerGO.GetComponent<Player>().rotationReference.canMove = false;
    }

    public void RespawnPlayer()
    {
        playerGO.transform.position = respawnPosition;
        playerGO.GetComponent<Player>().rotationReference.canMove = true;
        canvasDeath.SetActive(false);
        player.oxygen = maxOxygen;
        player.light = maxLight;
        isAlive = true;
        StartCoroutine(CheckLife());
    }
}
