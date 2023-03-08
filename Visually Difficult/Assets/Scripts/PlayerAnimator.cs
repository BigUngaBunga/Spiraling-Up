using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    enum Direction{ Idle, Left, Right }

    [SerializeField] private bool turnWithVelocity;

    private Animator animator;
    private Direction direction;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void UpdateAnimation(Vector2 direction)
    {
        SetDirection(direction.x);
    }

    public void Jump()
    {

    }

    public void WallJump()
    {

    }

    private void SetDirection(float x)
    {
        if (x > 0)
            direction = Direction.Left;
        else if (x < 0)
            direction = Direction.Right;
        else
            direction = Direction.Idle;

        animator.SetBool("IsRunning", direction != Direction.Idle);

        if (direction == Direction.Left && transform.localScale.x < 0 || direction == Direction.Right && transform.localScale.x > 0)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

}
