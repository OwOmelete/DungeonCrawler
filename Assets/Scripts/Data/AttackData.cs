using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "Scriptable Objects/AttackData")]
public class AttackData : ScriptableObject
{
    
    [Header("Pour nos merveilleux gds")]
    public float critChance;
    public float critMultiplier;
    public int Damage;
    public int range;
    
    [Header("Pas touche >:(")]
    public int Yoffset;
    public int Xoffset;
    public int actionCost;
    
    public float precision;
}