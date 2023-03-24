using UnityEngine;

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
    [SerializeField] Transform wallJumpEffectOrigin;

    [Header("Landing")]
    [SerializeField] GameObject landEffect;
    [SerializeField] float minFallTime;
    [SerializeField] float landEffectDuration;
    [SerializeField] Transform landEffectOrigin;

    [SerializeField] bool particlesEnabled = true;
    bool accelerated;
    bool isGrounded;

    float oldSpeed;
    float fallTimer;

    private void Start()
    {
        particlesEnabled = FindAnyObjectByType<VisualUpdater>().Settings == GraphicalSettings.Setting.High;
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
                PlayEffect(accelerateEffect, accelerateEffectOrigin, accelerateEffectDuration);
            
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
    public void WallJump() => PlayEffect(wallJumpEffect, wallJumpEffectOrigin, wallJumpEffectDuration);
    public void Land() => PlayEffect(fallTimer >= minFallTime ? landEffect : null, landEffectOrigin, landEffectDuration);

    void PlayEffect(GameObject effect, Transform origin, float duration)
    {
        if (effect == null || !particlesEnabled)
            return;

        Destroy(Instantiate(effect, origin.position, origin.rotation), duration);
    }
}
