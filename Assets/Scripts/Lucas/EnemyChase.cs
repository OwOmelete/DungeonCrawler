using System;
using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyChase : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] float dashSpeed = 10f;
    [SerializeField] private float enemyAttackDelay = 1f;
    [SerializeField] private GameObject playerReference;
    [SerializeField] private IaFishExplo iaFishRef;
    [SerializeField] private AudioSource audioSource;

    [SerializeField]
    private SpriteRenderer sr;

    private bool audioAlreadyPlayed = false;
    
    
    private bool playerOnTrigger;
    private void Awake()
    {
        
        rb = GetComponent<Rigidbody2D>();
    }
    
    IEnumerator DashToPlayer()
    {
        yield return new WaitForSeconds(enemyAttackDelay);
        Vector2 direction = (playerReference.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (iaFishRef.isBrotulo)
        {
            transform.DORotate(new Vector3(0, 0, angle + 270), 0.5f);
        }
        else
        {
            transform.DORotate(new Vector3(0, 0, angle), 0.5f);
            if (angle > 90f || angle < -90 )
            {
                sr.flipY = true;
            }
            else
            {
                sr.flipY = false;
            }
        }

        iaFishRef.canMove = false;
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
                    if (!audioAlreadyPlayed)
                    {
                        audioAlreadyPlayed = true;
                        audioSource.Play();
                    }
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

    private void OnDisable()
    {
        audioAlreadyPlayed = false;
    }
}
