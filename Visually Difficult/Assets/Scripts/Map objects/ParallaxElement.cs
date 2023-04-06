using UnityEngine;

public class ParallaxElement : MonoBehaviour
{
    [SerializeField] private Vector3 moveScale;
    [SerializeField] private Vector3 initialPosition;
    [SerializeField] private Vector2 bounds;
    private Vector3 Position => gameObject.transform.position;

    private void Start()
    {
        initialPosition = gameObject.transform.position;
        bounds = gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size;
        Camera.main.gameObject.GetComponent<ParallaxScroller>().AddElement(this);
    }

    public void Move(Vector3 distance)
    {
        gameObject.transform.position = initialPosition + new Vector3(distance.x * moveScale.x, distance.y * moveScale.y);
        if (Position.x + bounds.x < distance.x)
            initialPosition += new Vector3(bounds.x * 2 - 1, 0);
        else if(Position.x - bounds.x > distance.x)
            initialPosition -= new Vector3(bounds.x * 2 - 1, 0);

        if (Position.y + bounds.y < distance.y)
            initialPosition += new Vector3(0, bounds.y * 2 - 1);
        else if (Position.y - bounds.y > distance.y)
            initialPosition -= new Vector3(0, bounds.y * 2 - 1);

    }
}
