using UnityEngine;

public class SawBladeSpinner : MonoBehaviour
{
    [Range(0, 5)]
    [SerializeField] private float angularVelocity;

    void Update()
    {
        transform.Rotate(Vector3.forward, angularVelocity);
    }
}
