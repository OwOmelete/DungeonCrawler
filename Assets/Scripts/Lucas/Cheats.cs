using UnityEngine;

public class Cheats : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private OxygenManager oxygenManagerRef;
    [SerializeField] private LightManager lightManagerRef;

    public void Tp(Vector3 tpPosition)
    {
        playerTransform.position = tpPosition;
    }

    public void EndFight()
    {
        CombatManager.Instance.EndFight();
    }

    public void FullRessources()
    {
           oxygenManagerRef.AddOxygen(oxygenManagerRef.maxOxygen);
           lightManagerRef.AddLight(lightManagerRef.maxLight);
    }
}
