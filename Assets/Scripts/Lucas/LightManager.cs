using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightManager : MonoBehaviour
{
    #region Variables
    
    [Header("References")]
    public Light2D playerLight;   // objet light du player pour changer son intensité
    [SerializeField] private UiBar LightBar;
    
    [Header("Values")]
    public float maxLight = 10;  // capacité maximale de lumière
    [SerializeField] public float minLight = 1;  // capacité minimale de lumière
    [SerializeField] float looseLightValue = 0.1f; // ce que va perdre la jauge de lumiere toutes les secondes
    [SerializeField] private float lerpDuration = 1f; // ce que va perdre la jauge de lumiere toutes les secondes
    public float looseLightDelay = 1f;
    [HideInInspector] public bool canLooseLight = true; // ici pour stopper la perte de lumière dans certains cas
    public bool haveLight = true; // verifie si il reste de la lumière

    [HideInInspector] public PlayerDataInstance player;
    #endregion
    
    private void Start()
    {
        
        player.light = maxLight;
        playerLight.pointLightOuterRadius = player.light;
        playerLight.pointLightInnerRadius = player.light / 2;
        StartCoroutine(LooseLight());
    }

    IEnumerator LooseLight()    // coroutine qui fait baisser peu a peu le niveau de lumiere
    {
        while (haveLight && canLooseLight)
        {
            player.light -= looseLightValue;
            DOTween.To(() => playerLight.pointLightOuterRadius, x => playerLight.pointLightOuterRadius = x, player.light, lerpDuration);
            DOTween.To(() => playerLight.pointLightInnerRadius, x => playerLight.pointLightInnerRadius = x, player.light / 2, lerpDuration);
            //UpdateUi();
            yield return new WaitForSeconds(looseLightDelay);
        }
    }
    /*
    public void UpdateUi()
    {
        float quarterMaxLight = (maxLight - minLight) / 4;
        if (player.light <= quarterMaxLight * 3 && player.light > quarterMaxLight * 2 )
        {
            LightBar.ChangeSprite(1);
        }
        else if (player.light <= quarterMaxLight * 2 && player.light > quarterMaxLight)
        {
            LightBar.ChangeSprite(2);
        }
        else if (player.light <= quarterMaxLight && player.light > minLight)
        {
            LightBar.ChangeSprite(3);
        }
        else if (player.light <= minLight)
        {
            LightBar.ChangeSprite(4);
            haveLight = false;
        }
        else
        {
            LightBar.ChangeSprite(0);
        }
    }
    */
    public void AddLight(float value)   // recharge la lumière
    {
        player.light += value;
        player.light = Mathf.Clamp(player.oxygen, minLight, maxLight);
        DOTween.To(() => playerLight.pointLightOuterRadius, x => 
            playerLight.pointLightOuterRadius = x, player.light, lerpDuration);
        DOTween.To(() => playerLight.pointLightInnerRadius, x => 
            playerLight.pointLightInnerRadius = x, player.light / 2, lerpDuration);
        if (!haveLight || !canLooseLight)
        {
            canLooseLight = true;
            haveLight = true;
            RestartCoroutine();
        }
    }

    public void RestartCoroutine()
    {
        StartCoroutine(LooseLight());
    }
}
