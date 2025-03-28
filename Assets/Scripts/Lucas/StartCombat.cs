using System;
using System.Collections.Generic;
using UnityEngine;

public class StartCombat : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject combatScene;
    [SerializeField] private CombatManager combatManagerReference;

    public void SwitchToCombat(List<FishData> fishDatas)
    {
        player.SetActive(false);
        combatScene.SetActive(true);
        combatManagerReference._fishDatas.Clear();
        combatManagerReference._fishDatas = fishDatas;
        Player playerScript = player.GetComponent<Player>();
        combatManagerReference.player = playerScript.player;
        playerScript.lightManager.canLooseLight = false;
        playerScript.oxygenManager.canLooseOxygen = false;
        combatManagerReference.InitCombat();
    }
}
