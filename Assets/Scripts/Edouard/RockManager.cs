using UnityEngine;

public class RockManager : MonoBehaviour
{
    [SerializeField] private HealthManager healthManagerreference;
    public int delay = 5;
    public int damage;
    private void Start()
    {
        healthManagerreference = FindObjectOfType<HealthManager>();
        Destroy(gameObject, delay);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            healthManagerreference.playerData.hp -= damage;
            Debug.Log(gameObject.name + " collision with " + other.gameObject.name);
        }
    }
}