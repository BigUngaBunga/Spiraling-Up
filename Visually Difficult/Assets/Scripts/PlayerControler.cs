using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float jumpForce;
    [Range(0, 1f)]
    [SerializeField] private float jumpDecay;
    [SerializeField] private float speed;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float rayLength;
    [Range(0, 0.33f)]
    [SerializeField] private float drag;
    [SerializeField] private float interpolationSpeed;
    private GameObject topRayOrigin, bottomRayOrigin;


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
    //private PlayerInput wallJumpAction; <- Kanske

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
        if (!IsGrounded()) return;
        SetY(0);
        Vector2 force = Vector2.up * jumpForce;
        rigidbody.AddForce(force, ForceMode2D.Impulse);
        Print("Jumped with force: " + force);
        StartCoroutine(ContinueJump());
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
        
        var topRay = Physics2D.Raycast(topRayOrigin.transform.position, direction, rayLength, groundMask);
        var bottomRay = Physics2D.Raycast(bottomRayOrigin.transform.position, direction, rayLength, groundMask);

        if (drawDebug)
        {
            Debug.DrawRay(topRayOrigin.transform.position, direction * rayLength, Color.yellow);
            Debug.DrawRay(bottomRayOrigin.transform.position, direction * rayLength, Color.yellow);
        }

        if (!IsGrounded() && (HitGround(topRay) || HitGround(bottomRay))) 
            return;
        if (direction == Vector2.zero)
        {
            LerpX(0, drag);
            return;
        }

        float targetSpeed = direction.x * speed * Time.deltaTime * forceConstant;
        LerpX(targetSpeed, interpolationSpeed * Time.deltaTime);
    }

    private void SetX(float speed = 0) => rigidbody.velocity = new Vector2(speed, rigidbody.velocity.y);
    private void LerpX(float targetSpeed, float lerpRate) => SetX(Mathf.Lerp(rigidbody.velocity.x, targetSpeed, lerpRate));
    private void SetY(float speed = 0) => rigidbody.velocity = new Vector2(rigidbody.velocity.x, speed);
    private void LerpY(float targetSpeed, float lerpRate) => SetY(Mathf.Lerp(rigidbody.velocity.y, targetSpeed, lerpRate));



    private bool IsGrounded() => Physics2D.OverlapBox(GroundCheckPosition, groundCheckSize, 0f, groundMask);
    private bool HitGround(RaycastHit2D raycast) => raycast.transform != null;

    #region debug
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
