using UnityEngine;

public class StopRessourcesThirdBiome : MonoBehaviour
{
    [SerializeField] private LightManager lightManagerRef;
    [SerializeField] private OxygenManager oxygenManagerRef;
    [SerializeField] private AudioManager audioManagerRef;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            oxygenManagerRef.StopLooseOxygen();
            lightManagerRef.StopLooseLight();
            audioManagerRef.ThirdBiome();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            oxygenManagerRef.AddOxygen(0f);
            lightManagerRef.AddLight(0f);
            audioManagerRef.SwitchToExplo();
        }
    }
}
