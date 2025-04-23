using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] float dashSpeed = 10f;
    [SerializeField] private float enemyAttackDelay = 1f;
    private GameObject playerReference;
    private bool playerOnTrigger;
    private void Awake()
    {
        playerReference = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }
    
    IEnumerator DashToPlayer()
    {
        yield return new WaitForSeconds(enemyAttackDelay);
        Vector2 direction = (playerReference.transform.position - transform.position).normalized;
        float duration = 0.2f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            Vector2 newPosition = rb.position + direction * dashSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);

            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnTrigger = true;
            StartCoroutine(RaycastUpdate());
        }
    }

    IEnumerator RaycastUpdate()
    {
        while (playerOnTrigger)
        {
            Vector2 direction = (playerReference.transform.position - transform.position).normalized;
            float distance = Vector2.Distance(transform.position, playerReference.transform.position);
            int layerMask = LayerMask.GetMask("Player", "Wall");
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, layerMask);
            Debug.DrawRay(transform.position, direction * distance, Color.red);
            if (hit)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    StartCoroutine(DashToPlayer());
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnTrigger = false;
        }
    }
}
