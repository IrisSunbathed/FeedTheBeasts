using FeedTheBeasts.Scripts;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    AudioSource audioSource;
    GameCatalog gameCatalog;


    void Start()
    {
        gameCatalog = GameCatalog.Instance;
    }
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    internal void PlayMusic(MusicThemes musicThemes)
    {
        AudioClip audioClip = gameCatalog.GetAudioClip(musicThemes);
        audioSource.resource = audioClip;
        audioSource.Play();
    }
}
