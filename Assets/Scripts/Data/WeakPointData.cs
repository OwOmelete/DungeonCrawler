using UnityEngine;

[CreateAssetMenu(fileName = "WeakPointData", menuName = "Scriptable Objects/WeakPointData")]
public class WeakPointData : ScriptableObject
{
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

    public int damageMulti;
}
