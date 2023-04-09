using UnityEngine;

public class ParallaxElement : MonoBehaviour
{
    [SerializeField] private Vector3 moveScale;
    [SerializeField] private Vector2 extents;
    [SerializeField] private Vector3 initialPosition;
    
    private void Start()
    {
        initialPosition = transform.position;
        extents = GetComponent<SpriteRenderer>().bounds.extents / 2;
        
        Camera.main.gameObject.GetComponent<ParallaxScroller>().AddElement(this);
    }

    public void Move(Vector3 target)
    {
        if (initialPosition.x + extents.x < target.x)
            initialPosition += new Vector3(extents.x * 2, 0);
        else if(initialPosition.x - extents.x > target.x)
            initialPosition -= new Vector3(extents.x * 2, 0);

        if (initialPosition.y + extents.y < target.y)
            initialPosition += new Vector3(0, extents.y * 2);
        else if (initialPosition.y - extents.y > target.y)
            initialPosition -= new Vector3(0, extents.y * 2);

        transform.position = initialPosition + new Vector3((target.x - initialPosition.x) * moveScale.x, (target.y - initialPosition.y) * moveScale.y);
    }
}
