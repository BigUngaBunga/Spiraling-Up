using UnityEngine;
using UnityEngine.SceneManagement;

public class Entrance : DoorHandler
{
    [SerializeField] private GameObject playerPrefab;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        Instantiate(playerPrefab, transform.position, Quaternion.identity);
        DataCollector.StartLevel(SceneManager.GetActiveScene().buildIndex);
    }

    protected override void Start()
    {
        base.Start();

        anim.SetTrigger("Close");
    }
}
