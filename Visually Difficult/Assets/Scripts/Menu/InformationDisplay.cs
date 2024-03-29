using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InformationDisplay : MonoBehaviour
{
    private InputAction jumpAction;

    private PlayerController playerController;
    private PauseMenu pauseMenu;

    private StartInformation startScreen;
    private EndInformation endScreen;
    private RunInformation runInfo;

    private bool displayStart;

    public UnityEvent SkippedEndEvent => skippedEndEvent;
    private readonly UnityEvent skippedEndEvent = new();

    private void OnEnable() => EnableSkip();
    private void OnDisable() => DisableSkip();

    private void EnableSkip() => jumpAction.canceled += OnSkip;
    private void DisableSkip() => jumpAction.canceled -= OnSkip;
    
    
    public void ActivateEnd()
    {
        EnableSkip();
        if (pauseMenu != null)
            pauseMenu.enabled = false;
        runInfo.Deactivate();
        endScreen.Activate();
    }
    
    private void Awake()
    {
        startScreen = GetComponentInChildren<StartInformation>();
        endScreen = GetComponentInChildren<EndInformation>();
        runInfo = GetComponentInChildren<RunInformation>();
        jumpAction = GetComponent<PlayerInput>().actions["Jump"];
        startScreen.gameObject.SetActive(false);
        endScreen.gameObject.SetActive(false);
        runInfo.gameObject.SetActive(false);
        displayStart = !(SceneManager.GetActiveScene().buildIndex == 1 && DataCollector.DeathCount == 0);
        DisableSkip();
    }

    //Delayed in the Script Execution Order
    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        pauseMenu = FindObjectOfType<PauseMenu>();
        if (displayStart)
        {
            EnableSkip();
            startScreen.Activate();
            playerController.enabled = false;
        }
        else
        {
            runInfo.Activate();
            if (pauseMenu != null)
                pauseMenu.enabled = true;
        }


        if (pauseMenu != null)
            pauseMenu.enabled = true;
        else
            Debug.LogWarning("Could not find a pause menu");
    }

    private void OnSkip(InputAction.CallbackContext context)
    {
        jumpAction.Reset();
        if (startScreen.gameObject.activeSelf)
        {
            DisableSkip();
            startScreen.Deactivate();
            runInfo.Activate();
            playerController.enabled = true;
            if (pauseMenu != null)
                pauseMenu.enabled = true;
        }
        else if (endScreen.gameObject.activeSelf)
        {
            DisableSkip();
            endScreen.Deactivate();
            skippedEndEvent.Invoke();
        }
    }
}
