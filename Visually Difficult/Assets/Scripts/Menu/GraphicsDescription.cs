using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsDescription : MonoBehaviour
{
    [SerializeField] private Scrollbar presetToggle;
    private TextMeshProUGUI text;
    private readonly int presets = 3;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateText()
    {
        switch(Mathf.Min((int)(presetToggle.value * presets), presets - 1))
        {
            case 0: text.text = "Medium, Medium, Medium";
                break;
            case 1: text.text = "Low, Medium, High";
                break;
            case 2: text.text = "High, Medium, Low";
                break;
        }
    }
}
