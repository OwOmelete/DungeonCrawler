using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class MarineCurrent : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float movementTime;
    [SerializeField] private Transform[] pathPoints;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Movement();
        }
    }

    void Movement()
    {
        Vector3[] waypoints = new Vector3[pathPoints.Length];
        for (int i = 0; i < pathPoints.Length; i++)
        {
            waypoints[i] = pathPoints[i].position;
        }

        player.DOPath(waypoints, movementTime, PathType.CatmullRom)
            .SetEase(Ease.Linear) 
            .SetSpeedBased(); 
    }
}
