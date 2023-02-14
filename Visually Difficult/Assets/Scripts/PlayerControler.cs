using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float rayLength;
    [Range(0, 0.33f)]
    [SerializeField] private float drag;
    [SerializeField] private float interpolationSpeed;
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

    private PlayerInput input;
    private new Rigidbody2D rigidbody;
    private float forceConstant = 100f;
    #region Actions

    private InputAction moveAction;
    private InputAction jumpAction;

    #endregion

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInput>();

        topRayOrigin = GameObject.Find("TopRay");
        bottomRayOrigin = GameObject.Find("BottomRay");

        moveAction = input.actions["Move"];
        jumpAction = input.actions["Jump"];

        jumpAction.performed += Jump;
    }

    private void OnDestroy()
    {
        jumpAction.performed -= Jump;
    }

    private void FixedUpdate()
    {
        AddMovement(moveAction.ReadValue<Vector2>());
    }

    private void Jump(InputAction.CallbackContext context)
    {
        //TODO kolla alltid vid sidorna oberoende riktning för att förbättra spelarkontroll
        if (WallInDirection(lastDirection))
             WallJump();
        else if (IsGrounded())
        {
            SetY();
            rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            StartCoroutine(ContinueJump());
        }        
    }

    private void WallJump()
    {
        float angle = wallJumpAngle / Mathf.Rad2Deg;
        Vector2 force = new Vector2(-lastDirection.x * Mathf.Cos(angle), Mathf.Sin(angle)) * wallJumpForce;
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
        //TODO kika på om man borde reducera kontroll i luften
        if (!IsGrounded() && WallInDirection(direction)) 
            return;
        if (direction == Vector2.zero)
        {
            LerpX(0, drag);
            return;
        }
        Print("Is Moving");
        lastDirection = direction;
        float targetSpeed = direction.x * speed * Time.deltaTime * forceConstant;
        LerpX(targetSpeed, interpolationSpeed * Time.deltaTime);
    }

    #region Velocity
    private void SetX(float speed = 0) => rigidbody.velocity = new Vector2(speed, rigidbody.velocity.y);
    private void LerpX(float targetSpeed, float lerpRate) => SetX(Mathf.Lerp(rigidbody.velocity.x, targetSpeed, lerpRate));
    private void SetY(float speed = 0) => rigidbody.velocity = new Vector2(rigidbody.velocity.x, speed);
    private void LerpY(float targetSpeed, float lerpRate) => SetY(Mathf.Lerp(rigidbody.velocity.y, targetSpeed, lerpRate));
    #endregion

    #region Ground Check
    private bool IsGrounded() => Physics2D.OverlapBox(GroundCheckPosition, groundCheckSize, 0f, groundMask);
    private bool HitGround(RaycastHit2D raycast) => raycast.transform != null;
    private bool WallInDirection(Vector2 direction)
    {
        var topRay = Physics2D.Raycast(topRayOrigin.transform.position, direction, rayLength, groundMask);
        var bottomRay = Physics2D.Raycast(bottomRayOrigin.transform.position, direction, rayLength, groundMask);

        if (drawDebug)
        {
            Debug.DrawRay(topRayOrigin.transform.position, direction * rayLength, Color.yellow);
            Debug.DrawRay(bottomRayOrigin.transform.position, direction * rayLength, Color.yellow);
        }
        return HitGround(topRay) || HitGround(bottomRay);
    }
    #endregion

    #region Debug
    private void OnDrawGizmos()
    {
        if (drawDebug)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(GroundCheckPosition, groundCheckSize);
        }
    }

    private void Print(string text)
    {
        if (printDebug)
            Debug.Log(text);
    }

    #endregion
}
