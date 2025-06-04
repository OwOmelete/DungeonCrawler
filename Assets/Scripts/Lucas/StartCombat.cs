using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class StartCombat : MonoBehaviour
{
    public int playerPosCombatX = 0; 
    public int playerPosCombatY = 0;
    
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject combatScene;
    [SerializeField] private CombatManager combatManagerReference;
    [SerializeField] private GameObject healthBarExplo;
    [SerializeField] private RectTransform lightBar;
    [SerializeField] private RectTransform oxygenBar;
    [SerializeField] private Vector3 newLightBarPos;
    [SerializeField] private Vector3 newOxygenBarPos;
    [SerializeField] private float animDuration = 2f;
    [SerializeField] private Image dangerImage;
    [SerializeField] private Image blackFade;
    [SerializeField] private float fadeDelay = 0.5f;
    [SerializeField] private AudioManager audioManager;

    public void SwitchToCombat(List<FishData> fishDatas)
    {
        dangerImage.DOFade(1, 1);
        blackFade.DOFade(1, 1).SetDelay(fadeDelay);
        audioManager.SwitchToCombat();
        StartCoroutine(WaitForAnim(fishDatas));
    }

    IEnumerator WaitForAnim(List<FishData> fishDatas)
    {
        yield return new WaitForSeconds(animDuration);
        dangerImage.DOFade(0, 0.8f);
        blackFade.DOFade(0, 1f);
        player.SetActive(false);
        combatScene.SetActive(true);
        healthBarExplo.SetActive(false);
        lightBar.localPosition = newLightBarPos;
        Debug.Log(newLightBarPos);
        oxygenBar.localPosition = newOxygenBarPos;
        //combatManagerReference._fishDatas.Clear();
        combatManagerReference._fishDatas = fishDatas;
        Player playerScript = player.GetComponent<Player>();
        playerScript.player.positionX = playerPosCombatX;
        playerScript.player.positionY = playerPosCombatY;
        combatManagerReference.player = playerScript.player;
        playerScript.lightManager.canLooseLight = false;
        playerScript.oxygenManager.canLooseOxygen = false;
        combatManagerReference.InitCombat();
    }
}
