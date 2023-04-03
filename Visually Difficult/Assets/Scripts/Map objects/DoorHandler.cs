using UnityEngine;

public abstract class DoorHandler : MonoBehaviour
{
    protected enum Type { Entrance = 1, Exit = 2}

    [SerializeField] protected Type type = Type.Entrance;
    
    protected Animator anim;

    protected virtual void Start()
    {
        if (TryGetComponent(out DynamicAnimation dynAnim))
            dynAnim.Initialise();

        anim.SetInteger("Type", (int)type);
    }
}
