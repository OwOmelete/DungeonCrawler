using System;
using UnityEngine;

public class RockManager : MonoBehaviour
{
    public float delay = 5f;
    public float damage;
    private void Start()
    {
        Destroy(gameObject, delay);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other.gameObject.name + " collision" + gameObject.name);
    }
}