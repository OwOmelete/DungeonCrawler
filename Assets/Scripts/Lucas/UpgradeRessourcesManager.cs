using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeRessourcesManager : MonoBehaviour
{
    [SerializeField] private GameObject upgradeMenu;
    [SerializeField] private GameObject firstButton;
    [SerializeField] private LightManager lightManagerReference;
    [SerializeField] private OxygenManager oxygenManagerReference;
    [SerializeField] private float timeMultiplier = 1.5f;
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
            lightManagerReference.looseLightDelay *= timeMultiplier;
            canUpLight = false;
            upgradeMenu.SetActive(false);
            playerRef.canMove = true;
        }
        
    }

    public void UpgradeOxygen()
    {

        if (canUpOxygen)
        {
            oxygenManagerReference.oxygenLossInterval *= timeMultiplier;
            canUpOxygen = false;
            upgradeMenu.SetActive(false);
            playerRef.canMove = true;
        }
    }
}
