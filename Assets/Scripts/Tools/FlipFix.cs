using System;
using UnityEngine;

[ExecuteInEditMode]
public class FlipFix : MonoBehaviour
{
    private void Awake()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr.flipX)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            sr.flipX = false;
        }
        if (sr.flipY)
        {
            transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
            sr.flipY = false;
        }
    }
}
