using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class CombatManager : MonoBehaviour
{
    public PlayerData _playerData;
    public PlayerDataInstance player;
    public List<FishData> _fishDatas;
    [SerializeField] int gridHeight;
    [SerializeField] int gridWidth;
    private List<Fish> fishes = new List<Fish>();
    private EntityInstance[,] grid;
    private List<EntityInstance> turnOrder = new List<EntityInstance>();
    private bool combatFinished;
    private int currentTurnIndex = 0;
    private bool isPlaying;
    private int frameCap = 60;
    
    public void InitCombat()
    {
        combatFinished = false;
        grid = CreateGrid(gridHeight, gridWidth);
        SpawnEntitys();

        ///////// TESTS //////////
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
            }
        }
    }
    void Update()
    {
        if (combatFinished)
        {
            Debug.Log("vous avez gagné !!");
            return;
        }
        EntityInstance currentEntity = turnOrder[currentTurnIndex];
        if (currentEntity == player)
        {
            PlayerTurn(player);
        }
        else
        {
            IATurn(currentEntity);
        }
    }


    private void EndTurn()
    {
        currentTurnIndex = (currentTurnIndex + 1) % turnOrder.Count;
    }

    void PlayerTurn(PlayerDataInstance playerEntity)
    {
        Action(playerEntity);
        if (playerEntity.actionPoint == 0)
        {
            EndTurn();
            Debug.Log("plus d'action point");
            playerEntity.actionPoint = _playerData.actionPoint;
        }
    }
    
    void IATurn(EntityInstance entity)
    {
        Action(entity);
        if (entity.actionPoint == 0)
        {
            EndTurn();
            Debug.Log("plus d'action point");
            entity.actionPoint = entity.initialActionPoint;
        }
    }

    void Action(PlayerDataInstance playerEntity)
    {
        int verticalSpeed;
        int horizontalSpeed;
        if (playerEntity.isStanding)
        {
            verticalSpeed = 2;
            horizontalSpeed = 1;
        }
        else
        {
            verticalSpeed = 1;
            horizontalSpeed = 2;
        }
        #region Actions
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Move(playerEntity, playerEntity.positionX, playerEntity.positionY + verticalSpeed);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(playerEntity, playerEntity.positionX, playerEntity.positionY - verticalSpeed);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(playerEntity, playerEntity.positionX + horizontalSpeed, playerEntity.positionY);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(playerEntity, playerEntity.positionX - horizontalSpeed, playerEntity.positionY);
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            Attack(player.attackList[0], player, player.positionX, player.positionY + 1);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            Attack(player.attackList[0], player, player.positionX, player.positionY - 1);
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            Attack(player.attackList[0], player, player.positionX + 1, player.positionY);
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            Attack(player.attackList[0], player, player.positionX - 1, player.positionY);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            FlipPlayer();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("passer son tour");
            playerEntity.actionPoint = 0;
            return;
        }
        else
        {
            return;
        }
        #endregion
        playerEntity.actionPoint -= 1;
        Debug.Log("points d'action restants : " + playerEntity.actionPoint);
    }

    void FlipPlayer()
    {
        if (player.isStanding && player.positionX + player.width <grid.GetLength(1)-1)
        {
            if (grid[player.positionX + player.width, player.positionY] == null)
            {
                player.height = 1;
                player.width = 2;
                player.prefab.transform.position = new Vector3(player.positionX, player.positionY + 1,0);
                player.prefab.transform.rotation = Quaternion.Euler(0,0,-90);
                player.isStanding = !player.isStanding;
            }
        }
        else if (player.positionY + player.height <grid.GetLength(0)-1)
        {
            if (grid[player.positionX, player.positionY + player.height] == null)
            {
                player.height = 2;
                player.width = 1;
                player.prefab.transform.rotation = Quaternion.Euler(0,0,0);
                player.isStanding = !player.isStanding;
            }
        }
        
    }
    
    void Action(EntityInstance entity)
    {
        #region Actions
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Move(entity, entity.positionX, entity.positionY + 1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(entity, entity.positionX, entity.positionY - 1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(entity, entity.positionX + 1, entity.positionY);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(entity, entity.positionX - 1, entity.positionY);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("passer son tour");
            entity.actionPoint = 0;
            return;
        }
        else
        {
            return;
        }
        #endregion
        entity.actionPoint -= 1;
        Debug.Log("points d'action restants : " + entity.actionPoint);
    }

    EntityInstance[,] CreateGrid(int y, int x)
    {
        return new EntityInstance[y,x];
    }

    void SpawnEntitys()
    {
        if (player == null)
        {
            player = (PlayerDataInstance)_playerData.Instance();
        }
        turnOrder.Add(player);
        foreach (var fish in _fishDatas)
        {
            Fish newFish = new Fish
            {
                fishData = fish,
                fishDataInstance = (FishDataInstance)fish.Instance()
            };
            fishes.Add(newFish);
            turnOrder.Add(newFish.fishDataInstance);
        }
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
                new Vector3(fish.fishData.positionX,
                    fish.fishData.positionY, 0),quaternion.identity);
        }
    }

    void Move(EntityInstance entity, int posX, int posY)
    {
        if(!CanMove(entity, posX, posY))
        { 
            return;
        }
        else
        {
                
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

        if (entity == player)
        {
            if (!player.isStanding)
            {
                entity.prefab.transform.position = new Vector3(posX, posY+1, 0);
            }
            else
            {
                entity.prefab.transform.position = new Vector3(posX, posY, 0);
            }
        }
        else
        {
            entity.prefab.transform.position = new Vector3(posX, posY, 0);
        }
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
        EntityInstance lastEnnemyTouched = null;
        for (int i = 0; i < attack.range; i++)
        {
            /*if (grid[y+i*(int)Mathf.Sign(dirY), x+i*(int)Mathf.Sign(dirX)] != null)
            {
                Debug.Log("touché");
                Damage(grid[y, x], attack.Damage);
            }
            Debug.Log( entity.positionY + (entity.height) * NegativeToZero(GetSign(dirY)) +
                       i * GetSign(dirY)-NegativeToOne(dirY));
            Debug.Log(entity.positionX + (entity.width) * NegativeToZero(GetSign(dirX)) +
                      i * GetSign(dirX)-NegativeToOne(dirX));*/
            EntityInstance tile = GetAttackTile(entity, dirY, dirX, i);
            if (tile != null && tile != entity && tile != lastEnnemyTouched)
            {
                Debug.Log(tile);
                Debug.Log("touché");
                int dmg = attack.Damage;
                int r = Random.Range(0,1);
                if (r <= attack.critChance)
                {
                    Debug.Log("crit !");
                    dmg = (int)(dmg * attack.critMultiplier);
                    Debug.Log(dmg);
                }
                Damage(tile, dmg);
                lastEnnemyTouched = tile;
            }
            else
            {
                Debug.Log("cantAttack");
            }
        }
    }

    EntityInstance GetAttackTile(EntityInstance entity, int dirY, int dirX, int actualRange)
    {
        int y = GetOutTileY(entity, dirY) + (actualRange - 1) * GetSign(dirY);
        int x = GetOutTileX(entity, dirX) + (actualRange - 1) * GetSign(dirX);
        if (y < 0 || y >= grid.GetLength(0) || x < 0 || x >= grid.GetLength(1))
        {
            return null;
        }
        return grid[y,x];
    }
    int GetOutTileX(EntityInstance entity, int dirX)
    {
        return entity.positionX + entity.width * NegativeToZero(GetSign(dirX)) +
            1 * GetSign(dirX)-NegativeToOne(dirX);
    }
    int GetOutTileY(EntityInstance entity, int dirY)
    {
        return entity.positionY + entity.height * NegativeToZero(GetSign(dirY)) +
            1 * GetSign(dirY)-NegativeToOne(dirY);
    }

    void Damage(EntityInstance entity, int dmg)
    {
        entity.TakeDamage(dmg);
        if (entity.hp <= 0)
        {
            if (entity == player)
            {
                // gérer mort player
            }
            Die(entity);
            if (turnOrder.Count == 1)
            {
                combatFinished = true;
            }
        }
    }

    void Die(EntityInstance entity)
    {
        for (int i = 0; i < entity.height; i++)
        {
            for (int j = 0; j < entity.width; j++)
            {
                grid[entity.positionY + i,entity.positionX + j] = null ;
            }
        }
        turnOrder.Remove(entity);
        Destroy(entity.prefab);
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
        return 0;
    }
}
