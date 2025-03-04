using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTimeZone : MonoBehaviour
{
    public int damagePerSecond = 10; 
    private Dictionary<GameObject, Coroutine> activeCoroutines = new Dictionary<GameObject, Coroutine>();

    void OnTriggerEnter2D(Collider2D other)
    {
        TestPlayer player = other.GetComponent<TestPlayer>(); 
        if (player != null && !activeCoroutines.ContainsKey(other.gameObject))
        {
            Coroutine damageCoroutine = StartCoroutine(ApplyDamageOverTime(player, other.gameObject));
            activeCoroutines[other.gameObject] = damageCoroutine;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (activeCoroutines.ContainsKey(other.gameObject))
        {
            StopCoroutine(activeCoroutines[other.gameObject]);
            activeCoroutines.Remove(other.gameObject);
        }
    }

    private IEnumerator ApplyDamageOverTime(TestPlayer targetPlayer, GameObject target)
    {
        while (true)
        {
            targetPlayer.TakeDamage(damagePerSecond);
            yield return new WaitForSeconds(1f);
        }
    }
}