using System.Collections.Generic;
using System.Data;
using UnityEngine;

public abstract class Entity : ScriptableObject
{
    public string name;
    public int width;
    public int height;
    public int positionX;
    public int positionY;
    public int hp;
    public int armor;
    public int actionPoint;
    public enum dir
    {
        up,
        down,
        left,
        right
    }

    public dir startingDirection;

    public GameObject prefab;

    public List<AttackData> attackList;
    public List<WeakPointData> weakPointList;
    // movement list
    // attack list

    public abstract EntityInstance Instance();

    

    //isDead
}

public class EntityInstance
{
    [Header("Pour nos merveilleux gds")]
    public int hp;
    public int armor;
    public string name;
    public int positionX;
    public int positionY;
    
    [Header("UNIQUEMENT POUR BROTULO")]
    public dir direction;
    
    [Header("Pas touche")]
    public int width;
    public int height;
    public GameObject prefab;
    public List<AttackData> attackList;
    public List<WeakPointData> weakPointList;
    public int actionPoint;
    public int initialActionPoint;
    public bool isStanding = true;
    public Transform entityChild;
    public AbstractIA behaviour;
    public List<SpikeInstance> spikeList = new List<SpikeInstance>();
    public List<int> spikeIndexSupr = new List<int>();
    public enum dir
    {
        up,
        down,
        left,
        right
    }

    

    public EntityInstance(Entity data)
    {
        name = data.name;
        width = data.width;
        height = data.height;
        positionX = data.positionX;
        positionY = data.positionY;
        hp = data.hp;
        armor = data.armor;
        prefab = data.prefab;
        attackList = data.attackList;
        weakPointList = data.weakPointList;
        actionPoint = data.actionPoint;
        initialActionPoint = data.actionPoint;
    }
    
    public void TakeDamage(int dmg)
    {
        int losthp = dmg-armor;
        if (losthp >= 0)
        {
            hp -= losthp;
        }
    }
}
