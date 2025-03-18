using System;
using System.Collections.Generic;
using UnityEngine;

public class StartCombat : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject combatScene;
    [SerializeField] private CombatManager combatManagerReference;
    [SerializeField] private LightManager lightManagerReference;
    [SerializeField] private GameObject enemyFollow;
    [SerializeField] List<FishData> fishDatas;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SwitchToCombat();
        }
    }

    void SwitchToCombat()
    {
        player.SetActive(false);
        combatScene.SetActive(true);
        combatManagerReference._fishDatas.Clear();
        combatManagerReference._fishDatas = fishDatas;
        combatManagerReference.player = player.GetComponent<Player>().player;
        lightManagerReference.canLooseLight = false;
        combatManagerReference.InitCombat();
        Destroy(enemyFollow);
    }
}
