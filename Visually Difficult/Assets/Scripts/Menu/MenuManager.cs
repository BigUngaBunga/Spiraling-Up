using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Scrollbar presetToggle;
    private AudioPlayer audioPlayer;
    private GraphicalSettings settings;
    private readonly int presets = 3;

    private void Awake()
    {
        audioPlayer = FindObjectOfType<AudioPlayer>();
        CreateGraphicalSettings();        
    }

    private void Start()
    {
        Cursor.visible= true;
        Cursor.lockState = CursorLockMode.Confined;
        pauseMenu.SetActive(false);
        presetToggle.value = (float)Random.Range(0, 3) / (presets - 1);
        audioPlayer.PlayMenuMusic();
    }

    public void ExitGame() => Application.Quit();

    public void StartGame()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenu.SetActive(true);
        audioPlayer.PlayGameMusic();

        int tutorialIndex = 1;
        Debug.Log(settings.ToString());
        DataCollector.Clear();
        SceneManager.LoadSceneAsync(tutorialIndex);
    }

    private void CreateGraphicalSettings()
    {
        GameObject gameObject = new GameObject("Settings", typeof(DontDestroy));
        settings = gameObject.AddComponent<GraphicalSettings>();
        settings.SetGraphicsPreset(0);
    }

    public void UpdateGraphicalPreset()
    {
        settings.SetGraphicsPreset(Mathf.Min((int)(presetToggle.value * presets), presets - 1));
    }
}
