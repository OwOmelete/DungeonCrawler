using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FishHitPlayer : MonoBehaviour
{
    [SerializeField] private StartCombat startCombatReference;
    [SerializeField] private GameObject fish;
    [SerializeField] private List<FishData> fishDatas;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        startCombatReference.SwitchToCombat(fishDatas);
        fish.transform.DOKill();
        Destroy(fish);
    }
}
