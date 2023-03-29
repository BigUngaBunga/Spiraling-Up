using UnityEngine;

public class Cannon : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] Transform muzzle;
    [SerializeField] LayerMask aimMask;
    private Animator animation;

    [Header("Rotation")]
    [SerializeField] Vector2 minMaxAngle = new (-180, 180);
    private readonly float defaultAngle = 90;

    [Header("Projectile")]
    [SerializeField] Projectile projectile;

    [SerializeField] LayerMask hitMask;

    [SerializeField, Min(0.1f)] float fireRate = 1;
    [SerializeField, Min(0.1f)] float speed = 5;
    [SerializeField, Min(1)] float maxRange = 20;

    [Header("Effect")]
    [SerializeField] GameObject fireEffect;
    [SerializeField] float fireEffectDuration;

    [SerializeField] private Color justFiredColour;
    [SerializeField] private Color rearmedColour;
    private LineRenderer line;

    Transform target;
    float timeToFire;

    void Start()
    {
        animation = GetComponent<Animator>();
        line = GetComponent<LineRenderer>();
        SetTarget();
        StartCooldown();

        line.positionCount = 2;
        line.enabled = false;
    }

    void Update()
    {
        if (target == null)
        {
            SetTarget();
            return;
        }

        Vector2 direction = target.position - transform.position;
        var rayHit = Physics2D.Raycast(transform.position, direction.normalized, maxRange);
        bool hitPlayer = rayHit.transform != null && rayHit.transform.gameObject.CompareTag("Player");
        line.enabled = hitPlayer;
        if (!hitPlayer) 
            return;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.Clamp(angle, minMaxAngle.x, minMaxAngle.y) + defaultAngle);

        line.SetPosition(0, transform.position);
        line.SetPosition(1, target.position);
        float reloadPercentage = 1 - Mathf.Clamp((timeToFire - Time.time) / (1f / fireRate), 0f, 1f);
        Color lineColour = Color.Lerp(justFiredColour, rearmedColour, reloadPercentage);
        line.startColor = lineColour;
        line.endColor = lineColour;

        if (Time.time >= timeToFire)
        {
            Projectile temp = Instantiate(projectile, muzzle.position, muzzle.rotation);
            temp.Initiate(hitMask, speed, maxRange);
            animation.SetTrigger("Fire");
            StartCooldown();
        }
    }

    void StartCooldown() => timeToFire = Time.time + (1 / fireRate);

    void SetTarget() => target = GameObject.FindGameObjectWithTag("Player").transform;

    private void OnDrawGizmos()
    {
        if (target == null) return;
        Gizmos.color = Color.red;
        Vector2 direction = target.position - transform.position;
        Gizmos.DrawRay(transform.position, direction.normalized * maxRange);
    }
}
