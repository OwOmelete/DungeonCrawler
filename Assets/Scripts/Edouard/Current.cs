using UnityEngine;


public class Current : MonoBehaviour
{
    [Header("Current Options")]
    public float strength;

    public void OnTriggerStay2D(Collider2D other)
    {
        if (!other.attachedRigidbody)
            return;
        
        Vector2 forceDirection = transform.right; 
        other.attachedRigidbody.AddForce(forceDirection * strength, ForceMode2D.Force); 
    }
}