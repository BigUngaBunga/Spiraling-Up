using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private Button primaryButton;

    private bool isPaused = false;
    private InputAction pauseAction;

    private void Awake()
    {
        pauseAction = GetComponent<PlayerInput>().actions["Menu"];
        menu.SetActive(isPaused);
    }

    private void OnDisable() => pauseAction.performed -= OnPause;
    private void OnEnable() => pauseAction.performed += OnPause;

    private void OnPause(InputAction.CallbackContext context) => TogglePauseMenu();

    public void TogglePauseMenu()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        menu.SetActive(isPaused);
        if (isPaused)
            primaryButton.Select();

        Cursor.visible = isPaused;
        Cursor.lockState = isPaused ? CursorLockMode.Confined : CursorLockMode.Locked;
    }

    public void LoadMainMenu()
    {
        var dontDestroyObjects = FindObjectsByType(typeof(DontDestroy), FindObjectsInactive.Include, FindObjectsSortMode.None);
        for (int i = dontDestroyObjects.Length -1; i >= 0; --i)
            Destroy(dontDestroyObjects[i].GameObject());
        SceneManager.LoadSceneAsync(0);
    }

    public void Restart()
    {
        Debug.Log("Restarting");
        TogglePauseMenu();
        FindAnyObjectByType<PlayerController>().Kill("Restarting");
    }

    public void ExitGame() => Application.Quit();
}
