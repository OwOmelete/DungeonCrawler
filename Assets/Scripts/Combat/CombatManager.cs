using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;
using DG.Tweening;

public class CombatManager : MonoBehaviour
{
    public PlayerData _playerData;
    public PlayerDataInstance player;
    public Light2D playerLight;
    public List<FishData> _fishDatas;
    [SerializeField] int gridHeight;
    [SerializeField] int gridWidth;
    [SerializeField] float moveDuration;
    [SerializeField] float lightLostPerTurn;
    [SerializeField] GameObject combatScene;
    [SerializeField] GameObject playerExploration;
    [SerializeField] GameObject UiExplo;
    [SerializeField] LightManager lightManager;
    [SerializeField] OxygenManager oxygenManager;
    private List<Fish> fishes = new List<Fish>();
    private EntityInstance[,] grid;
    private List<EntityInstance> turnOrder = new List<EntityInstance>();
    private bool combatFinished;
    private int currentTurnIndex = 0;
    private bool isPlaying;
    private int attackCordsX;
    private int attackCordsY;
    private SpriteRenderer playerEntityRenderer;
    private Transform playerEntityChild;
    private bool canRotate = true;

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
            lightManager.canLooseLight = true;
            lightManager.RestartCoroutine();
            oxygenManager.canLooseOxygen = true;
            oxygenManager.RestartCoroutine();
            playerExploration.SetActive(true);
            UiExplo.SetActive(true);
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
        /*playerEntityRenderer.flipX = playerEntity.direction == EntityInstance.dir.upLeft ||
                                     playerEntity.direction == EntityInstance.dir.downRight;
        playerEntityRenderer.flipY = playerEntity.direction == EntityInstance.dir.rightUp ||
                                     playerEntity.direction == EntityInstance.dir.leftDown;*/
        
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
        int verticalSpeed = 1;
        int horizontalSpeed = 1;
        int actionPointLost = 1;
        if (playerEntity.isStanding && player.booster)
        {
            verticalSpeed = 2;
            horizontalSpeed = 1;
        }
        else if (player.booster)
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

