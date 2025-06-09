using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EndFight : MonoBehaviour
{
    [SerializeField] private Image endFightImg;
    [SerializeField] private Image blackFade;
    
    public void lastEnnemyDead()
    {
        StartCoroutine(endOfFight());
    }

    IEnumerator endOfFight()
    {
        //shader dissolution
        yield return new WaitForSeconds(1);
        endFightImg.DOFade(1, 1);
        blackFade.DOFade(1, 1).SetDelay(0.5f);
        yield return new WaitForSeconds(2);
        CombatManager.Instance.EndFight();
        CombatManager.Instance.deathManagerReference.Death();
        endFightImg.DOFade(0, 0.8f);
        blackFade.DOFade(0, 1f);
        //z.SetFloat("_DissolveProgression", 1);
    }

    
}