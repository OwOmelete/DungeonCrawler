using UnityEngine;
using UnityEngine.Serialization;

public class HealthManager : MonoBehaviour
{
    #region Variables

    [Header("Values")]
    public int maxHealth = 10;
    public PlayerData playerData;
    public Player player;
    public GameObject currentCheckPoint;

    #endregion
    
    private void Start()
    {
        playerData.hp = maxHealth;
    }

    public void Heal(int healAmount)
    {
        playerData.hp += healAmount;
        playerData.hp = Mathf.Clamp(playerData.hp, 0, maxHealth);
    }

    public void TakeDamage(int damageAmount)
    {
        playerData.hp -= damageAmount;
        playerData.hp = Mathf.Clamp(playerData.hp, 0, maxHealth);
    }

    public void Respawn()
    {
        //TODO: actualiser cette methode pour quelle soit fonctionelle avec la mort dans le niveau.
        player.transform.position = currentCheckPoint.transform.position;
    }
}
