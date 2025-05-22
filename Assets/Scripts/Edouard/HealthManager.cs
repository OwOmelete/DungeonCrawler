using UnityEngine;

public class HealthManager : MonoBehaviour
{
    #region Variables

    [Header("Values")]
    public int maxHealth = 10;
    public PlayerData player;

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

    public void TakeDamage(int damageAmount)
    {
        player.hp -= damageAmount;
        player.hp = Mathf.Clamp(player.hp, 0, maxHealth);
    }
}
