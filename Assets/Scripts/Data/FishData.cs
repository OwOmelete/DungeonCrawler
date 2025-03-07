using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "FishData", menuName = "Scriptable Objects/Entity/Fish")]
public class FishData : Entity
{
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
    public FishDataInstance(FishData data) : base(data)
    {
        
    }
}