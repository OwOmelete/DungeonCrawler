using System;
using Unity.VisualScripting;
using UnityEngine;
public class Player : MonoBehaviour
{

    private float moveHorizontal = 0f;    
    private float moveVertical = 0f;
    [SerializeField] private float moveSpeed = 55f;
    [SerializeField] private Rigidbody2D rb;
    void Update()
    {
        moveHorizontal = Input.GetAxis("Horizontal");     
        moveVertical = Input.GetAxis("Vertical");      
        rb.linearVelocity = new Vector2(moveHorizontal * moveSpeed, moveVertical * moveSpeed);
    }
}
