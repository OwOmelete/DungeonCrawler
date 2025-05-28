using System;
using UnityEngine;

public class DirectionPointsScript : MonoBehaviour
{
    private Transform[] pathPoints;
    private Transform player;

    public void Respawn()
    {
        for (int i = 0; i < pathPoints.Length; i++)
        {
            if (pathPoints[i].position.y < player.position.y)
            {
                pathPoints[i].gameObject.SetActive(true);
            }
            else
            {
                pathPoints[i].gameObject.SetActive(false);
            }
        }
    }
}
