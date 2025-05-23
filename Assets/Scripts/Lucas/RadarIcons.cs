using System;
using Unity.Mathematics;
using UnityEngine;

public class RadarIcons : MonoBehaviour
{
    [SerializeField] private Transform parent;

    private void Update()
    {
        transform.rotation = new quaternion(0f ,0f,  parent.rotation.z, 180);
    }
}
