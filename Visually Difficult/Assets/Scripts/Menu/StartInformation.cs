using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartInformation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelName, attemptText;

    private void Awake()
    {
        levelName.text = SceneManager.GetActiveScene().name;
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        attemptText.text = $"Attempt: {DataCollector.DeathCount + 1}";
        Time.timeScale = 0;
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
