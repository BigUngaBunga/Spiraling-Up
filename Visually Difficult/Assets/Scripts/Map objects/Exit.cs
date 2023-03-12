using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    bool hasRun = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasRun && collision.gameObject.CompareTag("Player"))
            SwitchToNextMap();
    }

    private void SwitchToNextMap()
    {
        DataCollector.EndLevel();

        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (SceneManager.sceneCountInBuildSettings > sceneIndex + 1)
        {
            SceneManager.LoadSceneAsync(sceneIndex + 1);
        }
        else
        {
            SceneManager.LoadSceneAsync(0);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            DataCollector.SaveData();
        }

        hasRun = true;
    }
}
