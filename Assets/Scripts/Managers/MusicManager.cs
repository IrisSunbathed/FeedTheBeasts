using System;
using DG.Tweening;
using FeedTheBeasts.Scripts;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    AudioSource audioSource;
    GameCatalog gameCatalog;

    AudioClip audioClip;
    void Start()
    {
        gameCatalog = GameCatalog.Instance;
        audioClip = gameCatalog.GetAudioClip(MusicThemes.Introdution);
        audioSource.resource = audioClip;
        audioSource.Play();
    }
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    internal void PlayMusic(MusicThemes musicThemes)
    {
        StopMusic();
        audioClip = gameCatalog.GetAudioClip(musicThemes);
        audioSource.resource = audioClip;
        audioSource.Play();
    }

    internal void StopMusic()
    {
        audioSource.Stop();
    }

    internal void FadeCurrentMusic(float endvalue, float duration)
    {
        audioSource.DOFade(endvalue, duration);
    }
}

