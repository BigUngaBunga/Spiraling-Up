using UnityEngine;
using UnityEngine.SceneManagement;

public class Entrance : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    private void Awake()
    {
        Instantiate(playerPrefab, transform.position, Quaternion.identity);
        DataCollector.StartLevel(SceneManager.GetActiveScene().buildIndex);
    }
}
