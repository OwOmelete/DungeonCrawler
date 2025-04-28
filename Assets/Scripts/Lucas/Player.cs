using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{

    private float moveHorizontal = 0f;    
    private float moveVertical = 0f;
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float accelerationSpeed = 3f;
    public Rotation rotationReference ;
    private Rigidbody2D rb;
    private Animator animator;

    [SerializeField] private PlayerData _playerData;
    public LightManager lightManager;
    public OxygenManager oxygenManager;
    public PlayerDataInstance player;
    private bool playerAlive = true;

    private void Awake()
    {
        player = (PlayerDataInstance)_playerData.Instance();
        animator = transform.GetChild(0).GetComponent<Animator>();
        lightManager.player = player;
        oxygenManager.player = player;
    }

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
                rb.AddForce(new Vector2(moveHorizontal, moveVertical).normalized * accelerationSpeed);
                animator.SetBool("isMoving", true);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }
            
        }
        else
        {
            if (moveHorizontal != 0 || moveVertical != 0)
            {
                rb.AddForce(new Vector2(moveHorizontal, moveVertical).normalized *
                            (accelerationSpeed * ((180 - Mathf.Abs(rotationReference.angleDiff)) / 180)));
            }
            else
            {
                animator.SetBool("isMoving", false);
            }
        }
        
    }
    void FixedUpdate()
    {
        if (rb.linearVelocity.magnitude > moveSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * moveSpeed;
        }
    }
}
