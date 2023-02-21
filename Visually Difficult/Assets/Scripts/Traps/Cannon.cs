using UnityEngine;

public class Cannon : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] Transform muzzle;
    [SerializeField] Transform barrelPivot;
    [SerializeField] LayerMask aimMask;

    [Header("Rotation")]
    [SerializeField] float defaultAngle;
    [SerializeField] Vector2 minMaxAngle = new (-180, 180);

    [Header("Projectile")]
    [SerializeField] Projectile projectile;

    [SerializeField] LayerMask hitMask;

    [SerializeField, Min(0.1f)] float fireRate = 1;
    [SerializeField, Min(0.1f)] float speed = 5;
    [SerializeField, Min(1)] float maxRange = 20;

    [Header("Effect")]
    [SerializeField] GameObject fireEffect;
    [SerializeField] float fireEffectDuration;

    Transform target;

    float timeToFire;

    void Start()
    {
        SetTarget();
    }

    void Update()
    {
        if (target == null)
        {
            SetTarget();
            return;
        }

        Vector2 direction = target.position - barrelPivot.position;
        
        if (Physics2D.Raycast(transform.position, direction, direction.magnitude, aimMask).collider != null)
        {
            barrelPivot.rotation = Quaternion.Euler(barrelPivot.eulerAngles.x, barrelPivot.eulerAngles.y, defaultAngle);
            return;
        }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        barrelPivot.rotation = Quaternion.Euler(barrelPivot.eulerAngles.x, barrelPivot.eulerAngles.y, Mathf.Clamp(angle, minMaxAngle.x, minMaxAngle.y));

        if (Time.time >= timeToFire)
        {
            Projectile temp = Instantiate(projectile, muzzle.position, muzzle.rotation);
            temp.Initiate(hitMask, speed, maxRange);

            timeToFire = Time.time + (1 / fireRate);
        }
    }

    void SetTarget() => target = GameObject.FindGameObjectWithTag("Player").transform;
}
