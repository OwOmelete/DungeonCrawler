using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterCombat : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
         //SceneManager.LoadScene("Scene de combat");
         Debug.Log("entrée en combat");
    }
}
