using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/Entity/PlayerData")]
public class PlayerData : Entity
{
    public override EntityInstance Instance()
    {
        return new PlayerDataInstance(this);
    }
}

public class PlayerDataInstance : EntityInstance
{
    public bool isStanding = true;
    public float light = 10;
    public PlayerDataInstance(PlayerData data) : base(data)
    {
        
    }
}