using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public Player player;
    public HealthManager healthManager;
   

    private void Update()
    {
        if (player.oxygenManager.player.oxygen <= 0)
        {
            healthManager.Respawn();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        healthManager.currentCheckPoint = gameObject;
    }

    
}