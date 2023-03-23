using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    enum Direction { Idle, Left, Right }

    [SerializeField] private float minimumFlipVelocity;

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

    public void Jump() => animator.SetTrigger("Jump");
    public void WallJump() => animator.SetTrigger("WallJump");
    public void SetGrounded(bool value) => animator.SetBool("Grounded", value);

    private void SetDirection(float x)
    {
        if (x > minimumFlipVelocity)
            direction = Direction.Right;
        else if (x < -minimumFlipVelocity)
            direction = Direction.Left;
        else
            direction = Direction.Idle;

        animator.SetBool("IsRunning", direction != Direction.Idle);

        if (direction == Direction.Right && transform.localScale.x < 0 || direction == Direction.Left && transform.localScale.x > 0)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

}
