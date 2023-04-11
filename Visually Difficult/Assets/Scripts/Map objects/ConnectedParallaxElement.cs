using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectedParallaxElement : ParallaxElement
{

    [SerializeField] private ConnectedParallaxElement nextElement, previousElement;
    [SerializeField] private Transform connectionStart, connectionEnd;


    private ConnectedParallaxElement GetFirstElement() {
        if (previousElement == null)
            return this;
        return previousElement.GetFirstElement(); 
    }

    private ConnectedParallaxElement GetLastElement() { 
        if (nextElement == null) 
            return this;
        return nextElement.GetLastElement();
    }

    private Vector3 GetStart() => GetFirstElement().connectionStart.position;

    private Vector3 GetEnd() => GetLastElement().connectionEnd.position;

    protected override void MoveDown()
    {
        if (nextElement != null) return;

        initialPosition += GetStart() - connectionEnd.position;
        nextElement = GetFirstElement();
        nextElement.previousElement = this;
        previousElement.nextElement = null;
        previousElement = null;
    }

    protected override void MoveUp()
    {
        if(previousElement != null) return;

        initialPosition += GetEnd() - connectionStart.position;
        previousElement = GetLastElement();
        previousElement.nextElement = this;
        nextElement.previousElement = null;
        nextElement = null;
    }
}
