using UnityEngine;

public class DynamicAnimation : MonoBehaviour
{
    [SerializeField] private RuntimeAnimatorController highAnimation;
    [SerializeField] private RuntimeAnimatorController midAnimation;
    [SerializeField] private RuntimeAnimatorController lowAnimation;
    private Animator animator;
    private VisualUpdater visualUpdater;

    private void Awake()
    {
        if (TryGetComponent(out Animator animator))
            this.animator = animator;
        else
            this.animator = GetComponentInChildren<Animator>();
        visualUpdater = FindAnyObjectByType<VisualUpdater>();
    }
    void Start()
    {
        animator.runtimeAnimatorController = PickAnimatorController();
    }

    private RuntimeAnimatorController PickAnimatorController()
    {
        return visualUpdater.Settings switch
        {
            GraphicalSettings.Setting.High => highAnimation,
            GraphicalSettings.Setting.Low => lowAnimation,
            _ => midAnimation,
        };
    }
}
