using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float rayLength;
    [Range(0, 0.33f)]
    [SerializeField] private float drag;
    [SerializeField] private float acceleration;
    private GameObject topRayOrigin, bottomRayOrigin;
    [SerializeField]private Vector2 lastDirection = Vector2.zero;

    [Header("Jumping")]
    [SerializeField] private float jumpForce;
    [Range(0, 1f)]
    [SerializeField] private float jumpDecay;
    [SerializeField] private float wallJumpForce;
    [Range(0, 90f)]
    [SerializeField] private float wallJumpAngle;


    [Header("Ground check")]
    [SerializeField] private Vector2 groundCheckOffset;
    [SerializeField] private Vector2 groundCheckSize;
    private Vector2 GroundCheckPosition => (Vector2)transform.position + groundCheckOffset;

    [Header("Debug")]
    [SerializeField] private bool drawDebug;
    [SerializeField] private bool printDebug;
    [SerializeField] private bool spaceForWallJump;

    private PlayerInput input;
    private new Rigidbody2D rigidbody;
    
    #region Actions
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
    #endregion

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInput>();

        topRayOrigin = GameObject.Find("TopRay");
        bottomRayOrigin = GameObject.Find("BottomRay");

        moveAction = input.actions["Move"];
        jumpAction = input.actions["Jump"];
    }

    public void Kill()
    {
        //TODO vänta lite innan man laddar om för att kunna använda någon effekt
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    private void FixedUpdate()
    {
        AddMovement(moveAction.ReadValue<Vector2>());
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (spaceForWallJump)
        {
            if (WallInDirection(Vector2.left, true))
                WallJump(Vector2.right);
            else if (WallInDirection(Vector2.right, true))
                WallJump(Vector2.left);
        }
        if (IsGrounded())
        {
            SetY();
            rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            StartCoroutine(ContinueJump());
        }
    }

    private void WallJump(Vector2 jumpDirection)
    {
        float angle = wallJumpAngle / Mathf.Rad2Deg;
        Vector2 force = new Vector2(jumpDirection.x * Mathf.Cos(angle), Mathf.Sin(angle)) * wallJumpForce;
        SetY();
        rigidbody.AddForce(force, ForceMode2D.Impulse);
    }

    private IEnumerator ContinueJump()
    {
        while (jumpAction.IsPressed())
        {
            Print("ContinuedJump");
            yield return new WaitForFixedUpdate();
        }
        Print("Stopped Jump");
        while (rigidbody.velocity.y > 0.1f)
        {
            LerpY(0, jumpDecay);
            yield return new WaitForFixedUpdate();
        }
    }

    private void AddMovement(Vector2 direction)
    {
        if (!spaceForWallJump)
            EvaluateWallJump(direction);
            
        if (!IsGrounded() && WallInDirection(direction)) 
            return;
        if (direction == Vector2.zero){
            LerpX(0, drag); return;}
        //TODO kika på om man borde reducera kontroll i luften
        Print("Is Moving");
        float targetSpeed = direction.x * speed;
        LerpX(targetSpeed, acceleration * Time.fixedDeltaTime);
        lastDirection = direction;
    }

    private void EvaluateWallJump(Vector2 direction)
    {
        if (direction != Vector2.zero && lastDirection == direction * -1 && WallInDirection(lastDirection))
            WallJump(direction);
    }

    #region Velocity
    private void SetX(float speed = 0) => rigidbody.velocity = new Vector2(speed, rigidbody.velocity.y);
    private void LerpX(float targetSpeed, float lerpRate) => SetX(Mathf.Lerp(rigidbody.velocity.x, targetSpeed, lerpRate));
    private void SetY(float speed = 0) => rigidbody.velocity = new Vector2(rigidbody.velocity.x, speed);
    private void LerpY(float targetSpeed, float lerpRate) => SetY(Mathf.Lerp(rigidbody.velocity.y, targetSpeed, lerpRate));
    #endregion

    #region Ground Check
    private bool IsGrounded() => Physics2D.OverlapBox(GroundCheckPosition, groundCheckSize, 0f, groundMask);
    private bool RayHit(RaycastHit2D raycast) => raycast.transform != null;
    private bool WallInDirection(Vector2 direction, bool dontTouchGround = false)
    {
        var topRay = Physics2D.Raycast(topRayOrigin.transform.position, direction, rayLength, groundMask);
        var bottomRay = Physics2D.Raycast(bottomRayOrigin.transform.position, direction, rayLength, groundMask);
        bool hitTop = RayHit(topRay);
        bool hitBottom = dontTouchGround ? (RayHit(bottomRay) && !IsGrounded()) : RayHit(bottomRay);
        return hitTop || hitBottom;
    }
    #endregion

    #region Debug
    private void OnDrawGizmos()
    {
        if (drawDebug)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(GroundCheckPosition, groundCheckSize);

            Gizmos.color = Color.blue;
            Gizmos.DrawRay(topRayOrigin.transform.position, Vector2.right * rayLength);
            Gizmos.DrawRay(bottomRayOrigin.transform.position, Vector2.right * rayLength);

            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(topRayOrigin.transform.position, Vector2.left * rayLength);
            Gizmos.DrawRay(bottomRayOrigin.transform.position, Vector2.left* rayLength);
        }
    }

    private void Print(string text)
    {
        if (printDebug)
            Debug.Log(text);
    }

    #endregion
}
