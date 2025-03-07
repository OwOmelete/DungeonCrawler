using UnityEngine;

public abstract class Entity : ScriptableObject
{
    public int width;
    public int height;
    public int positionX;
    public int positionY;
    public int hp;
    public int armor;
    // movement list
    // attack list

    public abstract EntityInstance Instance();

    //take damage

    //isDead
}

public class EntityInstance
{
    public int width;
    public int height;
    public int positionX;
    public int positionY;
    public int hp;
    public int armor;

    public EntityInstance(Entity data)
    {
        width = data.width;
        height = data.height;
        positionX = data.positionX;
        positionY = data.positionY;
        hp = data.hp;
        armor = data.armor;
    }
}
