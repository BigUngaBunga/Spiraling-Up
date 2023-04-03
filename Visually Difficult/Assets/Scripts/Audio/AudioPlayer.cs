using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public float SoundEffectVolume { get; private set; }
    public float MusicVolume { get; private set; }

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        SoundEffectVolume= 1f;
        MusicVolume= 1f;
    }

    public void PlaySoundEffect(AudioClip clip)
    {
        audioSource.PlayOneShot(clip, SoundEffectVolume);
    }

    public void PlayMusic(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.volume = MusicVolume;
        audioSource.Play();
    }
}
