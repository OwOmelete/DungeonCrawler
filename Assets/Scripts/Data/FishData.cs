using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "FishData", menuName = "Scriptable Objects/Entity/FishData")]
public class FishData : Entity
{
    public AttackData[] attacks;
    public WeakPointData[] WeakPointsRight;
    public WeakPointData[] WeakPointsLeft;
    public WeakPointData[] WeakPointsUp;
    public WeakPointData[] WeakPointsDown;
    //Position weaknesses
    //effects weaknesses( ? )
    //spawn zone (corail/kayou/carcasse)

    //IA behavior
    public override EntityInstance Instance()
    {
        return new FishDataInstance(this);
    }
}

public class FishDataInstance : EntityInstance
{
    public bool Flipped = false;
    public bool PreparingAttack = false;
    public bool HasAttacked = false;
    public bool FirstCycle = true;
    public SpriteRenderer sr;
    public AttackData[] attacks;
    public WeakPointData[] WeakPointsRight;
    public WeakPointData[] WeakPointsLeft;
    public WeakPointData[] WeakPointsUp;
    public WeakPointData[] WeakPointsDown;
    public FishDataInstance(FishData data) : base(data)
    {
        attacks = data.attacks;
        WeakPointsLeft = data.WeakPointsLeft;
        WeakPointsRight = data.WeakPointsRight;
        WeakPointsDown = data.WeakPointsDown;
        WeakPointsUp = data.WeakPointsUp;
    }
}