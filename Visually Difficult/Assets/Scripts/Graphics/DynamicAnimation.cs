using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class DynamicAnimation : MonoBehaviour
{
    [SerializeField] private AnimatorController highAnimation;
    [SerializeField] private AnimatorController midAnimation;
    [SerializeField] private AnimatorController lowAnimation;
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

    private AnimatorController PickAnimatorController()
    {
        return visualUpdater.Settings switch
        {
            GraphicalSettings.Setting.High => highAnimation,
            GraphicalSettings.Setting.Low => midAnimation,
            _ => lowAnimation,
        };
    }
}
