using System.Collections.Generic;
using UnityEngine;

public class ParallaxScroller : MonoBehaviour
{
    [SerializeField] private List<ParallaxElement> elements = new List<ParallaxElement>();

    public void MoveElementsTo(Vector3 target)
    {
        foreach (var element in elements)
            element.Move(target);
    }

    public void AddElement(ParallaxElement element) => elements.Add(element);
}
