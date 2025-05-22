using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeRessourcesManager : MonoBehaviour
{
    [SerializeField] private GameObject upgradeMenu;
    [SerializeField] private GameObject firstButton;
    [SerializeField] private LightManager lightManagerReference;
    [SerializeField] private OxygenManager oxygenManagerReference;
    [SerializeField] private float ressourcesMultiplier = 1.05f;
    [SerializeField] private Player playerRef;

    private bool canUpLight = true;
    private bool canUpOxygen = true;
    
    public void AddRessources()
    {
        playerRef.canMove = false;
        upgradeMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    public void UpgradeLight()
    {
        if (canUpLight)
        {
            lightManagerReference.maxLight *= ressourcesMultiplier;
            lightManagerReference.AddLight(ressourcesMultiplier - 1 * 10);
            canUpLight = false;
            upgradeMenu.SetActive(false);
            playerRef.canMove = true;
        }
        
    }

    public void UpgradeOxygen()
    {

        if (canUpOxygen)
        {
            oxygenManagerReference.maxOxygen *= ressourcesMultiplier;
            oxygenManagerReference.AddOxygen(ressourcesMultiplier - 1 * 100);
            canUpOxygen = false;
            upgradeMenu.SetActive(false);
            playerRef.canMove = true;
        }
    }
}
