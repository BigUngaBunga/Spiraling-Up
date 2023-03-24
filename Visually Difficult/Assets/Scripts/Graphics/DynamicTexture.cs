using UnityEngine;

public class DynamicTexture : MonoBehaviour
{
    [SerializeField] private Texture2D highTexture;
    [SerializeField] private Texture2D midTexture;
    [SerializeField] private Texture2D lowTexture;
    private SpriteRenderer spriteRenderer;
    private VisualUpdater visualUpdater;

    private void Awake()
    {
        if (TryGetComponent(out SpriteRenderer renderer))
            spriteRenderer= renderer;
        else
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        visualUpdater = FindAnyObjectByType<VisualUpdater>();
    }
    void Start()
    {
        spriteRenderer.material.mainTexture = PickTexture();
    }

    private Texture2D PickTexture()
    {
        return visualUpdater.Settings switch
        {
            GraphicalSettings.Setting.High => highTexture,
            GraphicalSettings.Setting.Low => lowTexture,
            _ => midTexture,
        };
    }
}
