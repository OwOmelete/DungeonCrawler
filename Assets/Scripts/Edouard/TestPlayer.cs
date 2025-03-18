using UnityEngine;
using System.Collections;

public class TestPlayer : MonoBehaviour
{
    // ATTENTION!!! Ce script n'est qu'un test, ne pas le confondre avec le vrai PLAYER!
    [Header("Player Stats")]
    public int health;
    public int oxygen;
    
    [Header("Player Max Stats")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int maxOxygen = 100;
    
    [Header("Oxygen Stettings")]
    public int oxygenLoss = 1; 
    public float oxygenLossInterval = 2f; 

    [Header("Player Speed and Rotation (Lucas)")]
    [SerializeField] private float moveSpeed = 55f;
    [SerializeField] private float turningMoveSpeed = 15f;
    [SerializeField] private Rotation rotationReference;
    private Rigidbody2D rb;
    private float moveHorizontal = 0f;
    private float moveVertical = 0f;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
        oxygen = maxOxygen;
        StartCoroutine(OxygenLossRoutine()); 
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
                rb.linearVelocity = new Vector2(moveHorizontal, moveVertical).normalized * moveSpeed *
                                    ((180 - Mathf.Abs(rotationReference.angleDiff))/180);
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }
    
    
    private IEnumerator OxygenLossRoutine()
    {
        while (oxygen > 0)
        {
            yield return new WaitForSeconds(oxygenLossInterval);
            oxygen -= oxygenLoss;
            oxygen = Mathf.Clamp(oxygen, 0, 100);
            //Debug.Log("Oxygen: " + oxygen);
            Death();
        }
    }

    private void Death()
    {
        if (health <= 0 || oxygen <= 0)
        {
            Debug.Log("DEAD");
            Destroy(gameObject); 
        }
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        health = Mathf.Clamp(health, 0, maxHealth); 
        //Debug.Log("Player took " + damageAmount + " damage. Health: " + health);

        Death(); 
    }
}