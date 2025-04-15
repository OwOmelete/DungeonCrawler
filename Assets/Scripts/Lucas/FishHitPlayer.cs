using System;
using System.Collections.Generic;
using UnityEngine;

public class FishHitPlayer : MonoBehaviour
{
    [SerializeField] private StartCombat startCombatReference;
    [SerializeField] private GameObject fish;
    [SerializeField] private List<FishData> fishDatas;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            startCombatReference.SwitchToCombat(fishDatas);
            Destroy(fish);
        }
    }
}
