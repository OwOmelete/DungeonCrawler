using System;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{

    private float moveHorizontal = 0f;    
    private float moveVertical = 0f;
    [SerializeField] private float moveSpeed = 55f;
    [SerializeField] private Rotation rotationReference;
    private Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        if (rotationReference.canMove)
        {
            if (moveHorizontal != 0 || moveVertical != 0)
            {
                rb.linearVelocity = new Vector2(moveHorizontal, moveVertical).normalized * moveSpeed;
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
        else
        {
            if (moveHorizontal != 0 || moveVertical != 0)
            {
                rb.linearVelocity = new Vector2(moveHorizontal, moveVertical).normalized * (moveSpeed * ((180 - Mathf.Abs(rotationReference.angleDiff))/180));
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }
}
