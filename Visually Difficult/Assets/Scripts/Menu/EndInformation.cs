using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndInformation : MonoBehaviour
{
    private const char timeEnd = 's';

    [SerializeField] private float goldTime, silverTime, bronzeTime;

    [Header("Assignment")]
    [SerializeField] private TextMeshProUGUI levelName;
    [SerializeField] private TextMeshProUGUI timeText, attemptsText;

    [Space(10)]
    [SerializeField] private Image goldMedal;
    [SerializeField] private Image silverMedal, bronzeMedal;
    [SerializeField] private TextMeshProUGUI goldText, silverText, bronzeText;

    [Header("Medal Effect")]
    [SerializeField] private float inactiveAlpha = 0.33f;
    [SerializeField] private float medalScaling = 1.5f;
    [SerializeField] private float medalLerpDuration = 0.5f;
    [SerializeField] private float delayBetweenMedals = 0.25f;
    [SerializeField] MedalParticleEffect goldEarnEffect, silverEarnEffect, bronzeEarnEffect;

    [Header("Medal sounds")]
    [SerializeField] private List<AudioClip> audioClips;
    private AudioPlayer audioPlayer;
    private int playedIndex = -1;

    private void Awake()
    {
        audioPlayer = FindObjectOfType<AudioPlayer>();
        levelName.text = SceneManager.GetActiveScene().name;
        goldText.text = goldTime.ToString() + timeEnd;
        silverText.text = silverTime.ToString() + timeEnd;
        bronzeText.text = bronzeTime.ToString() + timeEnd;
    }

    public void Activate()
    {
        Time.timeScale = 0;
        gameObject.SetActive(true);
        attemptsText.text = $"Attempts: {DataCollector.DeathCount + 1}";
        timeText.text = $"Time: {DataCollector.AttemptTime}{timeEnd}";

        StartCoroutine(UpdateMedals());
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    private void SetMedalAlpha(Image medal, float value)
    {
        var colour = medal.color;
        colour.a = value;
        medal.color = colour;
    }

    private void PlaySound(int soundIndex)
    {
        if (audioPlayer != null && playedIndex < soundIndex)
        {
            playedIndex = soundIndex;
            audioPlayer.PlaySoundEffect(audioClips[soundIndex < audioClips.Count ? soundIndex : audioClips.Count - 1]);
        }
    }

    IEnumerator UpdateMedals()
    {
        var wait = new WaitForSecondsRealtime(delayBetweenMedals);

        SetMedalAlpha(goldMedal, inactiveAlpha);
        SetMedalAlpha(silverMedal, inactiveAlpha);
        SetMedalAlpha(bronzeMedal, inactiveAlpha);

        Image currentMedal;
        float currentTime;
        MedalParticleEffect currentEffect;
        for (int i = 0; i < 3; i++)
        {
            currentMedal = i == 0 ? bronzeMedal : (i == 1 ? silverMedal : goldMedal);
            currentTime = i == 0 ? bronzeTime : (i == 1 ? silverTime : goldTime);
            currentEffect = i == 0 ? bronzeEarnEffect : (i == 1 ? silverEarnEffect : goldEarnEffect);

            bool coloured = false;

            float startTime = Time.unscaledTime;
            float passedTime;
            float lerpValue = 0;

            Vector3 originalScale = currentMedal.transform.localScale;
            Vector3 largeScale = originalScale * medalScaling;

            if (DataCollector.AttemptTime > currentTime)
                continue;

            while (lerpValue < 1)
            {
                passedTime = Time.unscaledTime - startTime;
                lerpValue = Mathf.Clamp01(passedTime / medalLerpDuration);

                currentMedal.transform.localScale = Vector3.Lerp(originalScale, largeScale, (lerpValue < 0.5f ? lerpValue : 1 - lerpValue) * 2);
                if (lerpValue >= 0.5f && !coloured)
                {
                    SetMedalAlpha(currentMedal, 1);
                    PlaySound(i);

                    if (currentEffect.particleEffect != null)
                    {
                        Vector3 position = Camera.main.ScreenToWorldPoint(currentMedal.transform.position);
                        position.z = 100;
                        StartCoroutine(DestroyRealtime(Instantiate(currentEffect.particleEffect, position, Quaternion.identity), currentEffect.particlesDuration));
                    }

                    coloured = true;
                }

                yield return null;
            }

            yield return wait;
        }
    }

    IEnumerator DestroyRealtime(GameObject toDestroy, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        Destroy(toDestroy);
    }
}

[System.Serializable]
struct MedalParticleEffect
{
    public GameObject particleEffect;
    public float particlesDuration;
}