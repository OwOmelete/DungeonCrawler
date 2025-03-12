using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Current : MonoBehaviour
{
    [Header("Current Options")]
    public float currentStrenght;
    public Vector2 direction;

    private void OnTriggerStay2D(Collider2D other)
    {
        other.attachedRigidbody.AddForce(direction * currentStrenght, ForceMode2D.Force);
    }

    /*private void OnTriggerExit2D(Collider2D other)
    {
        currentStrenght = 0;
        other.attachedRigidbody.AddForce(-currentStrenght * direction, 0f);
    }*/
}
 