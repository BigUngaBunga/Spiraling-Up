using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField]private float soundVolume, musicVolume;
    [SerializeField] private AudioClip menuMusic, gameMusic;
    private AudioSource musicSource, soundSource;

    public float SoundEffectVolume { 
        get { return soundVolume; }

        set { soundVolume = Mathf.Clamp(value, 0, 1); 
            soundSource.volume = soundVolume;
        }
    }
    public float MusicVolume
    {
        get { return musicVolume; }

        set { musicVolume = Mathf.Clamp(value, 0, 1);
            musicSource.volume = musicVolume;
        }
    }

    private void Awake()
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.volume = MusicVolume;
        musicSource.playOnAwake = false;
        soundSource = gameObject.AddComponent<AudioSource>();
        soundSource.volume = SoundEffectVolume;
        soundSource.playOnAwake = false;
    }

    public void PlaySoundEffect(AudioClip clip) => soundSource.PlayOneShot(clip);

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlayMenuMusic() => PlayMusic(menuMusic);
    public void PlayGameMusic() => PlayMusic(gameMusic);
}
