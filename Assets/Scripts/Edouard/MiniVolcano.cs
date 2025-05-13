using UnityEngine;

public class MiniVolcano : MonoBehaviour
{
    [HideInInspector] public PlayerDataInstance player;
    public int damage = 1;

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.hp -= damage;
            Debug.Log(gameObject.name + " collision with " + collision.name);
        }
    }
}
