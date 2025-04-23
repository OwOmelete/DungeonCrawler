using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "FishData", menuName = "Scriptable Objects/Entity/FishData/GGData")]
public class GGData : Entity
{
    
    public override EntityInstance Instance()
    {
        return new GGDataInstance(this);
    }
}

public class GGDataInstance : EntityInstance
{
    
    public GGDataInstance(GGData data) : base(data)
    {
        
    }
}