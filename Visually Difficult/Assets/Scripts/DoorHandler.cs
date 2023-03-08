using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorHandler : MonoBehaviour
{
    enum DoorType { Entrance, Exit}
    [SerializeField] private DoorType type;
    [SerializeField] private GameObject playerPrefab;

    private void Awake()
    {
        if (type == DoorType.Entrance)
            Instantiate(playerPrefab, transform.position, Quaternion.identity);
            
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (type == DoorType.Exit && collision.gameObject.CompareTag("Player"))
            SwitchToNextMap();
    }

    private void SwitchToNextMap()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (SceneManager.sceneCountInBuildSettings > sceneIndex + 1)
            SceneManager.LoadSceneAsync(sceneIndex + 1);
        else
            SceneManager.LoadSceneAsync(0);
    }
}
