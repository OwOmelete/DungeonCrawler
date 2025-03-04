using System;
using UnityEngine;

public class DestroyOnContact : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Objet Détruit");
        Destroy(gameObject);
    }
}
