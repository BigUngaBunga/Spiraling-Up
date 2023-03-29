using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndInformation : MonoBehaviour
{

    [SerializeField] private float goldTime, silverTime, bronzeTime;
    [SerializeField] private Image goldMedal, silverMedal, bronzeMedal;
    [SerializeField] private float inactiveAlpha = 0.33f;
    [SerializeField] private TextMeshProUGUI levelName, timeText, attemptsText;
    [SerializeField] private TextMeshProUGUI goldText, silverText, bronzeText;
    private char timeEnd = 's';
    private void Awake()
    {
        levelName.text = SceneManager.GetActiveScene().name;
        goldText.text = goldTime.ToString() + timeEnd;
        silverText.text = silverTime.ToString() + timeEnd;
        bronzeText.text = bronzeTime.ToString() + timeEnd;
    }

    public void Activate()
    {
        Time.timeScale = 0;
        gameObject.SetActive(true);
        attemptsText.text = $"Attempts: {DataCollector.DeathCount + 1}";
        timeText.text = $"Time: {DataCollector.AttemptTime}{timeEnd}";
        GotMedal(goldMedal, goldTime);
        GotMedal(silverMedal, silverTime);
        GotMedal(bronzeMedal, bronzeTime);
    }

    private void GotMedal(Image medal, float timeRequirement)
    {
        if (timeRequirement < DataCollector.AttemptTime) {
            //NO MEDOOL =(
            var colour = medal.color;
            colour.a = inactiveAlpha;
            medal.color = colour;
        }
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
