using System;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] PlayerData _playerData;
    [SerializeField] List<FishData> _fishDatas;
    private Entity[,] grid;
    void Start()
    {
        grid = createGrid(10,10);
        spawnEntitys();
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
            }
        }
        Move(_fishDatas[0], 4,8);
    }
    
    void Update()
    {
        
    }

    Entity[,] createGrid(int y, int x)
    {
        return new Entity[y,x];
    }

    void spawnEntitys()
    {
        foreach (var fish in _fishDatas)
        {
            for (int i = 0; i < fish.height; i++)
            {
                for (int j = 0; j < fish.width; j++)
                {
                    grid[fish.positionY + i,fish.positionX + j] = fish;
                }
            }
        }
    }

    void Move(Entity entity, int posX, int posY)
    {
        int rangeY = Mathf.Abs(entity.positionY - posY);
        int rangeX = Mathf.Abs(entity.positionX - posX);
        int range = rangeY + rangeX;
        for (int i = 1; i < range+1; i++)
        {
            float tempPosY = entity.positionY + (float)rangeY / range * i;
            float tempPosX = entity.positionX + (float)rangeX / range * i;
            Debug.Log(tempPosX);
            Debug.Log(tempPosY);
            if(!canMove(entity, (int)tempPosY, (int)tempPosX))
            { 
                return;
            }
            else
            {
                Debug.Log("canMove");
            }
        }
        for (int i = 0; i < entity.height; i++)
        {
            for (int j = 0; j < entity.width; j++)
            {
                grid[entity.positionY + i,entity.positionX + j] = null;
            }
        }
        for (int i = 0; i < entity.height; i++)
        {
            for (int j = 0; j < entity.width; j++)
            {
                grid[posY + i,posX + j] = entity;
            }
        }
    }
    
    bool canMove(Entity entity, int newX, int newY)
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
}
