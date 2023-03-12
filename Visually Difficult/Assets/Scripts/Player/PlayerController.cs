using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float rayLength;

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
    //TODO change to normal variables when debugging is done
    private Vector2 GroundCheckPosition => (Vector2)transform.position + groundCheckOffset * transform.localScale;
    private Vector2 GroundCheckSize => groundCheckSize * transform.localScale;


    [Header("Debug")]
    [SerializeField] private bool drawDebug;
    [SerializeField] private bool printDebug;
    
    private GameObject topRayOrigin, bottomRayOrigin;
    private bool isGrounded;
    private bool justJumped;
    private bool inputDisabled;

    private float jumpTimer;
    private float coyoteTimer;

    private new Rigidbody2D rigidbody;
    private PlayerAnimator animator;

    #region Input
    private InputAction moveAction;
    private InputAction jumpAction;

    private void OnDisable()
    {
        jumpAction.performed -= Jump;
        moveAction.Disable();

    }

    private void OnEnable()
    {
        jumpAction.performed += Jump;
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
        PlayerInput input = GetComponent<PlayerInput>();

        topRayOrigin = transform.Find("TopRay").gameObject;
        bottomRayOrigin = transform.Find("BottomRay").gameObject;

        moveAction = input.actions["Move"];
        jumpAction = input.actions["Jump"];
    }

    public void Kill(string deathReason = "")
    {
        //TODO vänta lite innan man laddar om för att kunna använda någon effekt
        Print(deathReason);
        DataCollector.IncreasePlayerDeaths();
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    private void FixedUpdate()
    {
        coyoteTimer = isGrounded ? 0 : coyoteTimer + Time.fixedDeltaTime;
        AddMovement(moveAction.ReadValue<Vector2>());
        Jumping();
        animator.UpdateAnimation(rigidbody.velocity);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (justJumped || isGrounded || groundMask != (groundMask | (1 << collision.gameObject.layer)))
            return;

        for (int i = 0; i < collision.contactCount; i++)
        {
            if (Vector2.Angle(collision.GetContact(i).normal, Vector2.up) < 46)
            {
                isGrounded = true;
                return;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }

    #region Jumping
    private void Jump(InputAction.CallbackContext context)
    {
        jumpTimer = jumpBuffer;
        justJumped = true;
        Invoke(nameof(ResetJustJumped), Time.fixedDeltaTime);
    }

    private void ResetJustJumped() => justJumped = false;

    private void WallJump(Vector2 jumpDirection)
    {
        animator.WallJump();
        float angle = wallJumpAngle * Mathf.Deg2Rad;
        Vector2 velocity = new Vector2(jumpDirection.x * Mathf.Sin(angle), Mathf.Cos(angle)) * wallJumpVelocity;
        rigidbody.velocity = velocity;

        jumpTimer = -1;
        coyoteTimer = coyoteTime;

        inputDisabled = true;
        StopAllCoroutines();
        StartCoroutine(ReenableInput());
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
            animator.Jump();
            SetY(jumpVelocity);
            StartCoroutine(ContinueJump());

            coyoteTimer = coyoteTime;
            jumpTimer = -1;
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
    private bool RayHit(RaycastHit2D raycast) => raycast.transform != null;

    private bool IsOnGround()
    {
        return Physics2D.OverlapBoxAll(GroundCheckPosition, GroundCheckSize, 0f, groundMask).Length > 0;
    }

    private bool WallInDirection(Vector2 direction)
    {
        var topRay = Physics2D.Raycast(topRayOrigin.transform.position, direction, rayLength, groundMask);
        var bottomRay = Physics2D.Raycast(bottomRayOrigin.transform.position, direction, rayLength, groundMask);

        bool hitTop = RayHit(topRay);
        bool hitBottom = RayHit(bottomRay);

        return !IsOnGround() && ( hitTop || hitBottom);
    }
    #endregion

    #region Debug
    private void OnDrawGizmos()
    {
        if (drawDebug)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(GroundCheckPosition, GroundCheckSize);

            if (topRayOrigin != null && bottomRayOrigin != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawRay(topRayOrigin.transform.position, Vector2.right * rayLength);
                Gizmos.DrawRay(bottomRayOrigin.transform.position, Vector2.right * rayLength);

                Gizmos.color = Color.yellow;
                Gizmos.DrawRay(topRayOrigin.transform.position, Vector2.left * rayLength);
                Gizmos.DrawRay(bottomRayOrigin.transform.position, Vector2.left * rayLength);
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