        if (Input.GetKeyDown(KeyCode.W))
        {
            player.booster = !player.booster;
            actionPointLost = 0;
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
        else if (Input.GetKeyDown(KeyCode.Q) && canRotate)
        {
            FlipPlayerRight(player);
        }
        else if (Input.GetKeyDown(KeyCode.E) && canRotate)
        {
            FlipPlayerLeft(player);
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

    void FlipPlayerRight(EntityInstance entity)
    {
        if (canTurnRight(entity))
        {
            if (entity.direction == EntityInstance.dir.left && (grid[entity.positionY + 1 , entity.positionX] == null ||grid[entity.positionY +1 , entity.positionX] == entity))
            {
                DoRotateRight(entity);
            }
            else if (entity.direction == EntityInstance.dir.up && (grid[entity.positionY , entity.positionX+1] == null||grid[entity.positionY, entity.positionX+1] == entity))
            {
                DoRotateRight(entity);            
            }
            else if (entity.direction == EntityInstance.dir.right && (grid[entity.positionY -1 , entity.positionX] == null ||grid[entity.positionY -1 , entity.positionX] == entity ))
            {
                DoRotateRight(entity);            
            }
            else if (entity.direction == EntityInstance.dir.down && (grid[entity.positionY, entity.positionX-1] == null||grid[entity.positionY , entity.positionX-1] == entity))
            {
                DoRotateRight(entity);            
            }
        }
    }
    void FlipPlayerLeft(EntityInstance entity)
    {
        if (canTurnLeft(entity)) 
        {
            if (entity.direction == EntityInstance.dir.left && (grid[entity.positionY - 1, entity.positionX] == null ||grid[entity.positionY -1 , entity.positionX] == entity))
            {
                DoRotateLeft(entity);
            }

            else if (entity.direction == EntityInstance.dir.up && (grid[entity.positionY , entity.positionX-1] == null||grid[entity.positionY, entity.positionX-1] == entity))
            {
                DoRotateLeft(entity);
            }
            
            else if (entity.direction == EntityInstance.dir.right && (grid[entity.positionY +1 , entity.positionX] == null ||grid[entity.positionY +1 , entity.positionX] == entity ))
            {
                DoRotateLeft(entity);
            }
            
            else if (entity.direction == EntityInstance.dir.down && (grid[entity.positionY, entity.positionX+1] == null||grid[entity.positionY , entity.positionX+1] == entity))
            {
                DoRotateLeft(entity);
            }
        }
    }
    
    private void DoRotateRight(EntityInstance entity)
    {
        canRotate = false;
        entity.entityChild.DOLocalRotate(new Vector3(0,0,entity.entityChild.localEulerAngles.z - 90), moveDuration).SetEase(Ease.InOutCubic).OnComplete(() =>
        {
            canRotate = true;
        });
        switch (entity.direction)
        {
            case EntityInstance.dir.up:
                assignDirection(EntityInstance.dir.right, entity);
                break;
            case EntityInstance.dir.down:
                assignDirection(EntityInstance.dir.left, entity);
                break;
            case EntityInstance.dir.left:
                assignDirection(EntityInstance.dir.up, entity);
                break;
            case EntityInstance.dir.right:
                assignDirection(EntityInstance.dir.down, entity);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        //player.prefab.transform.position = new Vector3(player.positionX, player.positionY, 0);
        //player.prefab.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void DoRotateLeft(EntityInstance entity)
    {
        canRotate = false;
        entity.entityChild.DOLocalRotate(new Vector3(0,0,entity.entityChild.localEulerAngles.z + 90), moveDuration).SetEase(Ease.InOutCubic).OnComplete(() =>
        {
            canRotate = true;
        });
        switch (entity.direction)
        {
            case EntityInstance.dir.up:
                assignDirection(EntityInstance.dir.left, entity);
                break;
            case EntityInstance.dir.down:
                assignDirection(EntityInstance.dir.right, entity);
                break;
            case EntityInstance.dir.left:
                assignDirection(EntityInstance.dir.down, entity);
                break;
            case EntityInstance.dir.right:
                assignDirection(EntityInstance.dir.up, entity);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        //player.prefab.transform.position = new Vector3(player.positionX, player.positionY, 0);
        //player.prefab.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    void assignDirection(EntityInstance.dir newDirection, EntityInstance entity)
    {
        switch (entity.direction)
        {
            case EntityInstance.dir.up:
                grid[entity.positionY + 1, entity.positionX] = null;
                break;
            case EntityInstance.dir.down:
                grid[entity.positionY - 1, entity.positionX] = null;
                break;
            case EntityInstance.dir.left:
                grid[entity.positionY, entity.positionX - 1] = null;
                break;
            case EntityInstance.dir.right:
                grid[entity.positionY, entity.positionX + 1] = null;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        entity.direction = newDirection;
        int tempHeight = entity.height;
        switch (newDirection)
        {
            case EntityInstance.dir.up:
                grid[entity.positionY + 1, entity.positionX] = player;
                entity.height = entity.width;
                entity.width = tempHeight;
                entity.isStanding = true;
                break;
            case EntityInstance.dir.down:
                grid[entity.positionY - 1, entity.positionX] = player;
                entity.height = entity.width;
                entity.width = tempHeight;
                entity.isStanding = true;
                break;
            case EntityInstance.dir.left:
                grid[entity.positionY, entity.positionX - 1] = player;
                entity.height = entity.width;
                entity.width = tempHeight;
                entity.isStanding = false;
                break;
            case EntityInstance.dir.right:
                grid[entity.positionY, entity.positionX + 1] = player;
                entity.height = entity.width;
                entity.width = tempHeight;
                entity.isStanding = false;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newDirection), newDirection, null);
        }
    }

    public bool canTurnLeft(EntityInstance entity)
    {
        if (entity.direction == EntityInstance.dir.right && entity.positionY < grid.GetLength(0) - 1)
        {
            return true;
        }
        if (entity.direction == EntityInstance.dir.up && entity.positionX > 0)
        {
            return true;
        }
        if (entity.direction == EntityInstance.dir.left && entity.positionY > 0)
        {
            return true;
        }
        if (entity.direction == EntityInstance.dir.down && entity.positionX < grid.GetLength(1) - 1)
        {
            return true;
        }

        return false;
    }
    public bool canTurnRight(EntityInstance entity)
    {
        if (entity.direction == EntityInstance.dir.right && entity.positionY > 0)
        {
            return true;
        }
        if (entity.direction == EntityInstance.dir.up && entity.positionX < grid.GetLength(1) - 1)
        {
            return true;
        }
        if (entity.direction == EntityInstance.dir.left && entity.positionY < grid.GetLength(0) - 1)
        {
            return true;
        }
        if (entity.direction == EntityInstance.dir.down && entity.positionX > 0)
        {
            return true;
        }

        return false;
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
        player.prefab = Instantiate(_playerData.prefab,
                new Vector3(_playerData.positionX, _playerData.positionY, 0),quaternion.identity);
        playerEntityRenderer = player.prefab.GetComponentInChildren<SpriteRenderer>();
        player.entityChild = player.prefab.transform.GetChild(0);
        Debug.Log(player.entityChild);
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
                    fish.fishDataInstance.entityChild = fish.fishDataInstance.prefab.transform.GetChild(0);
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
        
        UpdateGrid(entity, posX, posY);
        entity.prefab.transform.DOMove(new Vector3(posX, posY, 0), moveDuration).SetEase(Ease.InOutCubic);
        entity.positionX = posX;
        entity.positionY = posY;
    }

    void UpdateGrid(EntityInstance entity, int posX, int posY)
    {
        for (int i = 0; i < entity.height; i++)
        {
            for (int j = 0; j < entity.width; j++)
            {
                if (player.direction == EntityInstance.dir.up)
                {
                    grid[entity.positionY + i,entity.positionX + j] = null;
                }
                if (player.direction == EntityInstance.dir.right)
                {
                    grid[entity.positionY + i,entity.positionX + j] = null;
                }
                if (player.direction == EntityInstance.dir.left)
                {
                    grid[entity.positionY - i,entity.positionX - j] = null;
                }
                if (player.direction == EntityInstance.dir.down)
                {
                    grid[entity.positionY - i,entity.positionX - j] = null;
                }
            }
        }

        for (int i = 0; i < entity.height; i++)
        {
            for (int j = 0; j < entity.width; j++)
            {
                if (player.direction == EntityInstance.dir.up)
                {
                    grid[posY + i,posX + j] = entity;
                }
                if (player.direction == EntityInstance.dir.right)
                {
                    grid[posY + i,posX + j] = entity;
                }
                if (player.direction == EntityInstance.dir.left)
                {
                    grid[posY - i,posX - j] = entity;
                }
                if (player.direction == EntityInstance.dir.down)
                {
                    grid[posY - i,posX - j] = entity;
                }
            }
        }
    }
    
    bool CanMove(EntityInstance entity, int newX, int newY)
    {
        for (int i = 0; i < entity.height; i++)
        {
            for (int j = 0; j < entity.width; j++)
            {
                if (player.direction == EntityInstance.dir.up)
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
                if (player.direction == EntityInstance.dir.right)
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
                if (player.direction == EntityInstance.dir.left)
                {
                    if (newY - i < 0 || newY - i > grid.GetLength(0) - 1 || newX - j < 0 ||
                        newX - j > grid.GetLength(1) - 1)
                    {
                        return false;
                    }

                    if (grid[newY - i, newX - j] != null && grid[newY - i, newX - j] != entity)
                    {
                        return false;
                    }
                }
                if (player.direction == EntityInstance.dir.down)
                {
                    if (newY - i < 0 || newY - i > grid.GetLength(0) - 1 || newX - j < 0 ||
                        newX - j > grid.GetLength(1) - 1)
                    {
                        return false;
                    }

                    if (grid[newY - i, newX - j] != null && grid[newY - i, newX - j] != entity)
                    {
                        return false;
                    }
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
                float r = Random.Range(0, 100);
                if (r >= attack.precision * 100)
                {
                    Debug.Log("loupé");
                    break;
                }
                r = Random.Range(0, 100);
                if (r <= attack.critChance*100)
                {
                    Debug.Log("crit !");
                    dmg = (int)(dmg * attack.critMultiplier);
                    Debug.Log(dmg);
                }
                if (entity == player)
                {
                    dmg = (int)(dmg * player.RespirationDatas[player.respirationIndex].damageMultiplier);
                }

                dmg = WeakPointDetection(tile,entity,attack, attackCordsX, attackCordsY, dmg);
                Damage(tile, dmg);
                lastEnnemyTouched = tile;
            }
            else
            {
                Debug.Log("cantAttack");
            }
        }
    }

    int WeakPointDetection(EntityInstance attackedEntity,EntityInstance attackerEntity,AttackData attack, int x, int y, int dmg)
    {
        foreach (var weakPoint in attackedEntity.weakPointList)
        {
            if (x == attackedEntity.positionX + weakPoint.posX && y == attackedEntity.positionY + weakPoint.posY)
            {
                if (weakPoint.direction == WeakPointData.dir.any)
                {
                    Debug.Log("any");
                    return (dmg * weakPoint.damageMulti);
                }
                if (weakPoint.direction == WeakPointData.dir.up)
                {
                    if (attackedEntity.positionY < attackerEntity.positionY+attack.Yoffset)
                    {
                        Debug.Log("up");
                        return (dmg * weakPoint.damageMulti);
                    }
                }
                if (weakPoint.direction == WeakPointData.dir.down)
                {
                    if (attackedEntity.positionY > attackerEntity.positionY+attack.Yoffset)
                    {
                        Debug.Log("down");
                        return (dmg * weakPoint.damageMulti);
                    }
                }
                if (weakPoint.direction == WeakPointData.dir.left)
                {
                    if (attackedEntity.positionX > attackerEntity.positionX+attack.Xoffset)
                    {
                        Debug.Log("left");
                        return (dmg * weakPoint.damageMulti);
                    }
                }
                if (weakPoint.direction == WeakPointData.dir.right)
                {
                    if (attackedEntity.positionX < attackerEntity.positionX+attack.Xoffset)
                    {
                        Debug.Log("right");
                        return (dmg * weakPoint.damageMulti);
                    }
                }
            }
        }
        return dmg;
    }

    EntityInstance GetAttackTile(EntityInstance entity, int dirY, int dirX, int actualRange, AttackData attack)
    {
        attackCordsY = GetOutTileY(entity, dirY, attack) + (actualRange - 1) * GetSign(dirY);
        attackCordsX = GetOutTileX(entity, dirX, attack) + (actualRange - 1) * GetSign(dirX);
        if (attackCordsY < 0 || attackCordsY >= grid.GetLength(0) || attackCordsX < 0 || attackCordsX >= grid.GetLength(1))
        {
            return null;
        }
        Debug.Log(attackCordsX);
        Debug.Log(attackCordsY);
        return grid[attackCordsY,attackCordsX];
    }
    
    int GetOutTileX(EntityInstance entity, int dirX, AttackData attack)
    {
        int Xoffset = attack.Xoffset;
        if (!entity.isStanding)
        {
            Xoffset = attack.Yoffset;
        }

        switch (entity.direction)
        {
            case EntityInstance.dir.up:
                return entity.positionX + GetSign(dirX) + entity.width * NegativeToZero(GetSign(dirX)) +
                       Xoffset * ZeroToOne(dirX) - NegativeToOne(dirX);
            case EntityInstance.dir.down:
                return entity.positionX + GetSign(dirX) + entity.width * PositiveToZero(GetSign(dirX)) -
                       Xoffset * ZeroToOne(dirX) + PositiveToOne(dirX);
            case EntityInstance.dir.left:
                return entity.positionX + GetSign(dirX) + entity.width * PositiveToZero(GetSign(dirX)) -
                    Xoffset * ZeroToOne(dirX) + PositiveToOne(dirX);
            case EntityInstance.dir.right:
                return entity.positionX + GetSign(dirX) + entity.width * NegativeToZero(GetSign(dirX)) +
                    Xoffset * ZeroToOne(dirX) - NegativeToOne(dirX);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    int GetOutTileY(EntityInstance entity, int dirY,AttackData attack)
    {
        int Yoffset = attack.Yoffset;
        if (!entity.isStanding)
        {
            Yoffset = attack.Xoffset;
        }

        switch (entity.direction)
        {
            case EntityInstance.dir.up:
                return entity.positionY + GetSign(dirY) + entity.height * NegativeToZero(GetSign(dirY)) +
                    Yoffset * ZeroToOne(dirY) - NegativeToOne(dirY);
            case EntityInstance.dir.down:
                return entity.positionY + GetSign(dirY) + entity.height * PositiveToZero(GetSign(dirY)) -
                    Yoffset * ZeroToOne(dirY) + PositiveToOne(dirY);
            case EntityInstance.dir.left:
                return entity.positionY + GetSign(dirY) + entity.height * NegativeToZero(GetSign(dirY)) +
                    Yoffset * ZeroToOne(dirY) - NegativeToOne(dirY);
            case EntityInstance.dir.right:
                return entity.positionY + GetSign(dirY) + entity.height * PositiveToZero(GetSign(dirY)) -
                    Yoffset * ZeroToOne(dirY) + PositiveToOne(dirY);
            default:
                throw new ArgumentOutOfRangeException();
        }
        
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
    int PositiveToZero(int value)
    {
        if (value > 0)
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
    int PositiveToOne(int value)
    {
        if (value > 0)
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
    int ZeroToMinusOne(int value)
    {
        if (value == 0)
        {
            return -1;
        }

        return 0;
    }
}