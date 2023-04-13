using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField]private float soundVolume, musicVolume;

    public float SoundEffectVolume { 
        get { return soundVolume; }

        set { soundVolume = Mathf.Clamp(value, 0, 1); }
    }
    public float MusicVolume
    {
        get { return musicVolume; }

        set { musicVolume = Mathf.Clamp(value, 0, 1); }
    }

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
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
