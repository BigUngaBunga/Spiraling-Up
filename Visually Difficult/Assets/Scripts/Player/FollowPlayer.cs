using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [Range(1f, 10f)]
    [SerializeField] private float interpolationSpeed;
    [SerializeField] private Vector3 offset;
    private Vector3 TargetPosition => target.transform.position + offset;

    private void Start()
    {
        if (target == null)
            target = GameObject.Find("Player(Clone)");
        transform.position = TargetPosition;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, TargetPosition, interpolationSpeed * Time.fixedDeltaTime);
    }
}
