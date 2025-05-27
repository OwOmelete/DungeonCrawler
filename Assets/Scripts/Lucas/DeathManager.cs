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
    
    [SerializeField] private Transform lightParent;
    [SerializeField] private Transform oxygenParent;
    [SerializeField] private Transform healParent;
    
    private GameObject[] enemyTab;
    private GameObject[] lightTab;
    private GameObject[] oxygenTab;
    private GameObject[] healTab;
    private void Start()
    {
        enemyTab = new GameObject[enemyParent.childCount];
        for (int i = 0; i < enemyParent.childCount; i++)
        {
            enemyTab[i] = enemyParent.GetChild(i).gameObject;
        }
        lightTab = new GameObject[lightParent.childCount];
        for (int i = 0; i < lightParent.childCount; i++)
        {
            lightTab[i] = lightParent.GetChild(i).gameObject;
        }
        oxygenTab = new GameObject[oxygenParent.childCount];
        for (int i = 0; i < oxygenParent.childCount; i++)
        {
            oxygenTab[i] = oxygenParent.GetChild(i).gameObject;
        }
        healTab = new GameObject[healParent.childCount];
        for (int i = 0; i < healParent.childCount; i++)
        {
            healTab[i] = healParent.GetChild(i).gameObject;
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
        for (int i = 1; i < enemyTab.Length; i++)
        {
            enemyTab[i].SetActive(true);
            enemyTab[i].GetComponent<IaFishExplo>().RestartCoroutine();
        }

        for (int i = 0; i < lightTab.Length; i++)
        {
            lightTab[i].SetActive(true);         
        }
        for (int i = 0; i < healTab.Length; i++)
        {
            healTab[i].SetActive(true);         
        }
        for (int i = 0; i < oxygenTab.Length; i++)
        {
            oxygenTab[i].SetActive(true);         
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
