using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void ExitGame() => Application.Quit();

    public void StartGame()
    {
        //TODO best�m hur olika testscenarion skall v�ljas sedan starta spelet
        int tutorialIndex = 1;
        SceneManager.LoadSceneAsync(tutorialIndex);
    }
}
