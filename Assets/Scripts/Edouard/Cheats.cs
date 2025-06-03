using System;
using UnityEngine;
using UnityEngine.UI;

public class Cheats : MonoBehaviour
{
    #region Variables
    //public Input input;
    public GameObject cheatsCanvas;
    private bool isShowingCheats = false;
    
    public GameObject player;
    
    public GameObject Tp1stBiomeObject;
    public GameObject Tp2dnBiomeObject;
    public GameObject Tp3rdBiomeObject;
    
    public CombatManager combatManagerReference;

    public GameObject enemies;
    #endregion

    #region Unity Functions
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            cheatsCanvas.SetActive(!isShowingCheats);
            isShowingCheats = !isShowingCheats;
        }
    }
    #endregion

    #region CheatCommands
    
    public void Tp1stBiome()
    {
        player.transform.position = Tp1stBiomeObject.transform.position;
    }

    public void Tp2ndBiome()
    {
        player.transform.position = Tp2dnBiomeObject.transform.position;
    }

    public void Tp3rdBiome()
    {
        player.transform.position = Tp3rdBiomeObject.transform.position;
    }

    public void ToggleEnemies()
    {
        enemies.SetActive(!enemies.activeSelf);
    }

    /*public void StartCombats()
    {
        combatManagerReference.InitCombat();
    }*/
    
    public void EndCombat()
    {
        combatManagerReference.EndFight();
    }
    
    #endregion
    
}
