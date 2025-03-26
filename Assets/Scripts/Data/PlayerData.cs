using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/Entity/PlayerData")]
public class PlayerData : Entity
{
    public float ligth;
    public float oxygen;
    public List<RespirationData> RespirationDatas = new List<RespirationData>();
    public override EntityInstance Instance()
    {
        return new PlayerDataInstance(this);
    }
}

public class PlayerDataInstance : EntityInstance
{
    public bool isStanding = true;
    public float light;
    public float oxygen;
    public int respirationIndex = 1;
    public List<RespirationData> RespirationDatas;
    public PlayerDataInstance(PlayerData data) : base(data)
    {
        light = data.ligth;
        oxygen = data.oxygen;
        RespirationDatas = data.RespirationDatas;
    }
}