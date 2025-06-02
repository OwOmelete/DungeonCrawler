using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using Random = UnityEngine.Random;

public class IaFishExplo : MonoBehaviour
{
    [SerializeField] private Transform topLeftCorner;
    [SerializeField] private Transform bottomRightCorner;
    [SerializeField] private float speed = 2f;
    [SerializeField] private Transform parentTransform;
    [SerializeField] private SpriteRenderer parentSprite;
    [SerializeField] private float rotationTime = 0.5f; 
    public bool isBrotulo = false;
    private Vector3 actualTarget;
    private bool hitWall;
    [SerializeField] private Rigidbody2D rb;
    private Coroutine moveCoroutine = null;
    [HideInInspector] public bool canMove = true;

    void Start()
    {
        RestartCoroutine();
    }

    IEnumerator Move()
    {
        float angle;
        while (canMove) 
        {
            Vector2 direction = (actualTarget - transform.position).normalized;
            rb.linearVelocity = direction * speed;
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            parentTransform.DOKill();
            if (!isBrotulo)
            {
                parentTransform.DORotate(new Vector3(0, 0, angle), rotationTime);
            }
            else
            {
                parentTransform.DORotate(new Vector3(0, 0, angle + 270), rotationTime);
            }
            
            if ((angle > 90f || angle < -90 )&& !isBrotulo)
            {
                parentSprite.flipY = true;
            }
            else if (!isBrotulo)
            {
                parentSprite.flipY = false;
            }
            else if ((angle > 0 || angle < -180) && isBrotulo)
            {
                parentSprite.flipX = true;
            }
            else
            {
                parentSprite.flipX = false;
            }
            if (ApproximatelyEqual(transform.position, actualTarget)|| hitWall)
            {
                ChangeTarget();
                
            }
            yield return null;
        }
    }
    
    void ChangeTarget()
    {
        actualTarget.x = Random.Range(topLeftCorner.position.x, bottomRightCorner.position.x);
        actualTarget.y = Random.Range(topLeftCorner.position.y, bottomRightCorner.position.y);
    }
    
    bool ApproximatelyEqual(Vector3 a, Vector3 b, float error = 1f)
    {
        return Vector3.Distance(a, b) < error;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        hitWall = true;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        hitWall = false;
    }

    public void RestartCoroutine()
    {
        canMove = true;
        hitWall = false;
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
        ChangeTarget();
        moveCoroutine = StartCoroutine(Move());
    }

    private void OnDisable()
    {
        StopCoroutine(moveCoroutine);
        moveCoroutine = null;
        Debug.Log(moveCoroutine);
    }
}
