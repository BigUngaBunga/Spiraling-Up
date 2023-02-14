using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [Range(1f, 10f)]
    [SerializeField] private float interpolationSpeed;
    [SerializeField] private Vector3 offset;

    private void FixedUpdate()
    {
        Vector3 targetPosition = target.transform.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, interpolationSpeed * Time.fixedDeltaTime);
    }
}
