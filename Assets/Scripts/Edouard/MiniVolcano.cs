using UnityEngine;

public class MiniVolcano : MonoBehaviour
{
    private TestPlayer testPlayer;
    public int damage = 1;

    private void Start()
    {
        testPlayer = FindObjectOfType<TestPlayer>();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            testPlayer.health -= damage;
            Debug.Log(gameObject.name + " collision with " + collision.name);
        }
    }
}
