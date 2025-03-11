using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] PlayerData _playerData;
    [SerializeField] List<FishData> _fishDatas;
    private List<Fish> fishes = new List<Fish>();
    private PlayerDataInstance player;
    private EntityInstance[,] grid;

    private void Start()
    {
        player = (PlayerDataInstance)_playerData.Instance();
        foreach (var fish in _fishDatas)
        {
            Fish newFish = new Fish();
            newFish.fishData = fish;
            newFish.fishDataInstance = (FishDataInstance)fish.Instance();
            fishes.Add(newFish);
        }

        grid = CreateGrid(10, 10);
        SpawnEntitys();
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
            }
        }
        
        ///////// TESTS //////////

        Move(fishes[0].fishDataInstance, 1, 8);
        Move(player, 5, 8);
        Attack(player.attackList[0], player, 1, 8);
        Debug.Log(fishes[0].fishDataInstance.hp);
}
    
    void Update()
    {
        
    }

    EntityInstance[,] CreateGrid(int y, int x)
    {
        return new EntityInstance[y,x];
    }

    void SpawnEntitys()
    {
        for (int i = 0; i < player.height; i++)
        {
            for (int j = 0; j < player.width; j++)
            {
                grid[player.positionY + i,player.positionX + j] = player;
            }
        }
        player.prefab = Instantiate(player.prefab,
            new Vector3(player.positionX, player.positionY, 0),quaternion.identity);
        foreach (var fish in fishes)
        {
            for (int i = 0; i < fish.fishData.height; i++)
            {
                for (int j = 0; j < fish.fishData.width; j++)
                {
                    grid[fish.fishData.positionY + i,fish.fishData.positionX + j] = fish.fishDataInstance;
                }
            }
            fish.fishDataInstance.prefab = Instantiate(fish.fishData.prefab,
                new Vector3(fish.fishData.positionX, fish.fishData.positionY, 0),quaternion.identity);
        }
    }

    void Move(EntityInstance entity, int posX, int posY)
    {
        int rangeY = Mathf.Abs(entity.positionY - posY);
        int rangeX = Mathf.Abs(entity.positionX - posX);
        int range = rangeY + rangeX;
        for (int i = 1; i < range+1; i++)
        {
            float tempPosY = entity.positionY + (float)rangeY / range * i;
            float tempPosX = entity.positionX + (float)rangeX / range * i;
            if(!CanMove(entity, (int)tempPosY, (int)tempPosX))
            { 
                return;
            }
            else
            {
            }
        }
        for (int i = 0; i < entity.height; i++)
        {
            for (int j = 0; j < entity.width; j++)
            {
                grid[entity.positionY + i,entity.positionX + j] = null;
                grid[posY + i,posX + j] = entity;
            }
        }
        entity.prefab.transform.position = new Vector3(posX, posY, 0);
        entity.positionX = posX;
        entity.positionY = posY;
    }
    
    bool CanMove(EntityInstance entity, int newX, int newY)
    {
        for (int i = 0; i < entity.height; i++)
        {
            for (int j = 0; j < entity.width; j++)
            {
                if (newY+i < 0 || newY+i > grid.GetLength(0)-1 || newX+j < 0 || newX+j > grid.GetLength(1)-1)
                {
                    return false;
                }
                if (grid[newY+i, newX+j] != null && grid[newY+i, newX+j] != entity)
                {
                    return false;
                }
            }
        }
        return true;
    }

    void Attack(AttackData attack,EntityInstance entity, int x, int y)
    {
        int dirX = x - entity.positionX;
        int dirY = y - entity.positionY;
        for (int i = 0; i < attack.range; i++)
        {
            /*if (grid[y+i*(int)Mathf.Sign(dirY), x+i*(int)Mathf.Sign(dirX)] != null)
            {
                Debug.Log("touché");
                Damage(grid[y, x], attack.Damage);
            }*/
            
            Debug.Log( entity.positionY + (entity.height) * NegativeToZero(GetSign(dirY)) +
                       i * GetSign(dirY)-NegativeToOne(dirY));
            Debug.Log(entity.positionX + (entity.width) * NegativeToZero(GetSign(dirX)) +
                      i * GetSign(dirX)-NegativeToOne(dirX));
            EntityInstance tile = grid[
                entity.positionY + entity.height * NegativeToZero(GetSign(dirY)) +
                i * GetSign(dirY)-NegativeToOne(dirY),
                entity.positionX + entity.width * NegativeToZero(GetSign(dirX)) +
                i * GetSign(dirX)-NegativeToOne(dirX)];

            if (tile != null && tile != entity)
            {
                Debug.Log("touché");
                Damage(grid[y, x], attack.Damage);
            }
        }
    }

    void Damage(EntityInstance entity, int dmg)
    {
        entity.TakeDamage(dmg);
    }

    int NegativeToZero(int value)
    {
        if (value < 0)
        {
            return 0;
        }

        return value;
    }

    int GetSign(int value)
    {
        if (value == 0)
        {
            return 0;
        }

        return (int)Mathf.Sign(value);
    }

    int NegativeToOne(int value)
    {
        if (value < 0)
        {
            return 1;
        }
        else return 0;
    }
}
