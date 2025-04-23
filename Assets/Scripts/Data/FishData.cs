using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "FishData", menuName = "Scriptable Objects/Entity/FishData")]
public class FishData : Entity
{
    public string name;
    public AttackData[] attacks;
    public WeakPointData[] WeakPointsRight;
    public WeakPointData[] WeakPointsLeft;
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
    public string name;
    public bool Flipped = false;
    public bool PreparingAttack = false;
    public bool HasAttacked = false;
    public bool FirstCycle = true;
    public SpriteRenderer sr;
    public AttackData[] attacks;
    public WeakPointData[] WeakPointsRight;
    public WeakPointData[] WeakPointsLeft;
    public FishDataInstance(FishData data) : base(data)
    {
        name = data.name;
        attacks = data.attacks;
        WeakPointsLeft = data.WeakPointsLeft;
        WeakPointsRight = data.WeakPointsLeft;
    }
}