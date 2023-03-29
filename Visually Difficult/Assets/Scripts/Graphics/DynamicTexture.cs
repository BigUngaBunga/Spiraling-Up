using UnityEngine;

public class DynamicTexture : MonoBehaviour
{
    [SerializeField] private Sprite highTexture;
    [SerializeField] private Sprite midTexture;
    [SerializeField] private Sprite lowTexture;
    private SpriteRenderer spriteRenderer;
    private VisualUpdater visualUpdater;

    private void Awake()
    {
        if (TryGetComponent(out SpriteRenderer renderer))
            spriteRenderer = renderer;
        else
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        visualUpdater = FindAnyObjectByType<VisualUpdater>();
    }
    void Start()
    {
        spriteRenderer.sprite = PickTexture();
    }

    private Sprite PickTexture()
    {
        return visualUpdater.Settings switch
        {
            GraphicalSettings.Setting.High => highTexture,
            GraphicalSettings.Setting.Low => lowTexture,
            _ => midTexture,
        };
    }
}
