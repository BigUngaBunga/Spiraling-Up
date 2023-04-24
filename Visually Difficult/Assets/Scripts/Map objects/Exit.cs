using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : DoorHandler
{
    [SerializeField] private float endWaitTime = 0.5f;
    [SerializeField] private Vector2 playerEndOffset;

    private InformationDisplay informationDisplay;

    bool hasRun = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        informationDisplay = FindAnyObjectByType<InformationDisplay>();
    }

    protected override void Start()
    {
        base.Start();

        anim.SetTrigger("Close");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasRun && collision.gameObject.CompareTag("Player"))
            StartCoroutine(EndLevel(collision.gameObject));
    }

    private void ReachedEnd()
    {
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
            var dontDestroyObjects = FindObjectsByType(typeof(DontDestroy), FindObjectsInactive.Include, FindObjectsSortMode.None);
            for (int i = dontDestroyObjects.Length - 1; i >= 0; --i)
                Destroy(dontDestroyObjects[i].GameObject());
            SceneManager.LoadSceneAsync(0);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            DataCollector.SaveData();
        }

        hasRun = true;
    }

    IEnumerator EndLevel(GameObject player)
    {
        anim.SetTrigger("Open");
        DataCollector.EndLevel();

        if (player.TryGetComponent(out PlayerController control))
        {
            control.enabled = false;
            var rb = control.GetComponent<Rigidbody2D>();
            rb.isKinematic = true;
            rb.velocity = Vector2.zero;
        }
        yield return StartCoroutine(MoveToEnd(player));
        yield return new WaitForSeconds(endWaitTime);

        ReachedEnd();
    }

    private IEnumerator MoveToEnd(GameObject gameObject)
    {
        float lerpSpeed = 0.1f;
        float lerp = 0;
        Vector3 initalPosition = gameObject.transform.position;
        Vector3 targetPosition = transform.position + (Vector3)playerEndOffset;
        var wait = new WaitForSeconds(Time.fixedDeltaTime);
        do
        {
            lerp += lerpSpeed;
            gameObject.transform.position = Vector3.Lerp(initalPosition, targetPosition, lerp);
            yield return wait;
        } while (lerp < 1);
    }
}
