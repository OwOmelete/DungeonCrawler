using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DamageOverTimeArea : MonoBehaviour
{
    private Dictionary<GameObject, Coroutine> activeCoroutines = new Dictionary<GameObject, Coroutine>();

    [Header("DamageOverTime options")]
    public int damage = 10;
    public float damageInterval = 1f;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        TestPlayer player = other.GetComponent<TestPlayer>(); 
        if (player != null && !activeCoroutines.ContainsKey(other.gameObject))
        {
            Coroutine damageCoroutine = StartCoroutine(ApplyDamageOverTime(player, other.gameObject));
            activeCoroutines[other.gameObject] = damageCoroutine;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
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
            //targetPlayer.TakeDamage(damage);
            yield return new WaitForSeconds(damageInterval);
        }
    }
}