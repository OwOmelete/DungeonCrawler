using UnityEngine;
using UnityEngine.Serialization;

public class ToxicGarbage : MonoBehaviour
{
    public GameObject player;

    void OnTriggerEnter2D(Collider2D other)
    {
        player.healt = player.healt-- * Time.deltaTime;
    }
}
