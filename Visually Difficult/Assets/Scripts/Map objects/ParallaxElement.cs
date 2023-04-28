using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class ParallaxElement : MonoBehaviour
{
    [SerializeField] private Vector3 moveScale;
    [SerializeField] private Vector2 bounds;
    [SerializeField] protected Vector3 initialPosition;
    [SerializeField] private bool onlyMoveInX = false;
    [SerializeField] private bool overrideBounds = false;

    private Vector3 Position => transform.position;

    protected Vector3 moveX, moveY;

    void Start()
    {
        initialPosition = transform.position;
        if (!overrideBounds)
        {
            quaternion rotation = transform.rotation;
            transform.rotation = quaternion.identity;
            bounds = GetComponent<SpriteRenderer>().bounds.size;
            transform.rotation = rotation;
        }

        moveX = new Vector3(bounds.x * 2, 0);
        moveY = new Vector3(0, bounds.y * 2);

        Camera.main.gameObject.GetComponent<ParallaxScroller>().AddElement(this);
    }

    public void Move(Vector3 distance)
    {
        if (!onlyMoveInX)
            transform.position = initialPosition + new Vector3(distance.x * moveScale.x, distance.y * moveScale.y);
        else
            transform.position = new Vector3(initialPosition.x + distance.x * moveScale.x, transform.position.y);

        StartCoroutine(MoveInitialPosition(distance));
    }

    IEnumerator MoveInitialPosition(Vector3 distance)
    {
        yield return new WaitForEndOfFrame();

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
