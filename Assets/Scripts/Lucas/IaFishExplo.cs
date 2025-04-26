using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class IaFishExplo : MonoBehaviour
{
    [SerializeField] private Transform topLeftCorner;
    [SerializeField] private Transform bottomRightCorner;
    [SerializeField] private float speed = 2f;
    private Vector3 actualTarget;
    private bool hitWall;
    [SerializeField] private Rigidbody2D rb;
    void Start()
    {
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while (true)
        {
            Vector2 direction = (actualTarget - transform.position).normalized;
            rb.linearVelocity = direction * speed;
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
    
    bool ApproximatelyEqual(Vector3 a, Vector3 b, float error = 0.01f)
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
}
