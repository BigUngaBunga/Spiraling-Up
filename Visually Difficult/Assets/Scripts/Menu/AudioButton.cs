using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AudioButton : MonoBehaviour
{
    private enum AudioType { Sound, Music}

    [SerializeField] private AudioType type;
    [SerializeField] private float volume = 100;
    [SerializeField] private float increment = 10f;
    private AudioPlayer audioPlayer;
    private TextMeshProUGUI buttonText;
    private string initialButtonText;

    private void IncrementVolume()
    {
        volume -= increment;
        if (volume < 0) volume = 100;
        UpdateVolume();
    }

    private void Awake()
    {
        GetComponent<MenuButton>().clickedEvent.AddListener(IncrementVolume);

        audioPlayer = FindObjectOfType<AudioPlayer>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        initialButtonText = buttonText.text;
        UpdateVolume();
    }
    private void UpdateVolume()
    {
        if (type == AudioType.Sound)
            audioPlayer.SoundEffectVolume = volume / 100f;
        else if (type == AudioType.Music)
            audioPlayer.MusicVolume = volume / 100f;
        UpdateButtonText();
    }
    private void UpdateButtonText()
    {
        buttonText.text = initialButtonText + " " + volume.ToString() + '%';
    }
}
