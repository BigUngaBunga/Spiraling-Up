using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible= true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void ExitGame() => Application.Quit();

    public void StartGame()
    {
        //TODO best�m hur olika testscenarion skall v�ljas sedan starta spelet
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        int tutorialIndex = 1;
        SceneManager.LoadSceneAsync(tutorialIndex);
    }
}
