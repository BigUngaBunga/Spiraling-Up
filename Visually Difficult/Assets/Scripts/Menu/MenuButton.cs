using System.Collections;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private Vector3 addedScale = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField] private int lerpNumber = 5;
    private Vector3 initialScale;

    private void Awake() => initialScale = transform.localScale;
    private void OnDisable() => ResetScale();
    public void ResetScale() => transform.localScale = initialScale;

    public void ScaleUp() {
        StopCoroutine(nameof(LerpScale));
        StartCoroutine(LerpScale(initialScale + addedScale)); 
    }

    public void ScaleDown() {
        StopCoroutine(nameof(LerpScale));
        StartCoroutine(LerpScale(initialScale));
    }


    private IEnumerator LerpScale(Vector3 targetScale)
    {
        var wait = new WaitForSecondsRealtime(0.016f);
        Vector3 currentScale = transform.localScale;
        for (int i = 0; i <= lerpNumber; i++)
        {
            transform.localScale = Vector3.Lerp(currentScale, targetScale, (float)i/lerpNumber);
            yield return wait;
        }
    }
}
