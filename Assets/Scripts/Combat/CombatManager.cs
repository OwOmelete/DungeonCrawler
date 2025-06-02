using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;
using DG.Tweening;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    [SerializeField] private DeathManager deathManagerReference;
    [SerializeField] private GameObject Booster;
    [SerializeField] private SpriteRenderer[] LifeBarPlayer;
    [SerializeField] private SpriteRenderer[] LifeBarPlayerEmpty;
    [SerializeField] private SpriteRenderer[] LifeBarEnnemy1;
    [SerializeField] private SpriteRenderer[] LifeBarEnnemy1Empty;
    [SerializeField] private SpriteRenderer[] LifeBarEnnemy2;
    [SerializeField] private SpriteRenderer[] LifeBarEnnemy2Empty;
    [SerializeField] private Animator[] LifeBarAnimators;
    [SerializeField] private Sprite pvHit;
    [SerializeField] private float pvHitFlashDelay;
    [SerializeField] private GameObject MovePrevisu;
    [SerializeField] private GameObject AtkPrevisu;
    [SerializeField] private GameObject RotatePrevisu;
    [SerializeField] private GameObject Selection;
    [SerializeField] private SpriteRenderer Ennemy1Icon;
    [SerializeField] private SpriteRenderer Ennemy2Icon;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private SpriteRenderer[] buttons;
    [SerializeField] private TMP_Text[] textButtons;
    private List<Fish> fishes = new List<Fish>();
    [HideInInspector] public EntityInstance[,] grid;
    private List<EntityInstance> turnOrder = new List<EntityInstance>();
    public bool combatFinished;
    private int currentTurnIndex = 0;
    private bool isPlaying;
    private int attackCordsX;
    private int attackCordsY;
    private SpriteRenderer playerEntityRenderer;
    private Transform playerEntityChild;
    private bool canRotate = true;
    private bool hasAttacked = false;
    private bool hasMoved = false;
    public GameObject GGBarGO;
    public GameObject brotuloBarGO;
    private EntityInstance Ennemy1;
    private EntityInstance Ennemy2;
    private Dictionary<Vector2Int, List<Vector2Int>> voisins;
    private List<GameObject> previsuList = new List<GameObject>();
    private List<GameObject> selectionList = new List<GameObject>();
    private List<Tuple<int, int>> upPrevisuList = new List<Tuple<int, int>>();
    private List<Tuple<int, int>> downPrevisuList = new List<Tuple<int, int>>();
    private List<Tuple<int, int>> leftPrevisuList = new List<Tuple<int, int>>();
    private List<Tuple<int, int>> rightPrevisuList = new List<Tuple<int, int>>();
    public static CombatManager Instance;
    
    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
    }

    public void InitCombat()
    {
        
        combatFinished = false;
        grid = CreateGrid(gridHeight, gridWidth);
        foreach (var i in getNeighbors(2, 2))
        {
            Debug.Log(i);
        }
        SpawnEntitys();
        playerLight = player.prefab.transform.GetChild(0).GetComponent<Light2D>();
        UpdateLight();
        Booster.SetActive(player.booster);

        ///////// TESTS //////////
        
    }
    

    void UpdateLight()
    {
        playerLight.pointLightOuterRadius = player.light;
        playerLight.pointLightInnerRadius = player.light / 2;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    Debug.Log(grid[i,j]);
                }
            }
        }
        if (combatFinished)
        {
            EndFight();
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
        if (Input.GetKeyDown(KeyCode.E))
        {
            EndFight();
        }
    }


    private void EndTurn()
    {
        if (turnOrder.Count == 0)
        {
            return;
        }
        currentTurnIndex = (currentTurnIndex + 1) % turnOrder.Count;
        if (currentTurnIndex % turnOrder.Count == 0)
        {
            player.light -= player.lightLostPerTurn;
            player.light = Mathf.Clamp(player.light, 1, 10);
            UpdateLight();
            hasMoved = false;
        }
    }

    void PlayerTurn(PlayerDataInstance playerEntity)
    {
        Action(playerEntity);
        /*playerEntityRenderer.flipX = playerEntity.direction == EntityInstance.dir.upLeft ||
                                     playerEntity.direction == EntityInstance.dir.downRight;
        playerEntityRenderer.flipY = playerEntity.direction == EntityInstance.dir.rightUp ||
                                     playerEntity.direction == EntityInstance.dir.leftDown;*/
        
        if (playerEntity.actionPoint <= 0)
        {
            hasAttacked = false;
            hasMoved = false;
            Debug.Log("player oxygen : " +player.oxygen);
            Debug.Log("player light : " + player.light);
            EndTurn();
            //player.oxygen -= player.RespirationDatas[player.respirationIndex].oxygenLoss;
            Debug.Log("plus d'action point");
            //Debug.Log("oxygen : " + player.oxygen);
            playerEntity.actionPoint = playerEntity.RespirationDatas[playerEntity.respirationIndex].actionPoints;
        }
    }
    
    void IATurn(EntityInstance entity)
    {
        //Action(entity);
        if (entity is FishDataInstance)
        {
            FishDataInstance fish = entity as FishDataInstance;
            entity.behaviour.ManageTurn(fish);
        }
        else
        {
            Debug.Log("pas poisson :(");
        }
        EndTurn();
        Debug.Log("tour fini");
    }

    /*
    void ActionKeyboard(PlayerDataInstance playerEntity)
    {
        int verticalSpeed = 1;
        int horizontalSpeed = 1;
        int actionPointLost = 0;
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
        

        if (Input.GetKeyDown(KeyCode.W))
        {
            player.booster = !player.booster;
            Debug.Log("booster = " + player.booster);
            actionPointLost = 0;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && !hasAttacked && !hasMoved)
        {
            if (player.direction == EntityInstance.dir.up)
            {
                if (Move(playerEntity, playerEntity.positionX, playerEntity.positionY + verticalSpeed)) player.oxygen -= player.oxygenLostMove2Tiles;
                else
                {
                    Move(playerEntity, playerEntity.positionX, playerEntity.positionY + 1);
                    player.oxygen -= player.oxygenLostMove;
                }
            }
            else
            {
                Move(playerEntity, playerEntity.positionX, playerEntity.positionY + 1);
                player.oxygen -= player.oxygenLostMove;
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)&& !hasAttacked && !hasMoved)
        {
            if (player.direction == EntityInstance.dir.down)
            {
                if(Move(playerEntity, playerEntity.positionX, playerEntity.positionY - verticalSpeed)) player.oxygen -= player.oxygenLostMove2Tiles;
                else
                {
                    Move(playerEntity, playerEntity.positionX, playerEntity.positionY - 1);
                    player.oxygen -= player.oxygenLostMove;
                }
            }
            else
            {
                Move(playerEntity, playerEntity.positionX, playerEntity.positionY - 1);
                player.oxygen -= player.oxygenLostMove;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)&& !hasAttacked && !hasMoved)
        {
            if (player.direction == EntityInstance.dir.right)
            {
                if(Move(playerEntity, playerEntity.positionX + horizontalSpeed, playerEntity.positionY)) player.oxygen -= player.oxygenLostMove2Tiles;
                else
                {
                    Move(playerEntity, playerEntity.positionX + 1, playerEntity.positionY);
                    player.oxygen -= player.oxygenLostMove;
                }
            }
            else
            {
                Move(playerEntity, playerEntity.positionX + 1, playerEntity.positionY);
                player.oxygen -= player.oxygenLostMove;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)&& !hasAttacked && !hasMoved)
        {
            if (player.direction == EntityInstance.dir.left)
            {
                if(Move(playerEntity, playerEntity.positionX - horizontalSpeed, playerEntity.positionY)) player.oxygen -= player.oxygenLostMove2Tiles;
                else
                {
                    Move(playerEntity, playerEntity.positionX - 1, playerEntity.positionY);
                    player.oxygen -= player.oxygenLostMove;
                }
            }
            else
            {
                Move(playerEntity, playerEntity.positionX - 1, playerEntity.positionY);
                player.oxygen -= player.oxygenLostMove;
            }
        }
        
        else if (Input.GetKeyDown(KeyCode.I)&& !hasAttacked)
        {
            if (Attack(player.currentAttack, player, player.positionX, player.positionY + 1)) actionPointLost = 1;
            else hasAttacked = false;
        }
        else if (Input.GetKeyDown(KeyCode.K)&& !hasAttacked)
        {
            if(Attack(player.currentAttack, player, player.positionX, player.positionY - 1)) actionPointLost = 1;
            else hasAttacked = false;
        }
        else if (Input.GetKeyDown(KeyCode.L)&& !hasAttacked)
        {
            if (Attack(player.currentAttack, player, player.positionX + 1, player.positionY)) actionPointLost = 1;
            else hasAttacked = false;
        }
        else if (Input.GetKeyDown(KeyCode.J)&& !hasAttacked)
        {
            if (Attack(player.currentAttack, player, player.positionX - 1, player.positionY)) actionPointLost = 1;
            else hasAttacked = false;
        }
        else if (Input.GetKeyDown(KeyCode.E) && canRotate && !hasMoved)
        {
            if (canTurnRight(player))
            {
                FlipPlayerRight(player);
                hasMoved = true;
                player.oxygen -= player.oxygenLostRotate;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Q) && canRotate && !hasMoved)
        {
            if (canTurnLeft(player))
            {
                FlipPlayerLeft(player);
                hasMoved = true;
                player.oxygen -= player.oxygenLostRotate;
            }
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
        playerEntity.actionPoint -= actionPointLost;
    }
    */
    
    bool ApproximatelyEqual(float a, float b, float e = 0.1f)
    {
        return Mathf.Abs(a - b) < e;
    }

    void ShowPrevisu(GameObject obj, int x, int y)
    {
        GameObject p = Instantiate(obj, new Vector3(x, y, 0), Quaternion.identity);
        previsuList.Add(p);
        if (x > player.positionX && x > SecondaryPlayerCoordsX())
        {
            rightPrevisuList.Add(new Tuple<int, int>(x,y));
        }
        if (x < player.positionX && x < SecondaryPlayerCoordsX())
        {
            leftPrevisuList.Add(new Tuple<int, int>(x,y));
        }
        if (y < player.positionY && y < SecondaryPlayerCoordsY())
        {
            downPrevisuList.Add(new Tuple<int, int>(x,y));
        }
        if (y > player.positionY && y > SecondaryPlayerCoordsY())
        {
            upPrevisuList.Add(new Tuple<int, int>(x,y));
        }
    }

    void ShowMovements()
    {
        ResetPrevisuList();
        foreach (var i in getNeighbors(player.positionX, player.positionY))
        {
            if (grid[i.Item2, i.Item1] == null || grid[i.Item2, i.Item1] is SpikeInstance)
            {
                ShowPrevisu(MovePrevisu, i.Item1, i.Item2);
            }
        }
        foreach (var i in getNeighbors(SecondaryPlayerCoordsX(), SecondaryPlayerCoordsY()))
        {
            if (grid[i.Item2, i.Item1] == null || grid[i.Item2, i.Item1] is SpikeInstance)
            {
                ShowPrevisu(MovePrevisu, i.Item1, i.Item2);
            }
        }

        if (player.booster)
        {
                switch (player.direction)
            {
                case EntityInstance.dir.up:
                    if (isTileInGrid(player.positionX, player.positionY + 3))
                    {
                        if ((grid[player.positionY + 3, player.positionX] == null || grid[player.positionY + 3, player.positionX]is SpikeInstance)
                            && (grid[player.positionY + 2, player.positionX] == null ||grid[player.positionY + 2, player.positionX] is SpikeInstance ))
                        {
                            ShowPrevisu(MovePrevisu, player.positionX,player.positionY+3);
                        }
                    }
                    break;
                case EntityInstance.dir.down:
                    if (isTileInGrid(player.positionX, player.positionY - 3))
                    {
                        if ((grid[player.positionY - 3, player.positionX] == null || grid[player.positionY - 3, player.positionX] is SpikeInstance)
                            && (grid[player.positionY - 2, player.positionX] == null || grid[player.positionY - 2, player.positionX] is SpikeInstance))
                        {
                            ShowPrevisu(MovePrevisu, player.positionX,player.positionY-3);
                        }
                    }
                    break;
                case EntityInstance.dir.left:
                    if (isTileInGrid(player.positionX - 3, player.positionY))
                    {
                        if ((grid[player.positionY, player.positionX - 3] == null || grid[player.positionY, player.positionX - 3] is SpikeInstance)
                            && (grid[player.positionY, player.positionX - 2] == null || grid[player.positionY, player.positionX - 2] is SpikeInstance))
                        {
                            ShowPrevisu(MovePrevisu, player.positionX-3,player.positionY);
                        }
                    }
                    break;
                case EntityInstance.dir.right:
                    if (isTileInGrid(player.positionX + 3, player.positionY))
                    {
                        if ((grid[player.positionY, player.positionX + 3] == null || grid[player.positionY, player.positionX + 3] is SpikeInstance)
                            && (grid[player.positionY, player.positionX + 2] == null || grid[player.positionY, player.positionX + 2]is SpikeInstance))
                        {
                            ShowPrevisu(MovePrevisu, player.positionX+3,player.positionY);
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        
    }

    void ShowRotations()
    {
        ResetPrevisuList();
        if (player.direction == EntityInstance.dir.down || player.direction == EntityInstance.dir.up)
        {
            if (isTileInGrid(player.positionX + 1, player.positionY))
            {
                if (grid[player.positionY, player.positionX + 1] == null || grid[player.positionY, player.positionX + 1] is  SpikeInstance)
                {
                    ShowPrevisu(RotatePrevisu, player.positionX+1,player.positionY);
                }
            }
            if (isTileInGrid(player.positionX - 1, player.positionY))
            {
                if (grid[player.positionY, player.positionX - 1] == null || grid[player.positionY, player.positionX - 1] is  SpikeInstance)
                {
                    ShowPrevisu(RotatePrevisu, player.positionX-1,player.positionY);
                }
            }
        }
        if (player.direction == EntityInstance.dir.left || player.direction == EntityInstance.dir.right)
        {
            if (isTileInGrid(player.positionX, player.positionY + 1))
            {
                if (grid[player.positionY + 1, player.positionX] == null || grid[player.positionY + 1, player.positionX] is  SpikeInstance)
                {
                    ShowPrevisu(RotatePrevisu, player.positionX,player.positionY+1);
                }
            }
            if (isTileInGrid(player.positionX, player.positionY - 1))
            {
                if (grid[player.positionY - 1, player.positionX] == null || grid[player.positionY - 1, player.positionX] is  SpikeInstance)
                {
                    ShowPrevisu(RotatePrevisu, player.positionX,player.positionY-1);
                }
            }
        }
    }

    void ShowAttacks()
    {
        ResetPrevisuList();
        foreach (var i in getNeighbors(SecondaryPlayerCoordsX(), SecondaryPlayerCoordsY()))
        {
            if (grid[i.Item2, i.Item1] != player)
            {
                ShowPrevisu(AtkPrevisu, i.Item1, i.Item2);
            }
        }
    }

    void ShowSelection(List<Tuple<int, int>> list)
    {
        ResetSelectionList();
        foreach (var coords in list)
        {
            GameObject s = Instantiate(Selection, new Vector3(coords.Item1, coords.Item2, 0), Quaternion.identity);
            selectionList.Add(s);
        }
    }
    
    void ResetSelectionList()
    {
        for (int i = selectionList.Count - 1; i >= 0; i--)
        {
            Destroy(selectionList[i]);
        }
        selectionList.Clear();
    }

    void ResetPrevisuList()
    {
        rightPrevisuList.Clear();
        leftPrevisuList.Clear();
        downPrevisuList.Clear();
        upPrevisuList.Clear();
        for (int i = previsuList.Count - 1; i >= 0; i--)
        {
            Destroy(previsuList[i]);
        }
        previsuList.Clear();
    }

    private bool wantToAttack;
    private bool wantToRotate;
    private bool wantToMove = true;
    private bool waitForConfirm;
    private bool previsuShown = false;
    private float joysticXSave = 0f; 
    private float joysticYSave = 0f; 
    void Action(PlayerDataInstance playerEntity)
    {
        float joysticX = Input.GetAxis("Horizontal");
        float joysticY = Input.GetAxis("Vertical");
        int verticalSpeed = 1;
        int horizontalSpeed = 1;
        int actionPointLost = 0;
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

        if (wantToMove && !hasMoved && !previsuShown)
        {
            previsuShown = true;
            ShowMovements();
        }

        if (wantToAttack && !hasAttacked&& !previsuShown)
        {
            previsuShown = true;
            ShowAttacks();
        }

        if (wantToRotate  && !hasMoved&& !previsuShown)
        {
            previsuShown = true;
            ShowRotations();
        }
        
        Booster.SetActive(player.booster);
        
        // BOOSTER
        if (Input.GetKeyDown("joystick button 5"))
        {
            player.booster = !player.booster;
            Debug.Log("booster = " + player.booster);
            actionPointLost = 0;
            Booster.SetActive(player.booster);
            if (wantToMove && !hasMoved)
            {
                ShowMovements();
            }
        }

        if (Input.GetKeyDown("joystick button 2") && !hasMoved)
        {
            wantToRotate = false;
            wantToMove = true;
            wantToAttack = false;
            waitForConfirm = false;
            
            Debug.Log("Want to move");
            
            ShowMovements();
            ResetSelectionList();
            joysticXSave = 0;
            joysticYSave = 0;
            
        }
        else if (Input.GetKeyDown("joystick button 3")  && !hasMoved)
        {
            wantToRotate = true;
            wantToMove = false;
            wantToAttack = false;
            waitForConfirm = false;
            Debug.Log("Want to rotate");
            
            ShowRotations();
            ResetSelectionList();
            joysticXSave = 0;
            joysticYSave = 0;
        }
        else if (Input.GetKeyDown("joystick button 1"))
        {
            wantToRotate = false;
            wantToMove = false;
            wantToAttack = true;
            waitForConfirm = false;
            Debug.Log("Want to attack");
            
            ShowAttacks();
            ResetSelectionList();
            joysticXSave = 0;
            joysticYSave = 0;
        }
        else if (Input.GetKeyDown("joystick button 4"))
        {
            Debug.Log("passer son tour");
            playerEntity.actionPoint = 0;
            ResetPrevisuList();
            ResetSelectionList();
            joysticXSave = 0;
            joysticYSave = 0;
            previsuShown = false;
            return;
        }
        
        
        CheckJoystic(joysticY, joysticX);
        
        if (Input.GetKeyDown("joystick button 0")&& (joysticXSave != 0 || joysticYSave != 0))
        {
            #region Move
        
            // MOVE
            if (wantToMove)
            {
                if (joysticYSave == 1 && !hasAttacked && !hasMoved )
                {
                    if (player.direction == EntityInstance.dir.up)
                    {
                        if (Move(playerEntity, playerEntity.positionX, playerEntity.positionY + verticalSpeed))
                            player.oxygen -= player.oxygenLostMove2Tiles;
                        else
                        {
                            Move(playerEntity, playerEntity.positionX, playerEntity.positionY + 1);
                            player.oxygen -= player.oxygenLostMove;
                        }
                    }
                    else
                    {
                        Move(playerEntity, playerEntity.positionX, playerEntity.positionY + 1);
                        player.oxygen -= player.oxygenLostMove;
                    }
                    waitForConfirm = false;
                    endAction();
                }
                else if (joysticYSave == -1 && !hasAttacked && !hasMoved)
                {
                    if (player.direction == EntityInstance.dir.down)
                    {
                        if (Move(playerEntity, playerEntity.positionX, playerEntity.positionY - verticalSpeed))
                            player.oxygen -= player.oxygenLostMove2Tiles;
                        else
                        {
                            Move(playerEntity, playerEntity.positionX, playerEntity.positionY - 1);
                            player.oxygen -= player.oxygenLostMove;
                        }
                    }
                    else
                    {
                        Move(playerEntity, playerEntity.positionX, playerEntity.positionY - 1);
                        player.oxygen -= player.oxygenLostMove;
                    }
                    waitForConfirm = false;
                    endAction();
                }
                else if (joysticXSave == 1 && !hasAttacked && !hasMoved)
                {
                    if (player.direction == EntityInstance.dir.right)
                    {
                        if (Move(playerEntity, playerEntity.positionX + horizontalSpeed, playerEntity.positionY))
                            player.oxygen -= player.oxygenLostMove2Tiles;
                        else
                        {
                            Move(playerEntity, playerEntity.positionX + 1, playerEntity.positionY);
                            player.oxygen -= player.oxygenLostMove;
                        }
                    }
                    else
                    {
                        Move(playerEntity, playerEntity.positionX + 1, playerEntity.positionY);
                        player.oxygen -= player.oxygenLostMove;
                    }
                    waitForConfirm = false;
                    endAction();
                }
                else if (joysticXSave == - 1 && !hasAttacked && !hasMoved)
                {
                    if (player.direction == EntityInstance.dir.left)
                    {
                        if (Move(playerEntity, playerEntity.positionX - horizontalSpeed, playerEntity.positionY))
                            player.oxygen -= player.oxygenLostMove2Tiles;
                        else
                        {
                            Move(playerEntity, playerEntity.positionX - 1, playerEntity.positionY);
                            player.oxygen -= player.oxygenLostMove;
                        }
                    }
                    else
                    {
                        Move(playerEntity, playerEntity.positionX - 1, playerEntity.positionY);
                        player.oxygen -= player.oxygenLostMove;
                    }
                    waitForConfirm = false;
                    endAction();
                }
                else
                {
                    return;
                }
                
            }  
            #endregion
            
            #region Attack
            // ATTACK
            
            else if (wantToAttack && !hasAttacked)
            {
                if (joysticYSave == 1 && !hasAttacked)
                {
                    if (Attack(player.currentAttack, player, player.positionX, player.positionY + 1))
                    {
                        actionPointLost = 1; 
                        waitForConfirm = false;
                        endAction();
                    }
                        
                    else hasAttacked = false;
                }
                else if (joysticYSave == - 1 && !hasAttacked)
                {
                    if (Attack(player.currentAttack, player, player.positionX, player.positionY - 1))
                    {
                        actionPointLost = 1;
                        waitForConfirm = false;
                        endAction();
                    }
                    else hasAttacked = false;
                    
                }
                else if (joysticXSave == 1 && !hasAttacked)
                {
                    if (Attack(player.currentAttack, player, player.positionX + 1, player.positionY))
                    {
                        actionPointLost = 1;
                        waitForConfirm = false;
                        endAction();
                    }
                    else hasAttacked = false;
                }
                else if (joysticXSave == - 1&& !hasAttacked)
                {
                    if (Attack(player.currentAttack, player, player.positionX - 1, player.positionY))
                    {
                        actionPointLost = 1;
                        waitForConfirm = false;
                        endAction();
                    }
                    else hasAttacked = false;
                }
                else
                {
                    return;
                }
            }
            #endregion
            
            #region Rotate
            // ROTATE
           else if (wantToRotate)
            {
                switch (player.direction)
                {
                    case EntityInstance.dir.up:
                        if (joysticXSave == 1 && canRotate && !hasMoved)
                        {
                            if (canTurnRight(player))
                            {
                                FlipPlayerRight(player);
                                hasMoved = true;
                                player.oxygen -= player.oxygenLostRotate;
                                waitForConfirm = false;
                                endAction();
                            }
                        }
                        else if (joysticXSave == - 1 && canRotate && !hasMoved)
                        {
                            if (canTurnLeft(player))
                            {
                                FlipPlayerLeft(player);
                                hasMoved = true;
                                player.oxygen -= player.oxygenLostRotate;
                                waitForConfirm = false;
                                endAction();
                            }
                        }
                        else
                        {
                            return;
                        }
                        break;
                    case EntityInstance.dir.down:
                        if (joysticXSave == -1 && canRotate && !hasMoved)
                        {
                            if (canTurnRight(player))
                            {
                                FlipPlayerRight(player);
                                hasMoved = true;
                                player.oxygen -= player.oxygenLostRotate;
                                waitForConfirm = false;
                                endAction();
                            }
                        }
                        else if (joysticXSave == 1 && canRotate && !hasMoved)
                        {
                            if (canTurnLeft(player))
                            {
                                FlipPlayerLeft(player);
                                hasMoved = true;
                                player.oxygen -= player.oxygenLostRotate;
                                waitForConfirm = false;
                                endAction();
                            }
                        }
                        else
                        {
                            return;
                        }
                        break;
                    case EntityInstance.dir.left:
                        if (joysticYSave == 1 && canRotate && !hasMoved)
                        {
                            if (canTurnRight(player))
                            {
                                FlipPlayerRight(player);
                                hasMoved = true;
                                player.oxygen -= player.oxygenLostRotate;
                                waitForConfirm = false;
                                endAction();
                            }
                        }
                        else if (joysticYSave == -1 && canRotate && !hasMoved)
                        {
                            if (canTurnLeft(player))
                            {
                                FlipPlayerLeft(player);
                                hasMoved = true;
                                player.oxygen -= player.oxygenLostRotate;
                                waitForConfirm = false;
                                endAction();
                            }
                        }
                        else
                        {
                            return;
                        }
                        break;
                    case EntityInstance.dir.right:
                        if (joysticYSave == -1 && canRotate && !hasMoved)
                        {
                            if (canTurnRight(player))
                            {
                                FlipPlayerRight(player);
                                hasMoved = true;
                                player.oxygen -= player.oxygenLostRotate;
                                waitForConfirm = false;
                                endAction();
                            }
                        }
                        else if (joysticYSave == 1 && canRotate && !hasMoved)
                        {
                            if (canTurnLeft(player))
                            {
                                FlipPlayerLeft(player);
                                hasMoved = true;
                                player.oxygen -= player.oxygenLostRotate;
                                waitForConfirm = false;
                                endAction();
                            }
                        }
                        else
                        {
                            return;
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            #endregion
        }
        else if (joysticXSave == 0 && joysticYSave == 0)
        {
            waitForConfirm = false;
        }

        if (hasMoved)
        {
            for (int i = 0; i < 2; i++)
            {
                buttons[i].color = new Color(115f / 255f,115f/ 255f,115f/ 255f);
                textButtons[i].color = new Color(126f / 255f, 92f / 255f, 0);
            }
        }
        else
        {
            for (int i = 0; i < 2; i++)
            {
                buttons[i].color = new Color(255f/ 255f,255f/ 255f,255f/ 255f);
                textButtons[i].color = new Color(255f/ 255f, 183f/ 255f, 0);
            }
        }
        #endregion
        
        playerEntity.actionPoint -= actionPointLost;
    }

    void endAction()
    {
        ResetPrevisuList();
        ResetSelectionList();
        joysticXSave = 0;
        joysticYSave = 0;
    }

    void CheckJoystic(float joysticY, float joysticX)
    {
        if (ApproximatelyEqual(joysticY, 1) && !hasAttacked)
        {
            joysticYSave = 0;
            joysticXSave = 0;
            joysticYSave = 1;
            waitForConfirm = true;
            ShowSelection(upPrevisuList);
        }
        else if (ApproximatelyEqual(joysticY, - 1)&& !hasAttacked)
        {
            joysticYSave = 0;
            joysticXSave = 0;
            joysticYSave = - 1;
            waitForConfirm = true;
            ShowSelection(downPrevisuList);
        }
        else if (ApproximatelyEqual(joysticX, 1) && !hasAttacked)
        {
            joysticYSave = 0;
            joysticXSave = 0;
            joysticXSave = 1;
            waitForConfirm = true;
            ShowSelection(rightPrevisuList);
        }
        else if (ApproximatelyEqual(joysticX, - 1)&& !hasAttacked)
        {
            joysticYSave = 0;
            joysticXSave = 0;
            joysticXSave = - 1;
            waitForConfirm = true;
            ShowSelection(leftPrevisuList);
        }
    }
    
    
    public void FlipPlayerRight(EntityInstance entity)
    {
        if (canTurnRight(entity))
        {
            if (entity.direction == EntityInstance.dir.left && (grid[entity.positionY + 1 , entity.positionX] == null ||grid[entity.positionY +1 , entity.positionX] == entity || grid[entity.positionY + 1 , entity.positionX] is SpikeInstance ))
            {
                DoRotateRight(entity);
            }
            else if (entity.direction == EntityInstance.dir.up && (grid[entity.positionY , entity.positionX+1] == null||grid[entity.positionY, entity.positionX+1] == entity || grid[entity.positionY , entity.positionX+1] is SpikeInstance))
            {
                DoRotateRight(entity);            
            }
            else if (entity.direction == EntityInstance.dir.right && (grid[entity.positionY -1 , entity.positionX] == null ||grid[entity.positionY -1 , entity.positionX] == entity || grid[entity.positionY - 1 , entity.positionX] is SpikeInstance))
            {
                DoRotateRight(entity);            
            }
            else if (entity.direction == EntityInstance.dir.down && (grid[entity.positionY, entity.positionX-1] == null||grid[entity.positionY , entity.positionX-1] == entity || grid[entity.positionY , entity.positionX-1] is SpikeInstance))
            {
                DoRotateRight(entity);            
            }
        }
    }
    public void FlipPlayerLeft(EntityInstance entity)
    {
        if (canTurnLeft(entity)) 
        {
            if (entity.direction == EntityInstance.dir.left && (grid[entity.positionY - 1, entity.positionX] == null ||grid[entity.positionY -1 , entity.positionX] == entity || grid[entity.positionY - 1 , entity.positionX] is SpikeInstance))
            {
                DoRotateLeft(entity);
            }

            else if (entity.direction == EntityInstance.dir.up && (grid[entity.positionY , entity.positionX-1] == null||grid[entity.positionY, entity.positionX-1] == entity || grid[entity.positionY , entity.positionX-1] is SpikeInstance))
            {
                DoRotateLeft(entity);
            }
            
            else if (entity.direction == EntityInstance.dir.right && (grid[entity.positionY +1 , entity.positionX] == null ||grid[entity.positionY +1 , entity.positionX] == entity || grid[entity.positionY + 1 , entity.positionX] is SpikeInstance ))
            {
                DoRotateLeft(entity);
            }
            
            else if (entity.direction == EntityInstance.dir.down && (grid[entity.positionY, entity.positionX+1] == null||grid[entity.positionY , entity.positionX+1] == entity|| grid[entity.positionY , entity.positionX+1] is SpikeInstance))
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
                if (grid[entity.positionY + 1, entity.positionX] is SpikeInstance)
                {
                    SpikeInstance spike = grid[entity.positionY + 1, entity.positionX] as SpikeInstance;
                    Damage(entity,spike.hp);
                    spike.entity.spikeList.Remove(spike);
                    Destroy(spike.prefab);
                }
                grid[entity.positionY + 1, entity.positionX] = player;
                entity.height = entity.width;
                entity.width = tempHeight;
                entity.isStanding = true;
                break;
            case EntityInstance.dir.down:
                if (grid[entity.positionY - 1, entity.positionX] is SpikeInstance)
                {
                    SpikeInstance spike = grid[entity.positionY - 1, entity.positionX] as SpikeInstance;
                    Damage(entity,spike.hp);
                    spike.entity.spikeList.Remove(spike);
                    Destroy(spike.prefab);
                }
                grid[entity.positionY - 1, entity.positionX] = player;
                entity.height = entity.width;
                entity.width = tempHeight;
                entity.isStanding = true;
                break;
            case EntityInstance.dir.left:
                if (grid[entity.positionY, entity.positionX - 1] is SpikeInstance)
                {
                    SpikeInstance spike = grid[entity.positionY, entity.positionX - 1] as SpikeInstance;
                    Damage(entity,spike.hp);
                    spike.entity.spikeList.Remove(spike);
                    Destroy(spike.prefab);
                }
                grid[entity.positionY, entity.positionX - 1] = player;
                entity.height = entity.width;
                entity.width = tempHeight;
                entity.isStanding = false;
                break;
            case EntityInstance.dir.right:
                if (grid[entity.positionY, entity.positionX + 1] is SpikeInstance)
                {
                    SpikeInstance spike = grid[entity.positionY, entity.positionX + 1] as SpikeInstance;
                    Damage(entity,spike.hp);
                    spike.entity.spikeList.Remove(spike);
                    Destroy(spike.prefab);
                }
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
        Ennemy1Icon.enabled = false;
        Ennemy2Icon.enabled = false;
        Debug.Log(player.prefab);
        player.prefab = Instantiate(_playerData.prefab,
                new Vector3(player.positionX, player.positionY, 0),quaternion.identity);
        playerEntityRenderer = player.prefab.GetComponentInChildren<SpriteRenderer>();
        player.entityChild = player.prefab.transform.GetChild(0);
        turnOrder.Add(player);
        player.isStanding = true;
        for (int i = 0; i < _playerData.hp; i++)
        {
            LifeBarPlayerEmpty[i].enabled = true;
        }
        UpdateLifeBar(player,false);  
        for (int i = 0; i < _fishDatas.Count; i++)    
        {
            Fish newFish = new Fish
            {
                fishData = _fishDatas[i],
                fishDataInstance = (FishDataInstance)_fishDatas[i].Instance()
            };
            fishes.Add(newFish);
            turnOrder.Add(newFish.fishDataInstance);
            if (i == 0)
            {
                Ennemy1 = newFish.fishDataInstance;
                for (int j = 0; j < newFish.fishData.hp; j++)
                {
                    LifeBarEnnemy1Empty[j].enabled = true;
                }
                Ennemy1Icon.enabled = true;
                Ennemy1Icon.sprite = newFish.fishData.uiSprite;
                UpdateLifeBar(Ennemy1,false);
            }
            else if (i == 1)
            {
                Ennemy2 = newFish.fishDataInstance;
                for (int j = 0; j < newFish.fishData.hp; j++)
                {
                    LifeBarEnnemy2Empty[j].enabled = true;
                }
                Ennemy2Icon.enabled = true;
                Ennemy2Icon.sprite = newFish.fishData.uiSprite;
                UpdateLifeBar(Ennemy2,false);
            }
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
                    Debug.Log(fish.fishDataInstance.name);
                    Debug.Log(fish.fishDataInstance.direction);
                }
            }
            fish.fishDataInstance.prefab = Instantiate(fish.fishData.prefab,
                new Vector3(fish.fishData.positionX,
                    fish.fishData.positionY, 0),quaternion.identity);
            fish.fishDataInstance.entityChild = fish.fishDataInstance.prefab.transform.GetChild(0);
            fish.fishDataInstance.behaviour = fish.fishDataInstance.prefab.GetComponent<AbstractIA>();
            if (fish.fishDataInstance.behaviour is SpikeBallBehaviour)
            {
                FishDataInstance brotulo = fish.fishDataInstance;
                SpikeBallBehaviour IAref = fish.fishDataInstance.behaviour as SpikeBallBehaviour;
                Debug.Log(fish.fishData.startingDirection);
                switch (fish.fishData.startingDirection)
                {
                    case Entity.dir.up:
                        brotulo.weakPointList.Clear();
                        brotulo.direction = EntityInstance.dir.up;
                        brotulo.weakPointList.Add(brotulo.WeakPointsUp[0]);
                        brotulo.entityChild.transform.rotation = Quaternion.Euler(0, 0,0);
                        break;
                    case Entity.dir.down:
                        brotulo.weakPointList.Clear();
                        brotulo.direction = EntityInstance.dir.down;
                        brotulo.weakPointList.Add(brotulo.WeakPointsDown[0]);
                        brotulo.entityChild.transform.rotation = Quaternion.Euler(0, 0,180);
                        break;
                    case Entity.dir.left:
                        brotulo.weakPointList.Clear();
                        brotulo.direction = EntityInstance.dir.left;
                        brotulo.weakPointList.Add(brotulo.WeakPointsLeft[0]);
                        brotulo.entityChild.transform.rotation = Quaternion.Euler(0, 0,90);
                        break;
                    case Entity.dir.right:
                        brotulo.weakPointList.Clear();
                        brotulo.direction = EntityInstance.dir.right;
                        brotulo.weakPointList.Add(brotulo.WeakPointsRight[0]);
                        brotulo.entityChild.transform.rotation = Quaternion.Euler(0, 0,-90);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                brotulo.weakPointList.Add(IAref.DamageToAttacker);
                
            }

            else if (fish.fishDataInstance.behaviour is GrandGouzBehaviour)
            {
                fish.fishDataInstance.sr = fish.fishDataInstance.prefab.GetComponentInChildren<SpriteRenderer>();
                Debug.Log(fish.fishDataInstance.sr);
                GrandGouzBehaviour IAref = fish.fishDataInstance.behaviour as GrandGouzBehaviour;
                Debug.Log(fish.fishData.startingDirection);
                if (fish.fishData.startingDirection == Entity.dir.left)
                {
                    IAref.Flipping(fish.fishDataInstance);
                }
            }
        }
    }

    public bool Move(EntityInstance entity, int posX, int posY)
    {
        if(!CanMove(entity, posX, posY))
        { 
            return false;
        }
        if (entity == player)
        {
            hasMoved = true;
        }
        UpdateGrid(entity, posX, posY);
        entity.prefab.transform.DOMove(new Vector3(posX, posY, 0), moveDuration).SetEase(Ease.InOutCubic);
        entity.positionX = posX;
        entity.positionY = posY;
        return true;
    }

    void UpdateGrid(EntityInstance entity, int posX, int posY)
    {
        for (int i = 0; i < entity.height; i++)
        {
            for (int j = 0; j < entity.width; j++)
            {
                if (entity.direction == EntityInstance.dir.up)
                {
                    grid[entity.positionY + i,entity.positionX + j] = null;
                }
                if (entity.direction == EntityInstance.dir.right)
                {
                    grid[entity.positionY + i,entity.positionX + j] = null;
                }
                if (entity.direction == EntityInstance.dir.left)
                {
                    grid[entity.positionY - i,entity.positionX - j] = null;
                }
                if (entity.direction == EntityInstance.dir.down)
                {
                    grid[entity.positionY - i,entity.positionX - j] = null;
                }
            }
        }

        for (int i = 0; i < entity.height; i++)
        {
            for (int j = 0; j < entity.width; j++)
            {
                //Debug.Log(entity.direction);
                if (entity.direction == EntityInstance.dir.up)
                {
                    if (grid[posY + i, posX + j] is SpikeInstance)
                    {
                        SpikeInstance spike = grid[posY + i, posX + j] as SpikeInstance;
                        Damage(entity,spike.hp);
                        spike.entity.spikeList.Remove(spike);
                        Destroy(spike.prefab);
                    }
                    grid[posY + i,posX + j] = entity;
                }
                if (entity.direction == EntityInstance.dir.right)
                {
                    if (grid[posY + i, posX + j] is SpikeInstance)
                    {
                        SpikeInstance spike = grid[posY + i, posX + j] as SpikeInstance;
                        Damage(entity,spike.hp);
                        spike.entity.spikeList.Remove(spike);
                        Destroy(spike.prefab);
                    }
                    grid[posY + i,posX + j] = entity;
                }
                if (entity.direction == EntityInstance.dir.left)
                {
                    if (grid[posY - i, posX - j] is SpikeInstance)
                    {
                        SpikeInstance spike = grid[posY - i, posX - j] as SpikeInstance;
                        Damage(entity,spike.hp);
                        spike.entity.spikeList.Remove(spike);
                        Destroy(spike.prefab);
                    }
                    grid[posY - i,posX - j] = entity;
                }
                if (entity.direction == EntityInstance.dir.down)
                {
                    if (grid[posY - i, posX - j] is SpikeInstance)
                    {
                        SpikeInstance spike = grid[posY - i, posX - j] as SpikeInstance;
                        Damage(entity,spike.hp);
                        spike.entity.spikeList.Remove(spike);
                        Destroy(spike.prefab);
                    }
                    grid[posY - i,posX - j] = entity;
                }
            }
        }
    }
    
    public bool CanMove(EntityInstance entity, int newX, int newY)
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
                    if (grid[newY+i, newX+j] != null && grid[newY+i, newX+j] != entity && grid[newY+i, newX+j] is not SpikeInstance)
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
                    if (grid[newY+i, newX+j] != null && grid[newY+i, newX+j] != entity && grid[newY+i, newX+j] is not SpikeInstance)
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

                    if (grid[newY - i, newX - j] != null && grid[newY - i, newX - j] != entity && grid[newY+i, newX+j] is not SpikeInstance)
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

                    if (grid[newY - i, newX - j] != null && grid[newY - i, newX - j] != entity && grid[newY+i, newX+j] is not SpikeInstance)
                    {
                        return false;
                    }
                }
            }
        }
        
        return true;
    }
    
    public bool Attack(AttackData attack,EntityInstance entity, int x, int y)
    {
        bool endReturn = false;
        
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
            Debug.Log(tile);
            if (tile != null && tile != entity && tile != lastEnnemyTouched && tile is not SpikeInstance)
            {
                if (entity == player)
                {
                    hasAttacked = true;
                    player.oxygen -= player.oxygenLostAttack;
                }
                endReturn = true;
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
        }

        return endReturn;
    }

    int WeakPointDetection(EntityInstance attackedEntity,EntityInstance attackerEntity,AttackData attack, int x, int y, int dmg)
    {
        foreach (var weakPoint in attackedEntity.weakPointList)
        {
            Debug.Log(weakPoint.direction);
            int yoffset = attack.Yoffset;
            int xoffset = attack.Xoffset;
            if (!attackerEntity.isStanding)
            {
                yoffset = attack.Xoffset;
                xoffset = attack.Yoffset;
            }
            if (x == attackedEntity.positionX + weakPoint.posX && y == attackedEntity.positionY + weakPoint.posY)
            {
                if (weakPoint.direction == WeakPointData.dir.up)
                {
                    if (attackedEntity.positionY+weakPoint.posY < SecondaryPlayerCoordsY())
                    {
                        if (attackerEntity.hp - weakPoint.damageToAttacker <= 0)
                        {
                            Damage(attackerEntity, weakPoint.damageToAttacker);
                            return 0;
                        }
                        Damage(attackerEntity, weakPoint.damageToAttacker);
                        Debug.Log("up");
                        return (dmg * weakPoint.damageMulti);
                    }
                }
                if (weakPoint.direction == WeakPointData.dir.down)
                {
                    if (attackedEntity.positionY+weakPoint.posY > SecondaryPlayerCoordsY())
                    {
                        if (attackerEntity.hp - weakPoint.damageToAttacker <= 0)
                        {
                            Damage(attackerEntity, weakPoint.damageToAttacker);
                            return 0;
                        }
                        Damage(attackerEntity, weakPoint.damageToAttacker);
                        Debug.Log("down");
                        return (dmg * weakPoint.damageMulti);
                    }
                }
                if (weakPoint.direction == WeakPointData.dir.left)
                {
                    if (attackedEntity.positionX+weakPoint.posX > SecondaryPlayerCoordsX())
                    {
                        if (attackerEntity.hp - weakPoint.damageToAttacker <= 0)
                        {
                            Debug.Log("ui");
                            Damage(attackerEntity, weakPoint.damageToAttacker);
                            return 0;
                        }
                        Debug.Log("non");
                        Damage(attackerEntity, weakPoint.damageToAttacker);
                        Debug.Log("left");
                        return (dmg * weakPoint.damageMulti);
                    }
                }
                if (weakPoint.direction == WeakPointData.dir.right)
                {
                    if (attackedEntity.positionX+weakPoint.posX < SecondaryPlayerCoordsX())
                    {
                        if (attackerEntity.hp - weakPoint.damageToAttacker <= 0)
                        {
                            Damage(attackerEntity, weakPoint.damageToAttacker);
                            return 0;
                        }
                        Damage(attackerEntity, weakPoint.damageToAttacker);
                        
                        Debug.Log("right");
                        return (dmg * weakPoint.damageMulti);
                    }
                }
                if (weakPoint.direction == WeakPointData.dir.any)
                {
                    Debug.Log("any");
                    if (attackerEntity.hp - weakPoint.damageToAttacker <= 0)
                    {
                        Damage(attackerEntity, weakPoint.damageToAttacker);
                        return 0;
                    }
                    Damage(attackerEntity, weakPoint.damageToAttacker);
                    return (dmg * weakPoint.damageMulti);
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

    public void Damage(EntityInstance entity, int dmg)
    {
        entity.TakeDamage(dmg);
        Debug.Log("pv restant : " + entity.name + entity.hp);
        if (entity == Ennemy1 && dmg > 0)
        {
            UpdateLifeBar(Ennemy1,true);
            LifeBarAnimators[1].SetTrigger("isTakingDamage");
        }
        else if (entity == Ennemy2&& dmg > 0)
        {
            UpdateLifeBar(Ennemy2,true);
            LifeBarAnimators[2].SetTrigger("isTakingDamage");
        }
        else if (entity == player&& dmg > 0)
        {
            UpdateLifeBar(player,true);
            LifeBarAnimators[0].SetTrigger("isTakingDamage");
        }
        if (entity.hp <= 0)
        {
            if (entity == player)
            {
                combatFinished = true;
                List<int> indexToSuppr = new List<int>();
                for (int i = 0; i < turnOrder.Count; i++)
                {
                    if (turnOrder[i] is FishDataInstance)
                    {
                        indexToSuppr.Add(i);
                    }
                }

                for (int i = turnOrder.Count-1; i >= 0; i--)
                {
                    Debug.Log(i);
                    Debug.Log(turnOrder[i]);
                    if (turnOrder[i] != player)
                    {
                        Die(turnOrder[i]); 
                    }
                }
                EndFight();
                deathManagerReference.Death();
            }
            else
            {
                Die(entity);
            }
            Debug.Log("mort théorique");
            if (turnOrder.Count == 1)
            {
                combatFinished = true;
            }
        }
    }

    void Die(EntityInstance entity)
    {
        for (int i = 0; i <= entity.height-1; i++)
        {
            for (int j = 0; j <= entity.width-1; j++)
            {
                grid[entity.positionY + i,entity.positionX + j] = null ;
            }
        }
        for (int i = entity.spikeList.Count-1; i >= 0; i--)
        {
            entity.spikeList[i].prefab.transform.localScale = new Vector3(10, 1, 1);
            Destroy(entity.spikeList[i].prefab);
            grid[entity.spikeList[i].positionY,entity.spikeList[i].positionX] = null;
        }
        entity.spikeList.Clear();
        Destroy(entity.LastPrevisualisation);
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

    public void EndFight()
    {
        Debug.Log("Combat Fini");
        player.positionX = _playerData.positionX;
        player.positionY = _playerData.positionY;
        player.direction = EntityInstance.dir.up;
        player.height = 2;
        player.width = 1;
        player.actionPoint = player.RespirationDatas[player.respirationIndex].actionPoints;
        Destroy(player.prefab);
        for (int i = 0; i < LifeBarPlayerEmpty.Length; i++)
        {
            LifeBarEnnemy1Empty[i].enabled = false;
            LifeBarEnnemy2Empty[i].enabled = false;
            LifeBarPlayerEmpty[i].enabled = false;
        }
        List<int> fishToSupr = new List<int>();
        for (int i = 0; i < turnOrder.Count; i ++)
        {
            if (turnOrder[i] is FishDataInstance)
            {
                fishToSupr.Add(i);
            }
        }
        for (int i = fishToSupr.Count; i > 0; i--)
        {
            Die(turnOrder[i]);
        }
        turnOrder.Clear();
        fishes.Clear();
        currentTurnIndex = 0;
        lightManager.canLooseLight = true;
        lightManager.RestartCoroutine();
        oxygenManager.canLooseOxygen = true;
        oxygenManager.RestartCoroutine();
        playerExploration.SetActive(true);
        UiExplo.SetActive(true);
        combatScene.SetActive(false);
        audioManager.SwitchToExplo();

    }
    
    int SecondaryPlayerCoordsX()
    {
        switch (CombatManager.Instance.player.direction)
        {
            case EntityInstance.dir.up:
                return player.positionX;
            case EntityInstance.dir.down:
                return player.positionX;
            case EntityInstance.dir.left:
                return player.positionX - 1;
            case EntityInstance.dir.right:
                return player.positionX + 1;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    int SecondaryPlayerCoordsY()
    {
        switch (CombatManager.Instance.player.direction)
        {
            case EntityInstance.dir.up:
                return player.positionY + 1;
            case EntityInstance.dir.down:
                return player.positionY - 1;
            case EntityInstance.dir.left:
                return player.positionY;
            case EntityInstance.dir.right:
                return player.positionY;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    void UpdateLifeBar(EntityInstance entity, bool b)
    {
        if (entity == player)
        {
            for (int i = 0; i < LifeBarPlayer.Length; i++)
            {
                if (i+1 > player.hp)
                {
                    LifeBarPlayer[i].enabled = false;
                }
                else
                {
                    LifeBarPlayer[i].enabled = true;
                    if (b)
                    {
                        StartCoroutine(pvHitTakenAnim(LifeBarPlayer[i]));
                    }
                }
            }
        }
        else if (entity == Ennemy1)
        {
            for (int i = 0; i < LifeBarEnnemy1.Length; i++)
            {
                if (i+1 > Ennemy1.hp)
                {
                    LifeBarEnnemy1[i].enabled = false;
                }
                else
                {
                    LifeBarEnnemy1[i].enabled = true;
                    if (b)
                    {
                        StartCoroutine(pvHitTakenAnim(LifeBarEnnemy1[i]));
                    }
                }
            }
        }
        else if (entity == Ennemy2)
        {
            for (int i = 0; i < LifeBarEnnemy2.Length; i++)
            {
                if (i+1 > Ennemy1.hp)
                {
                    LifeBarEnnemy2[i].enabled = false;
                }
                else
                {
                    LifeBarEnnemy2[i].enabled = true;
                    if (b)
                    {
                        StartCoroutine(pvHitTakenAnim(LifeBarEnnemy2[i]));
                    }
                }
            }
        }
    }

    List<Tuple<int, int>> getNeighbors(int x, int y)
    {
        List<Tuple<int, int>> l = new List<Tuple<int, int>>();
        if (isTileInGrid(x + 1, y))
        {
            l.Add(new Tuple<int, int>(x+1, y));
        }

        if (isTileInGrid(x - 1, y))
        {
            l.Add(new Tuple<int, int>(x-1, y));
        }

        if (isTileInGrid(x, y + 1))
        {
            l.Add(new Tuple<int, int>(x, y+1));
        }

        if (isTileInGrid(x, y - 1))
        {
            l.Add(new Tuple<int, int>(x, y-1));
        }

        return l;
    }

    bool isTileInGrid(int x, int y)
    {
        if (y < 0 || y > grid.GetLength(0) - 1 || x < 0 || x > grid.GetLength(1) - 1)
        {
            return false;
        }
        return true;
    }

    IEnumerator pvHitTakenAnim(SpriteRenderer sr)
    {
        Sprite baseSprite = sr.sprite;
        sr.sprite = pvHit;
        yield return new WaitForSeconds(pvHitFlashDelay);
        sr.sprite = baseSprite;
        yield return new WaitForSeconds(pvHitFlashDelay);
        sr.sprite = pvHit;
        yield return new WaitForSeconds(pvHitFlashDelay);
        sr.sprite = baseSprite;
    }
}