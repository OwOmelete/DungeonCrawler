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
    public PlayerDataInstance(PlayerData data) : base(data)
    {
        
    }
}