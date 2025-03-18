using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "Scriptable Objects/AttackData")]
public class AttackData : ScriptableObject
{
    public int Damage;
    public int range;
    public int actionCost;
    public float critChance;
    public float critMultiplier;
}