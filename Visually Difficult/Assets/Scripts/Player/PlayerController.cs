using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float rayLength;
    [SerializeField] private Transform[] rayOrigins;

    [Header("Movement")]
    [SerializeField] private float speed = 10;
    [SerializeField] private float deceleration = 20;
    [SerializeField] private float acceleration = 15;

    [Header("Jumping")]
    [SerializeField] private float jumpVelocity = 13;
    [SerializeField] private float jumpDecay = 26;
    [SerializeField] private float jumpBuffer = 0.2f;
    [SerializeField] private float coyoteTime = 0.1f;

    [Header("Wall Jumping")]
    [SerializeField] private float wallJumpVelocity = 15;
    [Range(0, 90f)]
    [SerializeField] private float wallJumpAngle = 45;
    [SerializeField] private float wallJumpDisableDuration = 0.2f;

    [Header("Ground check")]
    [SerializeField] private Vector2 groundCheckOffset;
    [SerializeField] private Vector2 groundCheckSize;

    [Header("Debug")]
    [SerializeField] private bool drawDebug;
    [SerializeField] private bool printDebug;

    private Vector2 GroundCheckPosition => (Vector2)transform.position + groundCheckOffset * transform.localScale;
    private Vector2 GroundCheckSize => groundCheckSize * transform.localScale;

    private bool isGrounded;
    private bool justJumped;
    private bool inputDisabled;
    private bool dead;

    private float jumpTimer;
    private float coyoteTimer;

    private new Rigidbody2D rigidbody;
    private PlayerAnimator animator;
    private PlayerParticles particles;
    private new PlayerAudio audio;

    #region Input
    private InputAction moveAction;
    private InputAction jumpAction;

    private void OnDisable()
    {
        jumpAction.started -= Jump;
        moveAction.Disable();

    }

    private void OnEnable()
    {
        jumpAction.started += Jump;
        moveAction.Enable();
    }

    private IEnumerator ReenableInput()
    {
        yield return new WaitForSeconds(wallJumpDisableDuration);
        inputDisabled = false;
    }
    #endregion

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<PlayerAnimator>();
        particles = GetComponent<PlayerParticles>();
        audio = GetComponent<PlayerAudio>();

        PlayerInput input = GetComponent<PlayerInput>();

        moveAction = input.actions["Move"];
        jumpAction = input.actions["Jump"];
    }

    private void Start()
    {
        if (TryGetComponent(out DynamicAnimation dynAnim))
            dynAnim.Initialise();
    }

    public void Kill(string deathReason = "")
    {
        if (dead)
            return;

        Print(deathReason);

        enabled = false;
        animator.Die(DeathReset);
        particles.Die();
        audio.Die();

        dead = true;
    }

    private void FixedUpdate()
    {
        coyoteTimer = isGrounded ? 0 : coyoteTimer + Time.fixedDeltaTime;
        AddMovement(moveAction.ReadValue<Vector2>());
        Jumping();

        animator.SetGrounded(isGrounded);
        animator.UpdateAnimation(rigidbody.velocity);

        particles.SetGrounded(isGrounded);
        particles.UpdateSpeed(rigidbody.velocity.x, Time.fixedDeltaTime);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (justJumped || isGrounded || groundMask != (groundMask | (1 << collision.gameObject.layer)))
            return;

        for (int i = 0; i < collision.contactCount; i++)
        {
            if (Vector2.Angle(collision.GetContact(i).normal, Vector2.up) < 46)
            {
                particles.Land();
                audio.Land();
                isGrounded = true;
                return;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }

    private void DeathReset()
    {
        DataCollector.IncreasePlayerDeaths();
        DataCollector.RestartAttemptTimer();
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    #region Jumping
    private void Jump(InputAction.CallbackContext context)
    {
        jumpTimer = jumpBuffer;
        justJumped = true;
        Invoke(nameof(ResetJustJumped), Time.fixedDeltaTime * 2);
    }

    private void ResetJustJumped() => justJumped = false;

    private void WallJump(Vector2 jumpDirection)
    {
        animator.WallJump();
        particles.WallJump(jumpDirection.x);
        audio.WallJump();

        float angle = wallJumpAngle * Mathf.Deg2Rad;
        Vector2 velocity = new Vector2(jumpDirection.x * Mathf.Sin(angle), Mathf.Cos(angle)) * wallJumpVelocity;
        rigidbody.velocity = velocity;

        jumpTimer = -1;
        coyoteTimer = coyoteTime;

        inputDisabled = true;
        StopAllCoroutines();
        StartCoroutine(ReenableInput());
    }

    private IEnumerator BeginJump()
    {
        animator.Jump();
        particles.Jump();
        audio.Jump();

        coyoteTimer = coyoteTime;
        jumpTimer = -1;

        yield return new WaitForFixedUpdate();

        SetY(jumpVelocity);
        StartCoroutine(ContinueJump());

    }

    private IEnumerator ContinueJump()
    {
        WaitForFixedUpdate wait = new ();

        while (jumpAction.IsPressed() && rigidbody.velocity.y > 0)
            yield return wait;
        while (rigidbody.velocity.y > 0)
        {
            LerpY(0, jumpDecay * Time.fixedDeltaTime);
            yield return wait;
        }
    }

    private void Jumping()
    {
        if (jumpTimer <= 0)
            return;

        if (coyoteTimer < coyoteTime)
        {
            StartCoroutine(BeginJump());
        }
        else
        {
            if (WallInDirection(Vector2.left))
                WallJump(Vector2.right);
            else if (WallInDirection(Vector2.right))
                WallJump(Vector2.left);
        }

        isGrounded = false;
        jumpTimer -= Time.fixedDeltaTime;
    }

    #endregion

    #region Velocity
    private void AddMovement(Vector2 direction)
    {
        if (inputDisabled || (!isGrounded && WallInDirection(direction)))
        {
            if (printDebug)
                Print("Can't move due to wall or disabled input");
            return;
        }

        if (direction == Vector2.zero || (rigidbody.velocity.x > 0 && direction.x < 0) || (rigidbody.velocity.x < 0 && direction.x > 0))
            LerpX(0, deceleration * Time.fixedDeltaTime);

        if (direction != Vector2.zero)
        {
            float targetSpeed = direction.x * speed;
            LerpX(targetSpeed, acceleration * Time.fixedDeltaTime);
        }
    }
    private void SetX(float speed = 0) => rigidbody.velocity = new Vector2(speed, rigidbody.velocity.y);
    private void LerpX(float targetSpeed, float lerpRate) => SetX(Mathf.MoveTowards(rigidbody.velocity.x, targetSpeed, lerpRate));
    private void SetY(float speed = 0) => rigidbody.velocity = new Vector2(rigidbody.velocity.x, speed);
    private void LerpY(float targetSpeed, float lerpRate) => SetY(Mathf.MoveTowards(rigidbody.velocity.y, targetSpeed, lerpRate));
    #endregion

    #region Ground Check

    private bool IsOnGround()
    {
        return Physics2D.OverlapBoxAll(GroundCheckPosition, GroundCheckSize, 0f, groundMask).Length > 0;
    }

    private bool RayHitGround(Vector3 position, Vector3 direction) => Physics2D.Raycast(position, direction, rayLength, groundMask).transform != null;
    private bool WallInDirection(Vector2 direction)
    {
        foreach (var ray in rayOrigins.Select(ray => ray.position))
            if (RayHitGround(ray, direction))
                return !IsOnGround();

        return false;
    }
    #endregion

    #region Debug
    private void OnDrawGizmos()
    {
        if (drawDebug)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(GroundCheckPosition, GroundCheckSize);

            foreach (var ray in rayOrigins.Select(ray => ray.position))
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawRay(ray, Vector2.right * rayLength);
                Gizmos.color = Color.yellow;
                Gizmos.DrawRay(ray, Vector2.left * rayLength);
            }
        }
    }

    private void Print(string text)
    {
        if (printDebug)
            Debug.Log(text);
    }
    #endregion
}
