using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using DepthOfField = UnityEngine.Rendering.Universal.DepthOfField;

public class OxygenManager : MonoBehaviour
{
    [SerializeField] private float maxBlur = 26f;
    [SerializeField] private UiBar OxygenBar;
    public float maxOxygen = 100;
    [SerializeField] private float oxygenLoss = 1; 
    public float oxygenLossInterval = 1f;
    [SerializeField] private Volume globalVolume;
    [HideInInspector] public bool canLooseOxygen = true; 
    [HideInInspector] public PlayerDataInstance player;
    private DepthOfField depthOfField;
    void Start()
    {
        player.oxygen = maxOxygen;
        RestartCoroutine();
        if (globalVolume.profile.TryGet(out depthOfField)){}
        else
        {
            Debug.LogError("Aucun Depth Of Field dans le volume");
        }
    }

    public void RestartCoroutine()
    {
        StartCoroutine(OxygenLossRoutine()); 
    }
    private IEnumerator OxygenLossRoutine()
    {
        
        while (player.oxygen > 0 && canLooseOxygen)
        {
            yield return new WaitForSeconds(oxygenLossInterval);
            player.oxygen -= oxygenLoss;
            player.oxygen = Mathf.Clamp(player.oxygen, 0, 100);
            UpdateUi();
        }
    }
    public void AddOxygen(float value)
    {
        if (player.oxygen <= 0)
        {
            canLooseOxygen = true;
            player.oxygen += value;
            player.oxygen = Mathf.Clamp(player.oxygen, 0, maxOxygen);
            UpdateUi();
            StartCoroutine(OxygenLossRoutine());
        }
        else
        {
            canLooseOxygen = true;
            player.oxygen += value;
            player.oxygen = Mathf.Clamp(player.oxygen, 0, maxOxygen);
            UpdateUi();
        }
    }
    
    public void UpdateUi()
    {
        /*
        float quarterMaxOxygen = maxOxygen / 4;
        
        if (player.oxygen <= quarterMaxOxygen * 3 && player.oxygen > quarterMaxOxygen * 2 )
        {
            OxygenBar.ChangeSprite(1);
        }
        else if (player.oxygen <= quarterMaxOxygen * 2 && player.oxygen > quarterMaxOxygen)
        {
            OxygenBar.ChangeSprite(2);
        }
        else if (player.oxygen <= quarterMaxOxygen && player.oxygen > 0)
        {
            OxygenBar.ChangeSprite(3);
        }
        else if (player.oxygen <= 0)
        {
            OxygenBar.ChangeSprite(4);
        }
        else
        {
            OxygenBar.ChangeSprite(0);
        }
        */
        
        if (player.oxygen <= maxOxygen / 5)
        {
            DOTween.To(() => depthOfField.focalLength.value, x => 
                depthOfField.focalLength.value = x, maxBlur , 4);
        }
        if (player.oxygen > maxOxygen / 5)
        {
            DOTween.To(() => depthOfField.focalLength.value, x => 
                depthOfField.focalLength.value = x, 1 , 4);
        }
    }
    
}
