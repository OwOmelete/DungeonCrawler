using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 2, -10);
    [SerializeField] private float followSpeed = 5f;

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        }
        else if (target == null)
        {
            Destroy(this.gameObject);
        }
    }

    // Fonction pour définir dynamiquement la cible
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}