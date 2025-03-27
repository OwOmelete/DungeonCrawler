using System.Collections;
using UnityEngine;

public class OxygenManager : MonoBehaviour
{
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
        while (player.oxygen > 0 && canLooseOxygen)
        {
            yield return new WaitForSeconds(oxygenLossInterval);
            player.oxygen -= oxygenLoss;
            player.oxygen = Mathf.Clamp(player.oxygen, 0, 100);
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
