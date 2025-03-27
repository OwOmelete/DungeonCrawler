using System;
using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    [SerializeField] private float enemyMoveDuration = 1f;
    [SerializeField] private float enemyAttackDelay = 1f;
    private GameObject playerReference;
    private bool playerOnTrigger;
    private void Awake()
    {
        playerReference = GameObject.FindGameObjectWithTag("Player");
    }

    void Attack()
    {
        transform.DOMove(playerReference.transform.position , enemyMoveDuration).SetDelay(enemyAttackDelay);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        playerOnTrigger = true;
        StartCoroutine(RaycastUpdate());
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
                    Attack();
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }



    private void OnTriggerExit2D(Collider2D other)
    {
        playerOnTrigger = false;
    }
}
