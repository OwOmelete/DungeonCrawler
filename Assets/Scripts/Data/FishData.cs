using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "FishData", menuName = "Scriptable Objects/Entity/FishData")]
public class FishData : Entity
{
    public string name;
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
    public FishDataInstance(FishData data) : base(data)
    {
        name = data.name;
    }
}