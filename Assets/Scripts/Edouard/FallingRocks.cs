using UnityEngine;


public class FallingRocks : MonoBehaviour
{
    public GameObject fallingRock;
    //public float ySpawnCenter;
    public float activationChance = 50f; 
    public int rockCount = 5; 
    public float minX = -100f, maxX = 100f;

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
            float ySpawn = Random.Range(transform.position.y + 10, transform.position.y + 20);
            Vector2 spawnPos = new Vector2(xSpawn, ySpawn);
        
            Instantiate(fallingRock, spawnPos, Quaternion.identity); 
        }
    }
}