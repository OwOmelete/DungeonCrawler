using System.Collections;
using UnityEngine;

public class OxygenManager : MonoBehaviour
{
    [SerializeField] private UiBar OxygenBar;
    [SerializeField] private int maxOxygen = 100;
    [SerializeField] private int oxygenLoss = 1; 
    [SerializeField] private float oxygenLossInterval = 2f;
    [HideInInspector] public bool canLooseOxygen = true; 
    [HideInInspector] public PlayerDataInstance player;
    void Start()
    {
        player.oxygen = maxOxygen;
        RestartCoroutine();
    }

    public void RestartCoroutine()
    {
        StartCoroutine(OxygenLossRoutine()); 
    }
    private IEnumerator OxygenLossRoutine()
    {
        float quarterMaxOxygen = maxOxygen / 4;
        while (player.oxygen > 0 && canLooseOxygen)
        {
            yield return new WaitForSeconds(oxygenLossInterval);
            player.oxygen -= oxygenLoss;
            
            player.oxygen = Mathf.Clamp(player.oxygen, 0, 100);
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
        }
    }
    public void AddOxygen(float value)
    {
        player.oxygen += value;
        player.oxygen = Mathf.Clamp(player.oxygen, 0, maxOxygen);
    }
    
    void LowOxygenEffects() // pour mettre un effet de flou quand on manque d'oxygene ou meme des effets en general
    {
        
    }
}
