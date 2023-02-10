using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallOutOfWorld : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Fell out of world");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
    }
}
