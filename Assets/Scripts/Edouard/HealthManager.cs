using System;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    #region Variables

    [Header("Values")]
    public int maxHealth = 10;

    [HideInInspector] public PlayerDataInstance player;

    #endregion
    
    private void Start()
    {
        player.hp = maxHealth;
    }

    public void Heal(int healAmount)
    {
        player.hp += healAmount;
        player.hp = Mathf.Clamp(player.hp, 0, maxHealth);
    }
}
