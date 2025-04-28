using Unity.Mathematics;
using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using Quaternion = System.Numerics.Quaternion;
[RequireComponent(typeof(SpriteRenderer))]
public class Rotation : MonoBehaviour
{
    [SerializeField] private float lerpSpeed;
    [SerializeField] private float angleBeforeRestartMovement = 10f;
    private SpriteRenderer spriteRenderer;
    [HideInInspector] public bool canMove = true;
    [HideInInspector] public float angleDiff;
    private float xScale;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        xScale = transform.localScale.x;
    }
    void Update()
    {
        Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
        if (direction == Vector2.zero)
        {
            return;
        }
        float targetAngle = (Mathf.Atan2(direction.x, -direction.y) * Mathf.Rad2Deg);
        transform.rotation = UnityEngine.Quaternion.Lerp(transform.rotation,UnityEngine.Quaternion.Euler(transform.rotation.x,transform.rotation.y,targetAngle), lerpSpeed);
        float currentAngle = NormalizeAngle(transform.eulerAngles.z);
        if (currentAngle > 0)
        {
            transform.localScale = new Vector2(xScale, transform.localScale.y);
        }
        else
        {
            transform.localScale = new Vector2(-xScale, transform.localScale.y);
        }
        angleDiff = Mathf.DeltaAngle(currentAngle, targetAngle);
        if (Mathf.Abs(angleDiff) > angleBeforeRestartMovement) 
        {
            canMove = false;
        }
        else
        {
            canMove = true;
        }
    }
    float NormalizeAngle(float angle)
    {
        angle = angle % 360;
        if (angle > 180) angle -= 360;
        return angle;
    }
}
