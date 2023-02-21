using UnityEngine;

public class Projectile : MonoBehaviour
{
    LayerMask hitMask;

    float speed;
    float maxRange;

    float distanceTraveled;

    void Update()
    {
        float travelDistance = speed * Time.deltaTime;
        distanceTraveled += travelDistance;

        RaycastHit2D hit = Physics2D.Raycast(transform.position - (transform.right * transform.localScale.x), transform.right, travelDistance + transform.localScale.x, hitMask);
        if (hit.collider != null)
        {
            if (hit.transform.TryGetComponent(out PlayerController player))
                player.Kill();

            travelDistance = hit.distance;

            Destroy(gameObject);
        }
        else if (distanceTraveled > maxRange)
        {
            Destroy(gameObject);
        }

        transform.position += transform.right * travelDistance;
    }

    public void Initiate(LayerMask hitMask, float speed, float maxRange)
    {
        this.hitMask = hitMask;
        this.speed = speed;
        this.maxRange = maxRange;
    }
}
