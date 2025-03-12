using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Current : MonoBehaviour
{
    [Header("Current Options")]
    public float strenght = 20000;
    void OnTriggerStay2D(Collider2D other)
    {
        //Debug.Log("Object is in trigger");
        other.attachedRigidbody.AddForce (Vector2.right*strenght*Time.deltaTime);

    }
}
 