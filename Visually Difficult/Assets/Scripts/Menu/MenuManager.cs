using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    private void Start()
    {
        Cursor.visible= true;
        Cursor.lockState = CursorLockMode.Confined;
        pauseMenu.SetActive(false);
    }

    public void ExitGame() => Application.Quit();

    public void StartGame()
    {
        //TODO bestäm hur olika testscenarion skall väljas sedan starta spelet
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        pauseMenu.SetActive(true);

        int tutorialIndex = 1;
        SceneManager.LoadSceneAsync(tutorialIndex);
    }
}
