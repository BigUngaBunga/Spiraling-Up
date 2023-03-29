using UnityEngine;
using Settings = GraphicalSettings.Setting;

public class Cannon : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] Transform muzzle;
    [SerializeField] Transform barrelPivot;
    [SerializeField] LayerMask aimMask;

    [Header("Rotation")]
    [SerializeField] float angleOffset;
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

    [SerializeField] private Color justFiredColour;
    [SerializeField] private Color rearmedColour;
    private LineRenderer line;

    Transform target;
    float timeToFire;

    Settings setting = Settings.Medium;

    void Start()
    {
        setting = FindAnyObjectByType<VisualUpdater>().Settings;

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

        Vector2 direction = target.position - barrelPivot.position;
        var rayHit = Physics2D.Raycast(transform.position, direction.normalized, maxRange);
        bool hitPlayer = rayHit.transform != null && rayHit.transform.gameObject.CompareTag("Player");
        line.enabled = hitPlayer;

        if (!hitPlayer) 
            return;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < minMaxAngle.x || angle > minMaxAngle.y)
        {
            line.enabled = false;
            return;
        }

        barrelPivot.rotation = Quaternion.Euler(barrelPivot.eulerAngles.x, barrelPivot.eulerAngles.y, Mathf.Clamp(angle, minMaxAngle.x, minMaxAngle.y) + angleOffset);

        line.SetPosition(0, muzzle.position);
        line.SetPosition(1, target.position);
        float reloadPercentage = 1 - Mathf.Clamp((timeToFire - Time.time) / (1f / fireRate), 0f, 1f);
        Color lineColour = Color.Lerp(justFiredColour, rearmedColour, reloadPercentage);
        line.startColor = lineColour;
        line.endColor = lineColour;

        if (Time.time >= timeToFire)
        {
            Projectile temp = Instantiate(projectile, muzzle.position, muzzle.rotation);
            temp.Initiate(hitMask, speed, maxRange, setting);
            StartCooldown();

            if (fireEffect != null && setting == Settings.High)
                Destroy(Instantiate(fireEffect, muzzle.position, muzzle.rotation), fireEffectDuration);
        }
    }

    void StartCooldown() => timeToFire = Time.time + (1 / fireRate);

    void SetTarget() => target = GameObject.FindGameObjectWithTag("Player").transform;

    private void OnDrawGizmos()
    {
        if (target == null) return;
        Gizmos.color = Color.red;
        Vector2 direction = target.position - barrelPivot.position;
        Gizmos.DrawRay(transform.position, direction.normalized * maxRange);
    }
}
