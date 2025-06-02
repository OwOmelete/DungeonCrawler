using System;
using System.Collections.Generic;
using UnityEngine;

public class FishHitPlayer : MonoBehaviour
{
    [SerializeField] private int playerPosCombatX = 0;
    [SerializeField] private int playerPosCombatY = 0;
    
    [SerializeField] private StartCombat startCombatReference;
    [SerializeField] private GameObject fish;
    [SerializeField] private List<FishData> fishDatas;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            startCombatReference.playerPosCombatX = playerPosCombatX;
            startCombatReference.playerPosCombatY = playerPosCombatY;
            startCombatReference.SwitchToCombat(fishDatas);
            fish.SetActive(false);
        }
    }
}
