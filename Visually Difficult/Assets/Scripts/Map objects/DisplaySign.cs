using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DisplaySign : MonoBehaviour
{
    [SerializeField] private GameObject inGameSign;
    [SerializeField] private float signScale;
    [SerializeField] private Vector3 offset;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            HandleSign(true); 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            HandleSign(false); 
        }
    }

    private void HandleSign(bool entered)
    {
        Vector3 scale = inGameSign.transform.localScale;
        Vector3 position = inGameSign.transform.localPosition;
        inGameSign.transform.localScale = entered ? scale * signScale : scale / signScale;
        inGameSign.transform.localPosition = entered ? position + offset: position -offset;
    }
}
