using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deadly : MonoBehaviour
{
    [SerializeField] string deathReason = "Unspecified";

    private void OnCollisionEnter2D(Collision2D collision) => KillIfPlayer(collision.gameObject);

    private void OnTriggerEnter2D(Collider2D collision) => KillIfPlayer(collision.gameObject);

    private void KillIfPlayer(GameObject collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log(deathReason);
            collision.GetComponent<PlayerController>().Kill();
        }
    }
}
