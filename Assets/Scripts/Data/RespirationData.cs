using UnityEngine;

[CreateAssetMenu(fileName = "RespirationData", menuName = "Scriptable Objects/RespirationData")]
public class RespirationData : ScriptableObject
{
    public float damageMultiplier;
    public float oxygenLoss;
    public int actionPoints;

}
