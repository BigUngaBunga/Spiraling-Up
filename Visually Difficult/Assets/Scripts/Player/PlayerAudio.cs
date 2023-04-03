using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip land;
    [SerializeField] private AudioClip die;

    [SerializeField] private bool playJumpSound;
    [SerializeField] private bool playLandSound;
    [SerializeField] private bool playDeatSound;

    private AudioPlayer audioPlayer;

    private void Awake()
    {
        audioPlayer = FindObjectOfType<AudioPlayer>();
    }

    public void Jump()
    {
        if (playJumpSound && audioPlayer != null)
            audioPlayer.PlaySoundEffect(jump);
    }
    //Here in case we want to add a unique sound effect for wall jumping
    public void WallJump() => Jump();
    public void Land()
    {
        if (playLandSound && audioPlayer != null)
            audioPlayer.PlaySoundEffect(land);
    }
    public void Die()
    {
        if (playDeatSound && audioPlayer != null)
            audioPlayer.PlaySoundEffect(die);
    }
}
