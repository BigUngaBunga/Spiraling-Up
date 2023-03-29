using UnityEngine;
using Setting = GraphicalSettings.Setting;

public class Projectile : Deadly
{
    [SerializeField] GameObject impactEffect;
    [SerializeField] float impactEffectDuration;

    LayerMask hitMask;

    float speed;
    float maxRange;

    float distanceTraveled;

    Setting setting = Setting.Medium;

    public void Initiate(LayerMask hitMask, float speed, float maxRange, Setting setting)
    {
        this.hitMask = hitMask;
        this.speed = speed;
        this.maxRange = maxRange;
        this.setting = setting;
    }

    void Update()
    {
        float travelDistance = speed * Time.deltaTime;
        distanceTraveled += travelDistance;
        RaycastHit2D hit = Physics2D.Raycast(transform.position - (transform.right * transform.localScale.x), transform.right, travelDistance + transform.localScale.x, hitMask);
        if (hit.collider != null)
        {
            if (hit.transform.TryGetComponent(out PlayerController player))
                player.Kill(deathReason);

            travelDistance = hit.distance;

            if (setting == Setting.High && impactEffect != null && player == null)
                Destroy(Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal)), impactEffectDuration);
            
            Destroy(gameObject);
        }
        else if (distanceTraveled > maxRange)
        {
            Destroy(gameObject);
        }

        transform.position += transform.right * travelDistance;
    }
}
