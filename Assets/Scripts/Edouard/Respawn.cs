using UnityEngine;

public partial class Respawn : MonoBehaviour
{
    public GameObject respawnPoint;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Respawn");
        other.transform.position = respawnPoint.transform.position;
    }
}
