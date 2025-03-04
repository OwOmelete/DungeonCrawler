using System;
using UnityEngine;

public class DestroyOnContact : MonoBehaviour
{
    //à mettre dans le Player
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Objet Détruit");
        Destroy(gameObject);
    }
}
