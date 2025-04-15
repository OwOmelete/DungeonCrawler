using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniVolcano : MonoBehaviour
{
    [SerializeField] private TestPlayer testPlayer;
    public int damage;

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
