using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/Entity/PlayerData")]
public class PlayerData : Entity
{
    public float light;
    public float oxygen;
    public List<RespirationData> RespirationDatas = new List<RespirationData>();
    public override EntityInstance Instance()
    {
        return new PlayerDataInstance(this);
    }
}

public class PlayerDataInstance : EntityInstance
{
    public float light;
    public float oxygen;
    public int respirationIndex = 1;
    public List<RespirationData> RespirationDatas;
    public AttackData currentAttack;
    public bool booster = true;
    public PlayerDataInstance(PlayerData data) : base(data)
    {
        light = data.light;
        oxygen = data.oxygen;
        RespirationDatas = data.RespirationDatas;
        currentAttack = data.attackList[0];
    }
}