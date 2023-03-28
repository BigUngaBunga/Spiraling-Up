using TMPro;
using UnityEngine;

public class RunInformation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI attemptText, time;
    [SerializeField] private int framesPerUpdate = 1;

    private void SetText() => time.text = $"{DataCollector.AttemptTime}s";

    public void Activate()
    {
        gameObject.SetActive(true);
        attemptText.text = attemptText.text = $"Attempt {DataCollector.DeathCount + 1}:";
        InvokeRepeating(nameof(SetText), 0, Time.fixedDeltaTime * framesPerUpdate);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        CancelInvoke();
    }
}
