using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    private InformationDisplay informationDisplay;
    bool hasRun = false;

    private void Awake()
    {
        informationDisplay = FindAnyObjectByType<InformationDisplay>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasRun && collision.gameObject.CompareTag("Player"))
            ReachedEnd();
            //SwitchToNextMap();
    }

    private void ReachedEnd()
    {
        DataCollector.EndLevel();
        informationDisplay.SkippedEndEvent.AddListener(SwitchToNextMap);
        informationDisplay.ActivateEnd();
    }

    private void SwitchToNextMap()
    {
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
