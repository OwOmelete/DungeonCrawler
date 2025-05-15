using System;
using System.Collections;
using System.Net.Mime;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class DeathManager : MonoBehaviour
{
    private bool isAlive = true;
    [SerializeField] Player player;
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private GameObject playerGO;
    public Vector3 respawnPosition;
    [SerializeField] private Image imageDeath;
    [SerializeField] private float maxOxygen = 100;
    [SerializeField] private float maxLight = 10;
    [SerializeField] private int maxHp = 10;
    [SerializeField] private OxygenManager oxygenRef;
    [SerializeField] private LightManager lightRef;
    [SerializeField] private Transform enemyParent;
    [SerializeField] private GameObject combatScene;
    private GameObject[] enemyTab;
    private void Start()
    {
        enemyTab = new GameObject[enemyParent.childCount];
        for (int i = 0; i < enemyParent.childCount; i++)
        {
            enemyTab[i] = enemyParent.GetChild(i).gameObject;
        }
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
                if (combatScene.activeSelf)
                {
                    CombatManager.Instance.EndFight();
                }
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
        playerGO.SetActive(false);
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
        for (int i = 0; i < enemyTab.Length; i++)
        {
            enemyTab[i].SetActive(true);
        }
        playerGO.SetActive(true);
        playerGO.transform.position = respawnPosition;
        playerGO.GetComponent<Player>().rotationReference.canMove = true;
        player.player.hp = _playerData.hp;
        oxygenRef.AddOxygen(maxOxygen);
        lightRef.AddLight(maxLight);
        isAlive = true;
        for (int i = 0; i < enemyTab.Length; i++)
        {
            enemyTab[i].SetActive(true);
        }
        StartCoroutine(CheckLife());
    }
    
}
