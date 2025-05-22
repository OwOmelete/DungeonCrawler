using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public Player player;

    private void Update()
    {
        if (player.oxygenManager.player.oxygen <= 0)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        player.transform.position = transform.position;
    }
}