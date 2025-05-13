using UnityEngine;

public class FallingRocks : MonoBehaviour
{
    public GameObject fallingRock;
    public float activationChance = 50f; 
    public int rockCount = 5;
    [Header("Spawn horizontale")] public float minX = -100f;
    public float maxX = 100f;
    [Header("Spawn verticale")] public int minY = 10;
    public int maxY = 10;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            float roll = Random.Range(0f, 100f); 
            if (roll < activationChance) 
            {
                SpawnRocks();
            }
        }
    }

    void SpawnRocks()
    {
        for (int i = 0; i < rockCount; i++) 
        {
            float xSpawn = Random.Range(transform.position.x + minX, transform.position.x + maxX); 
            float ySpawn = Random.Range(transform.position.y + minY, transform.position.y + (minY + maxY));
            Vector2 spawnPos = new Vector2(xSpawn, ySpawn);
        
            Instantiate(fallingRock, spawnPos, Quaternion.identity); 
        }
    }
}