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
    /*public enum dir
    {
        up,
        down,
        left,
        right
    }

    public dir startingdDirection;*/

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
    public string name;
    public int width;
    public int height;
    public int positionX;
    public int positionY;
    public int hp;
    public int armor;
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

    public dir direction;
    //public Entity.dir startingDirection;

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
        //startingDirection = data.startingdDirection;
    }
    
    public void TakeDamage(int dmg)
    {
        hp -= dmg-armor;
    }
}
