using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeRessourcesManager : MonoBehaviour
{
    [SerializeField] private GameObject upgradeMenu;
    [SerializeField] private GameObject firstButton;
    [SerializeField] private LightManager lightManagerReference;
    [SerializeField] private OxygenManager oxygenManagerReference;
    [SerializeField] private float ressourcesMultiplier = 1.05f;
    [SerializeField] private Image menuBackground;
    [SerializeField] private Image button1;
    [SerializeField] private Image button2;
    [SerializeField] private AudioSource upgradeAudio;
    
    public void AddRessources()
    {
        upgradeMenu.SetActive(true);
        menuBackground.DOFade(1, 0.5f);
        button1.DOFade(1, 0.5f);
        button2.DOFade(1, 0.5f);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);
        StartCoroutine(StartMenu());
    }

    public void UpgradeLight()
    {
        lightManagerReference.maxLight *= ressourcesMultiplier;
        lightManagerReference.AddLight(lightManagerReference.maxLight);
        Time.timeScale = 1;
        StartCoroutine(CloseMenu());
    }

    public void UpgradeOxygen()
    {
        oxygenManagerReference.maxOxygen *= ressourcesMultiplier;
        oxygenManagerReference.AddOxygen(oxygenManagerReference.maxOxygen);
        Time.timeScale = 1;
        StartCoroutine(CloseMenu());
    }

    IEnumerator StartMenu()
    {
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0;
    }

    IEnumerator CloseMenu()
    {
        upgradeAudio.Play();
        menuBackground.DOFade(0, 0.5f);
        button1.DOFade(0, 0.5f);
        button2.DOFade(0, 0.5f);
        yield return new WaitForSeconds(0.5f);
        upgradeMenu.SetActive(false);
    }
}
