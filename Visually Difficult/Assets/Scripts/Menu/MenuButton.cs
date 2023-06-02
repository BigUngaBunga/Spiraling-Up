using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuButton : MonoBehaviour
{
    public UnityEvent clickedEvent = new UnityEvent();

    [SerializeField] private Vector3 addedScale = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField] private int lerpNumber = 5;
    [SerializeField] private AudioClip clickButton, selectButton;

    private AudioPlayer audioPlayer;
    private Vector3 initialScale;
    private Vector3 largeScale => initialScale + addedScale;
    private bool isLerping = false;
    private bool isSelected = false;

    private void Awake()
    {
        audioPlayer = FindObjectOfType<AudioPlayer>();
        initialScale = transform.localScale;
    }
    private void OnDisable() => ResetScale();
    public void ResetScale() => transform.localScale = initialScale;
    public void OnSelected()
    {
        isSelected = true;
        audioPlayer.PlaySoundEffect(selectButton);
        StartLerp();
    }
    public void OnDeselected()
    {
        isSelected = false;
        StartLerp();
    }
    public void OnPointerDown()
    {
        clickedEvent.Invoke();
    }

    public void OnClick()
    {
        audioPlayer.PlaySoundEffect(clickButton);
        
    }

    private void StartLerp()
    {
        if (!isLerping)
            StartCoroutine(LerpScale(isSelected ? largeScale : initialScale));
    }


    private IEnumerator LerpScale(Vector3 targetScale)
    {
        isLerping = true;
        var wait = new WaitForSecondsRealtime(0.016f);
        Vector3 currentScale = transform.localScale;
        for (int i = 0; i <= lerpNumber; i++)
        {
            transform.localScale = Vector3.Lerp(currentScale, targetScale, (float)i/lerpNumber);
            yield return wait;
        }
        isLerping = false;

        if (isSelected && targetScale != largeScale)
            StartCoroutine(LerpScale(largeScale));
        else if (!isSelected && targetScale != initialScale)
            StartCoroutine(LerpScale(initialScale));
    }
}
