using System;
using System.Collections;
using TMPro;
using UnityEditor.Build.Content;
using UnityEditor.Searcher;
using UnityEngine;


public class GamepadCombatControl : MonoBehaviour
{
    [SerializeField] private CombatManager combatManagerReference;
    [SerializeField] private GameObject selectionParent;
    [SerializeField] private float waitTimeBeforeNextSelection = 0.2f;
    [SerializeField] private int tabHeight = 5;
    [SerializeField] private int tabWidth = 6;
    [SerializeField] private TMP_Text fishName;
    [SerializeField] private TMP_Text fishHp;
    [SerializeField] private TMP_Text fishArmor;
    [SerializeField] private Transform menuActionsPlayer;
    [SerializeField] private float uiOffsetX = 1f;
    [SerializeField] private float uiOffsetY = 1f;
    private bool waitForClick;
    private bool canChangeSelection = true;
    private GameObject[,] selectionTab = new GameObject[5,6];
    private int[] actualPlacement = new int[2];
    
    private void Awake()
    {
        actualPlacement[0] = 0;
        actualPlacement[1] = 0;
        for (int i = 0; i < tabHeight; i++)
        {
            for (int j = 0; j < tabWidth ; j++)
            {
                int index = i * tabWidth + j;
                selectionTab[i, j] = selectionParent.transform.GetChild(index).gameObject;
            }
        }
    }

    private void Update()
    {
        float dpadX = Input.GetAxis("DPadHorizontal");
        float dpadY = Input.GetAxis("DPadVertical");

        if (dpadX < -0.5f && canChangeSelection)
        {
            if (waitForClick)
            {
                StartCoroutine(WaitBeforeNextSelection());
                combatManagerReference.FlipPlayerLeft(combatManagerReference.player);
                waitForClick = false;
            }
            else if (actualPlacement[1] != 0)
            {
                StartCoroutine(WaitBeforeNextSelection());
                selectionTab[actualPlacement[0], actualPlacement[1]].SetActive(false);
                actualPlacement[1] -= 1;
                selectionTab[actualPlacement[0], actualPlacement[1]].SetActive(true);
            }
        }
        else if (dpadX > 0.5f && canChangeSelection)
        {
            if (waitForClick)
            {
                StartCoroutine(WaitBeforeNextSelection());
                combatManagerReference.FlipPlayerRight(combatManagerReference.player);
                waitForClick = false;
            }
            else if (actualPlacement[1] != tabWidth - 1)
            {
                StartCoroutine(WaitBeforeNextSelection());
                selectionTab[actualPlacement[0], actualPlacement[1]].SetActive(false);
                actualPlacement[1] += 1;
                selectionTab[actualPlacement[0], actualPlacement[1]].SetActive(true);
            }
        }
        if (dpadY < -0.5f && canChangeSelection && !waitForClick)
        {
            if (actualPlacement[0] != 0 )
            {
                StartCoroutine(WaitBeforeNextSelection());
                selectionTab[actualPlacement[0], actualPlacement[1]].SetActive(false);
                actualPlacement[0] -= 1;
                selectionTab[actualPlacement[0], actualPlacement[1]].SetActive(true);
            }
        }
        else if (dpadY > 0.5f && canChangeSelection && !waitForClick)
        {
            
            if (actualPlacement[0] != tabHeight - 1)
            {  
                StartCoroutine(WaitBeforeNextSelection());
                selectionTab[actualPlacement[0], actualPlacement[1]].SetActive(false);
                actualPlacement[0] += 1;
                selectionTab[actualPlacement[0], actualPlacement[1]].SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton0)&& canChangeSelection && !waitForClick)
        {
            ShowInformations();
            StartCoroutine(WaitBeforeNextSelection());
            
        }
    }

    IEnumerator WaitBeforeNextSelection()
    {
        canChangeSelection = false;
        yield return new WaitForSeconds(waitTimeBeforeNextSelection);
        canChangeSelection = true;
    }

    void ShowInformations()
    {
        EntityInstance actualCase = combatManagerReference.grid[actualPlacement[0], actualPlacement[1]];
        if (actualCase != null)
        {
            
            if (actualCase == combatManagerReference.player)
            {
                menuActionsPlayer.gameObject.SetActive(true);
                if (actualPlacement[1] == tabWidth - 1 )
                {
                    menuActionsPlayer.position =
                        selectionTab[actualPlacement[0], actualPlacement[1]].transform.position + new Vector3(-uiOffsetX * 2.2f, uiOffsetY, 0);
                }
                else
                {
                    menuActionsPlayer.position =
                        selectionTab[actualPlacement[0], actualPlacement[1]].transform.position + new Vector3(uiOffsetX,uiOffsetY,0);
                }
                
            }
            
            else
            {
                UpdateInformations(actualCase);
            }
        }
    }

    void UpdateInformations(EntityInstance entity)  // nathan ici si tu peux mettre genre quand on attaque ça update l'ui sur le poisson qu'on vient de toucher
    {
        fishName.text = entity.name;
        fishHp.text = ("Hp : " + entity.hp);
        fishArmor.text = ("Armor : " + entity.armor);
    }

    public void PlayerButtonUnactive()
    {
        menuActionsPlayer.gameObject.SetActive(false);
    }

    public void Rotation()
    {
        waitForClick = true;
        PlayerButtonUnactive();
    }
    
}

