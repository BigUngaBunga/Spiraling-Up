using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask groundMask;

    [SerializeField] private Vector2 groundCheckOffset;
    [SerializeField] private Vector2 groundCheckSize;
    private Vector2 GroundCheckPosition => (Vector2)transform.position + groundCheckOffset;


    private PlayerInput input;
    private Rigidbody2D rigidbody;
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

        moveAction = input.actions["Move"];
        jumpAction = input.actions["Jump"];

        jumpAction.performed += Jump;
    }

    private void FixedUpdate()
    {
        AddMovement(moveAction.ReadValue<Vector2>());
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (!Physics2D.OverlapBox(GroundCheckPosition, groundCheckSize, 0f, groundMask)) return;
        Vector2 force = Vector2.up * jumpForce;
        ResetY();
        rigidbody.AddForce(force, ForceMode2D.Impulse);
        Debug.Log("Jumped with force: " + force);
    }

    private void AddMovement(Vector2 direction)
    {
        //TODO använd lerp för mjukare rörese
        ResetX();
        Vector2 speed = direction * this.speed * Time.deltaTime * forceConstant;
        rigidbody.velocity = new Vector2(speed.x, rigidbody.velocity.y);
        if (direction != Vector2.zero) { Debug.Log("Set speed: " + speed); }
    }

    private void ResetX() => rigidbody.velocity.Set(0, rigidbody.velocity.y);
    private void ResetY() => rigidbody.velocity.Set(rigidbody.velocity.x, 0);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(GroundCheckPosition, groundCheckSize);
    }
}
