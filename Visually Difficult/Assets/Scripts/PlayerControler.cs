using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{
    private PlayerInput input;
    private Rigidbody2D rigidbody;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
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
        Vector2 force = Vector2.up * jumpForce * Time.deltaTime * forceConstant;
        rigidbody.AddForce(force, ForceMode2D.Impulse);
        Debug.Log("Jumped with force: " + force);
    }

    private void AddMovement(Vector2 direction)
    {
        Vector2 force = direction * speed * Time.deltaTime * forceConstant;
        rigidbody.AddForce(force);
        if (direction != Vector2.zero) { Debug.Log("Added force: " + force); }
    }
}
