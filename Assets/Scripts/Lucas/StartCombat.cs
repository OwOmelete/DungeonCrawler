using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class StartCombat : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject combatScene;
    [SerializeField] private CombatManager combatManagerReference;
    [SerializeField] private GameObject uiExplo;
    [SerializeField] private float animDuration = 2f;
    [SerializeField] private Image dangerImage;
    [SerializeField] private Image blackFade;
    [SerializeField] private float fadeDelay = 0.5f;

    public void SwitchToCombat(List<FishData> fishDatas)
    {
        dangerImage.DOFade(1, 1);
        blackFade.DOFade(1, 1).SetDelay(fadeDelay);
        StartCoroutine(WaitForAnim(fishDatas));
    }

    IEnumerator WaitForAnim(List<FishData> fishDatas)
    {
        yield return new WaitForSeconds(animDuration);
        dangerImage.DOFade(0, 1);
        blackFade.DOFade(0, 1);
        player.SetActive(false);
        combatScene.SetActive(true);
        //uiExplo.SetActive(false);
        combatManagerReference._fishDatas.Clear();
        combatManagerReference._fishDatas = fishDatas;
        Player playerScript = player.GetComponent<Player>();
        combatManagerReference.player = playerScript.player;
        playerScript.lightManager.canLooseLight = false;
        playerScript.oxygenManager.canLooseOxygen = false;
        combatManagerReference.InitCombat();
    }
}
