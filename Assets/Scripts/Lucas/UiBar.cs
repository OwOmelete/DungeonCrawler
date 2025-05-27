using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UiBar : MonoBehaviour
{
    [SerializeField] private float updateFrequency = 1f;
    [SerializeField] private Player player;
    [SerializeField] private LightManager lightManager;
    [SerializeField] private OxygenManager oxygenManager;
    [SerializeField] private Image imageOxygen;
    [SerializeField] private Image imageLight;
    [SerializeField] private Image imageHeal;
    [SerializeField] private float lerpDuration = 1f;

    private void Start()
    {
        StartCoroutine(UpdateUi());
    }

    IEnumerator UpdateUi()
    {
        while (true)
        {
            DOTween.To(() => imageOxygen.fillAmount, x => 
                imageOxygen.fillAmount = x, player.player.oxygen / oxygenManager.maxOxygen, lerpDuration);
            DOTween.To(() => imageLight.fillAmount, x => 
                imageLight.fillAmount = x, player.player.light / lightManager.maxLight, lerpDuration);
            DOTween.To(() => imageHeal.fillAmount, x => 
                imageHeal.fillAmount = x, (float)player.player.hp / 4, lerpDuration);
            yield return new WaitForSeconds(updateFrequency);
        }
    }
    
}
