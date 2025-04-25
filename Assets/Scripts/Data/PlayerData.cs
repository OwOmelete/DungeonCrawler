using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/Entity/PlayerData")]
public class PlayerData : Entity
{
    public float ligth;
    public float oxygen;
    public float oxygenLostMove;
    public float oxygenLostMove2Tiles;
    public float oxygenLostRotate;
    public float oxygenLostAttack;
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
    public float oxygenLostMove;
    public float oxygenLostMove2Tiles;
    public float oxygenLostRotate;
    public float oxygenLostAttack;
    public PlayerDataInstance(PlayerData data) : base(data)
    {
        light = data.ligth;
        oxygen = data.oxygen;
        RespirationDatas = data.RespirationDatas;
        currentAttack = data.attackList[0];
        oxygenLostAttack = data.oxygenLostAttack;
        oxygenLostMove = data.oxygenLostMove;
        oxygenLostMove2Tiles = data.oxygenLostMove2Tiles;
        oxygenLostRotate = data.oxygenLostRotate;
    }
}