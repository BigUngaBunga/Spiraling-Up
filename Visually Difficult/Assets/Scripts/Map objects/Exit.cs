using System.Collections;
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

    IEnumerator EndLevel(GameObject player)
    {
        anim.SetTrigger("Open");

        if (player.TryGetComponent(out PlayerController control))
        {
            control.enabled = false;
            var rb = control.GetComponent<Rigidbody2D>();
            rb.isKinematic = true;
            rb.velocity = Vector2.zero;
        }

        player.transform.position = transform.position + (Vector3)playerEndOffset;

        yield return new WaitForSeconds(endWaitTime);

        ReachedEnd();
    }
}
