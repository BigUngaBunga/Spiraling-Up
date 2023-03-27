using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartInformation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelName, attemptsText;

    private void Awake()
    {
        levelName.text = SceneManager.GetActiveScene().name;
        attemptsText.text = $"Attempt: {DataCollector.DeathCount + 1}";
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
