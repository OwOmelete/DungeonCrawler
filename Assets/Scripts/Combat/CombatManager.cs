using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class CombatManager : MonoBehaviour
{
    public PlayerData _playerData;
    public PlayerDataInstance player;
    public Light2D playerLight;
    public List<FishData> _fishDatas;
    [SerializeField] int gridHeight;
    [SerializeField] int gridWidth;
    [SerializeField] float lightLostPerTurn;
    [SerializeField] GameObject combatScene;
    [SerializeField] GameObject playerExploration;
    [SerializeField] LightManager lightManager;
    [SerializeField] OxygenManager oxygenManager;
    private List<Fish> fishes = new List<Fish>();
    private EntityInstance[,] grid;
    private List<EntityInstance> turnOrder = new List<EntityInstance>();
    private bool combatFinished;
    private int currentTurnIndex = 0;
    private bool isPlaying;
    private int frameCap = 60;
    private bool isFirstFight = false;

    public void InitCombat()
    {
        combatFinished = false;
        grid = CreateGrid(gridHeight, gridWidth);
        SpawnEntitys();
        playerLight = player.prefab.transform.GetChild(0).GetComponent<Light2D>();
        UpdateLight();

        ///////// TESTS //////////
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
            }
        }
    }
    

    void UpdateLight()
    {
        playerLight.pointLightOuterRadius = player.light;
        playerLight.pointLightInnerRadius = player.light / 2;
    }
    void Update()
    {
        if (combatFinished)
        {
            Debug.Log("vous avez gagné !!");
            player.positionX = _playerData.positionX;
            player.positionY = _playerData.positionY;
            player.actionPoint = player.RespirationDatas[player.respirationIndex].actionPoints;
            Destroy(player.prefab);
            turnOrder.Clear();
            fishes.Clear();
            currentTurnIndex = 0;
            player.prefab.SetActive(false);
            lightManager.AddLight(0);
            oxygenManager.canLooseOxygen = true;
            oxygenManager.RestartCoroutine();
            playerExploration.SetActive(true);
            combatScene.SetActive(false);
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
        if (currentTurnIndex % turnOrder.Count == 0)
        {
            player.light -= lightLostPerTurn;
            player.light = Mathf.Clamp(player.light, 1, 10);
            UpdateLight();
        }
    }

    void PlayerTurn(PlayerDataInstance playerEntity)
    {
        Action(playerEntity);
        if (playerEntity.actionPoint == 0)
        {
            EndTurn();
            //player.oxygen -= player.RespirationDatas[player.respirationIndex].oxygenLoss;
            Debug.Log("plus d'action point");
            //Debug.Log("oxygen : " + player.oxygen);
            playerEntity.actionPoint = playerEntity.RespirationDatas[playerEntity.respirationIndex].actionPoints;
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
        int actionPointLost = 1;
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

        if (playerEntity.actionPoint == playerEntity.RespirationDatas[playerEntity.respirationIndex].actionPoints)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                playerEntity.respirationIndex = 0;
                Debug.Log(playerEntity.RespirationDatas[playerEntity.respirationIndex]);
                playerEntity.actionPoint = playerEntity.RespirationDatas[playerEntity.respirationIndex].actionPoints;
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                playerEntity.respirationIndex = 1;
                Debug.Log(playerEntity.RespirationDatas[playerEntity.respirationIndex]);
                playerEntity.actionPoint = playerEntity.RespirationDatas[playerEntity.respirationIndex].actionPoints;
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                playerEntity.respirationIndex = 2;
                Debug.Log(playerEntity.RespirationDatas[playerEntity.respirationIndex]);
                playerEntity.actionPoint = playerEntity.RespirationDatas[playerEntity.respirationIndex].actionPoints;
            }
        }
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
        else if (Input.GetKeyDown(KeyCode.A))
        {
            player.currentAttack = player.attackList[0];
            actionPointLost = 0;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            player.currentAttack = player.attackList[1];
            actionPointLost = 0;
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            if (player.actionPoint >= player.currentAttack.actionCost)
            {
                Attack(player.currentAttack, player, player.positionX, player.positionY + 1);
                actionPointLost = player.currentAttack.actionCost;
            }
            else
            {
                actionPointLost = 0;
                Debug.Log("pas assez d'action points");
            }
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            if (player.actionPoint >= player.currentAttack.actionCost)
            {
                Attack(player.currentAttack, player, player.positionX, player.positionY - 1);
                actionPointLost = player.currentAttack.actionCost;
            }
            else
            {
                actionPointLost = 0;
                Debug.Log("pas assez d'action points");
            }
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            if (player.actionPoint >= player.currentAttack.actionCost)
            {
                Attack(player.currentAttack, player, player.positionX + 1, player.positionY);
                actionPointLost = player.currentAttack.actionCost;
            }
            else
            {
                actionPointLost = 0;
                Debug.Log("pas assez d'action points");
            }
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            if (player.actionPoint >= player.currentAttack.actionCost)
            {
                Attack(player.currentAttack, player, player.positionX - 1, player.positionY);
                actionPointLost = player.currentAttack.actionCost;
            }
            else
            {
                actionPointLost = 0;
                Debug.Log("pas assez d'action points");
            }
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

        if (playerEntity.actionPoint == playerEntity.RespirationDatas[playerEntity.respirationIndex].actionPoints)
        {
            player.oxygen -= player.RespirationDatas[player.respirationIndex].oxygenLoss;
            Debug.Log("oxygen : " + player.oxygen);
        }
        playerEntity.actionPoint -= actionPointLost;
        Debug.Log("points d'action restants : " + playerEntity.actionPoint);
    }

    void FlipPlayer()
    {
        if (player.isStanding && player.positionX + player.width <grid.GetLength(1)-1)
        {
            if (grid[player.positionY,player.positionX + player.width] == null)
            {
                grid[player.positionY,player.positionX + player.width] = player;
                grid[ player.positionY + player.height-1,player.positionX] = null;
                player.height = 1;
                player.width = 2;
                player.prefab.transform.position = new Vector3(player.positionX, player.positionY + 1,0);
                player.prefab.transform.rotation = Quaternion.Euler(0,0,-90);
                player.isStanding = !player.isStanding;
            }
        }
        else if (player.positionY + player.height <grid.GetLength(0)-1)
        {
            if (grid[ player.positionY + player.height,player.positionX] == null)
            {
                grid[ player.positionY + player.height,player.positionX] = player;
                grid[player.positionY,player.positionX + player.width-1] = null;
                player.height = 2;
                player.width = 1;
                player.prefab.transform.position = new Vector3(player.positionX, player.positionY,0);
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
        if (1==0)
        {
            player = (PlayerDataInstance)_playerData.Instance();
            isFirstFight = false;
        }
        player.prefab = Instantiate(_playerData.prefab,
                new Vector3(_playerData.positionX, _playerData.positionY, 0),quaternion.identity);
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
            EntityInstance tile = GetAttackTile(entity, dirY, dirX, i, attack);
            if (tile != null && tile != entity && tile != lastEnnemyTouched)
            {
                Debug.Log(tile);
                Debug.Log("touché");
                int dmg = attack.Damage;
                float r = Random.Range(0,1);
                if (r <= attack.critChance)
                {
                    Debug.Log("crit !");
                    dmg = (int)(dmg * attack.critMultiplier);
                    Debug.Log(dmg);
                }

                if (entity == player)
                {
                    dmg = (int)(dmg * player.RespirationDatas[player.respirationIndex].damageMultiplier);
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

    EntityInstance GetAttackTile(EntityInstance entity, int dirY, int dirX, int actualRange, AttackData attack)
    {
        int y = GetOutTileY(entity, dirY, attack) + (actualRange - 1) * GetSign(dirY);
        int x = GetOutTileX(entity, dirX, attack) + (actualRange - 1) * GetSign(dirX);
        if (y < 0 || y >= grid.GetLength(0) || x < 0 || x >= grid.GetLength(1))
        {
            return null;
        }
        return grid[y,x];
    }
    int GetOutTileX(EntityInstance entity, int dirX, AttackData attack)
    {
        int Xoffset = attack.Xoffset;
        if (!entity.isStanding)
        {
            Xoffset = attack.Yoffset;
        }
        return entity.positionX + entity.width * NegativeToZero(GetSign(dirX)) + Xoffset*ZeroToOne(dirX) +
            1 * GetSign(dirX)-NegativeToOne(dirX);
    }
    int GetOutTileY(EntityInstance entity, int dirY,AttackData attack)
    {
        int Yoffset = attack.Yoffset;
        if (!entity.isStanding)
        {
            Yoffset = attack.Xoffset;
        }
        return entity.positionY + entity.height * NegativeToZero(GetSign(dirY)) + Yoffset*ZeroToOne(dirY) +
            1 * GetSign(dirY)-NegativeToOne(dirY);
    }

    void Damage(EntityInstance entity, int dmg)
    {
        entity.TakeDamage(dmg);
        Debug.Log(entity.hp);
        if (entity.hp <= 0)
        {
            if (entity == player)
            {
                // gérer mort player
            }
            Debug.Log("mort théorique");
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

    int ZeroToOne(int value)
    {
        if (value == 0)
        {
            return 1;
        }

        return 0;
    }
}