using System;
using UnityEngine;

public class RockManager : MonoBehaviour
{
    [SerializeField] public TestPlayer testPlayer;
    public int delay = 5;
    public int damage;
    private void Start()
    {
        Destroy(gameObject, delay);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        testPlayer.health -= damage;
        Debug.Log(other.gameObject.name + " collision" + gameObject.name);
    }
}