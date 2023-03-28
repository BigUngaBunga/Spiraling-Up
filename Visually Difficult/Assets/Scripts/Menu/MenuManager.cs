using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Scrollbar presetToggle;
    private GraphicalSettings settings;
    private void Start()
    {
        Cursor.visible= true;
        Cursor.lockState = CursorLockMode.Confined;
        pauseMenu.SetActive(false);
        CreateGraphicalSettings();
    }

    public void ExitGame() => Application.Quit();

    public void StartGame()
    {

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenu.SetActive(true);

        int tutorialIndex = 1;
        DataCollector.StartLevel(tutorialIndex);
        SceneManager.LoadSceneAsync(tutorialIndex);
    }

    private void CreateGraphicalSettings()
    {
        GameObject gameObject = new GameObject("Settings", typeof(DontDestroy));
        settings = gameObject.AddComponent<GraphicalSettings>();
        settings.SetGraphicsPreset(Random.Range(0, 3));
    }

    public void UpdateGraphicalPreset()
    {
        int presets = 3;
        settings.SetGraphicsPreset(Mathf.Min((int)(presetToggle.value * presets), presets - 1));
    }
}
