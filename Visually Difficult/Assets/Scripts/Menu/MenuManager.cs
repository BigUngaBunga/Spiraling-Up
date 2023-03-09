using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void ExitGame() => Application.Quit();

    public void StartGame()
    {
        //TODO bestäm hur olika testscenarion skall väljas sedan starta spelet
        int tutorialIndex = 1;
        SceneManager.LoadSceneAsync(tutorialIndex);
    }
}
