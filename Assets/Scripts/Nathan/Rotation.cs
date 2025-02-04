using Unity.Mathematics;
using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.UIElements;
using Quaternion = System.Numerics.Quaternion;

public class Rotation : MonoBehaviour
{
    [SerializeField] private float lerpSpeed;
    void Update()
    {
         Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
         if (direction == Vector2.zero)
         {
             return;
         }
         float targetAngle = (Mathf.Atan2(direction.x, -direction.y) * Mathf.Rad2Deg);
         quaternion finalRotation = UnityEngine.Quaternion.Lerp(transform.rotation,UnityEngine.Quaternion.Euler(transform.rotation.x,transform.rotation.y,targetAngle), lerpSpeed);
         transform.rotation = finalRotation;
    }   
    
}
