using UnityEngine;

public class RockManager : MonoBehaviour
{
    [SerializeField] private TestPlayer testPlayer;
    public int delay = 5;
    public int damage;
    private void Start()
    {
        testPlayer = FindObjectOfType<TestPlayer>();
        Destroy(gameObject, delay);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //testPlayer.health -= damage;
            Debug.Log(gameObject.name + " collision with " + other.gameObject.name);
        }
    }
}