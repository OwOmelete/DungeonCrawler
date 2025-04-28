using System;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    [SerializeField] Player player;

    public void Kill()
    {
        player.player.hp = 0;
    }
}
