using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ParallaxElement : MonoBehaviour
{
    [SerializeField] private Vector3 moveScale;
    [SerializeField] private Vector2 bounds;
    [SerializeField] protected Vector3 initialPosition;
    private Vector3 Position => transform.position;

    protected Vector3 moveX, moveY;

    protected virtual void Start()
    {
        initialPosition = transform.position;
        bounds = GetComponent<SpriteRenderer>().bounds.size;
        moveX = new Vector3(bounds.x * 2 - 1, 0);
        moveY = new Vector3(0, bounds.y * 2 - 1);

        Camera.main.gameObject.GetComponent<ParallaxScroller>().AddElement(this);
    }

    public void Move(Vector3 distance)
    {
        gameObject.transform.position = initialPosition + new Vector3(distance.x * moveScale.x, distance.y * moveScale.y);
        if (Position.x + bounds.x < distance.x)
            initialPosition += moveX;
        else if (Position.x - bounds.x > distance.x)
            initialPosition -= moveX;
        if (Position.y + bounds.y < distance.y)
            MoveUp();
        else if (Position.y - bounds.y > distance.y)
            MoveDown();
    }

    protected virtual void MoveUp() => initialPosition += moveY;

    protected virtual void MoveDown() => initialPosition -= moveY;
}
