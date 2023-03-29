using System.Collections;
using UnityEngine;
using Settings = GraphicalSettings.Setting;

public class PlayerParticles : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] GameObject accelerateEffect;
    [SerializeField] float minMoveSpeedDifference;
    [SerializeField] float accelerateEffectDuration;
    [SerializeField] Transform accelerateEffectOrigin;

    [Header("Jumping")]
    [SerializeField] GameObject jumpEffect;
    [SerializeField] float jumpEffectDuration;
    [SerializeField] Transform jumpEffectOrigin;

    [Header("Wall Jumping")]
    [SerializeField] GameObject wallJumpEffect;
    [SerializeField] float wallJumpEffectDuration;
    [SerializeField] Transform wallJumpEffectOriginRight;
    [SerializeField] Transform wallJumpEffectOriginLeft;

    [Header("Landing")]
    [SerializeField] GameObject landEffect;
    [SerializeField] float minFallTime;
    [SerializeField] float landEffectDuration;
    [SerializeField] Transform landEffectOrigin;

    [Header("Death")]
    [SerializeField] GameObject deathEffect;
    [SerializeField] float deathDelay;
    [SerializeField] Transform deathEffectOrigin;

    bool accelerated;
    bool isGrounded;

    float oldSpeed;
    float fallTimer;

    Settings settings = Settings.Medium;

    void Start()
    {
        settings = FindAnyObjectByType<VisualUpdater>().Settings;
    }

    void Update()
    {
        fallTimer = isGrounded ? 0 : fallTimer + Time.deltaTime;
    }

    public void UpdateSpeed(float speed, float delta)
    {
        if (accelerateEffect == null || !isGrounded)
            return;

        if (Mathf.Abs(speed) > Mathf.Abs(oldSpeed) && Mathf.Abs(oldSpeed - speed) / delta >= minMoveSpeedDifference)
        {
            if (!accelerated)
                PlayEffect(accelerateEffect, accelerateEffectOrigin, accelerateEffectDuration, Settings.Medium);
            
            accelerated = true;
        }
        else if (accelerated)
        {
            accelerated = false;
        }

        oldSpeed = speed;
    }

    public void SetGrounded(bool value) => isGrounded = value;
    public void Jump() => PlayEffect(jumpEffect, jumpEffectOrigin, jumpEffectDuration);
    public void WallJump(float directionX) => PlayEffect(wallJumpEffect, directionX > 0 ? wallJumpEffectOriginLeft : wallJumpEffectOriginRight, wallJumpEffectDuration, Settings.Medium);
    public void Land() => PlayEffect(fallTimer >= minFallTime ? landEffect : null, landEffectOrigin, landEffectDuration);
    public void Die(System.Action runOnEnd) => StartCoroutine(PlayDeathEffect(runOnEnd));

    void PlayEffect(GameObject effect, Transform origin, float duration, Settings minimumLevel = Settings.High)
    {
        if (effect == null || settings > minimumLevel)
            return;

        Destroy(Instantiate(effect, origin.position, origin.rotation), duration);
    }

    IEnumerator PlayEffect(GameObject effect, Transform origin, float duration, float delay, Settings minimumLevel = Settings.High)
    {
        yield return new WaitForSeconds(delay);
        PlayEffect(effect, origin, duration, minimumLevel);
    }

    IEnumerator PlayDeathEffect(System.Action runOnEnd)
    {
        if (deathEffect != null)
        {
            PlayEffect(deathEffect, deathEffectOrigin, deathDelay, Settings.Low);
            yield return new WaitForSeconds(deathDelay);
        }

        runOnEnd?.Invoke();
    }
}
