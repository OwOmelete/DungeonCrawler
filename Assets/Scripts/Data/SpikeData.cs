using UnityEngine;

[CreateAssetMenu(fileName = "FishData", menuName = "Scriptable Objects/Entity/Spike")]
public class SpikeData : Entity
{
    public override EntityInstance Instance()
    {
        return new SpikeInstance(this);
    }
}

public class SpikeInstance : EntityInstance
{
    public int dirX;
    public int dirY;
    public SpikeInstance(SpikeData data) : base(data)
    {
        
    }
}
