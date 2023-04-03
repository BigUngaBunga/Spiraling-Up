using UnityEngine;

public class DynamicAnimation : MonoBehaviour
{
    [SerializeField] private RuntimeAnimatorController highAnimation;
    [SerializeField] private RuntimeAnimatorController midAnimation;
    [SerializeField] private RuntimeAnimatorController lowAnimation;

    private Animator animator;
    private VisualUpdater visualUpdater;

    void Awake()
    {
        if (TryGetComponent(out Animator animator))
            this.animator = animator;
        else
            this.animator = GetComponentInChildren<Animator>();

        visualUpdater = FindAnyObjectByType<VisualUpdater>();
    }

    public void Initialise()
    {
        animator.runtimeAnimatorController = PickAnimatorController();
    }

    RuntimeAnimatorController PickAnimatorController()
    {
        return visualUpdater.Settings switch
        {
            GraphicalSettings.Setting.High => highAnimation,
            GraphicalSettings.Setting.Low => lowAnimation,
            _ => midAnimation,
        };
    }
}
