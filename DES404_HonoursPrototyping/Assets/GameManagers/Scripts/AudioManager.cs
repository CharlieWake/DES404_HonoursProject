using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    public AudioSource musicSourceA;
    public AudioSource musicSourceB;
    private bool isPlayingA = true;

    public AudioSource sfxSource;

    [Header("Music Settings")]
    public float musicFadeDuration;
    public float musicVolume = 1f;
    public float sfxVolume = 1f;

    public bool audioMuted = false;
    public string currentTrack = "";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        musicSourceA.volume = musicVolume;
        musicSourceB.volume = 0f;
        sfxSource.volume = sfxVolume;

        AudioClip mainMenuTrack = Resources.Load<AudioClip>("Audio/Music/MainMenu");

        if (mainMenuTrack != null)
        {
            musicSourceA.clip = mainMenuTrack;
            musicSourceA.Play();
            currentTrack = ("Audio/Music/MainMenu");
        }
        else
        {
            Debug.LogError("Could not find MainMenuTrack");
        }
    }

    public void PlayMusic(AudioClip newClip)
    {
        StopAllCoroutines();
        StartCoroutine(CrossfadeMusic(newClip));
    }

    private IEnumerator CrossfadeMusic(AudioClip newClip)
    {
        AudioSource activeTrack = isPlayingA ? musicSourceA : musicSourceB;
        AudioSource newTrack = isPlayingA ? musicSourceB : musicSourceA;

        newTrack.clip = newClip;
        newTrack.volume = 0f;
        newTrack.Play();

        float time = 0;
        while (time < musicFadeDuration)
        {
            time += Time.unscaledDeltaTime;
            float t = time / musicFadeDuration;
            activeTrack.volume = Mathf.Lerp(musicVolume, 0f, t);
            newTrack.volume = Mathf.Lerp(0f, musicVolume, t);
            yield return null;
        }

        activeTrack.Stop();
        isPlayingA = !isPlayingA;
    }

    public void PlaySFX(AudioClip sfxClip)
    {
        sfxSource.PlayOneShot(sfxClip);
    }

    public void PlayMusicByName(string clipPath)
    {
        if (currentTrack == clipPath) return;
        
        AudioClip track = Resources.Load<AudioClip>(clipPath);
        if (track != null)
        {
            PlayMusic(track);
            currentTrack = clipPath;
        }
        else
        {
            Debug.LogError("Music clip not found at path: " + clipPath);
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        if (isPlayingA)
            musicSourceA.volume = volume;
        else
            musicSourceB.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        sfxSource.volume = volume;
    }

    public void MuteAudio()
    {
        if (audioMuted == false)
        {
            SetMusicVolume(0f);
            SetSFXVolume(0f);
            audioMuted = true;
        }
        else
        {
            SetMusicVolume(1f);
            SetSFXVolume(1f);
            audioMuted = false;
        }
    }
}
