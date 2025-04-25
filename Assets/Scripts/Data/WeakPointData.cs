using UnityEngine;

[CreateAssetMenu(fileName = "WeakPointData", menuName = "Scriptable Objects/WeakPointData")]
public class WeakPointData : ScriptableObject
{
    [Header("Pour nos merveilleux gds")]
    public int damageMulti;
    
    
    [Header("Pas touche >:(")]
    public int posX;
    public int posY;
    public enum dir
    {
        any,
        up,
        down,
        left, 
        right
    }

    public dir direction;

    
}
