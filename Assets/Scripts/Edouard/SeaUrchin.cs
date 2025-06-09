using System.Collections;
using UnityEngine;

public class SeaUrchin : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private Player player;
    [Header("Values")]
    [SerializeField] private float strength = 5000f;
    [SerializeField] private float knockDuration = 0.2f;
    [SerializeField] private int damage = 1;
    [SerializeField] private GameObject triggerTextRef;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        if (InteractTextManager.INSTANCE.firstSpikeDamage)
        {
            InteractTextManager.INSTANCE.firstSpikeDamage = false;
            triggerTextRef.SetActive(true);
        }
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        Vector2 direction = (collision.transform.position - transform.position).normalized;
        direction = direction.normalized;

        Vector2 force = direction * strength;

        StartCoroutine(BruteForceKnockback(rb, direction, force, knockDuration));

        DamagePlayer();
    }

    private IEnumerator BruteForceKnockback(Rigidbody2D rb, Vector2 direction, Vector2 force, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            rb.linearVelocity = force; 
            rb.MovePosition(rb.position + direction * 0.2f); 
            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }

    void DamagePlayer()
    {
        player.player.TakeDamage(damage);
    }
}