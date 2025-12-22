using System.Collections;
using FeedTheBeasts.Scripts;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FXSoundsManager : MonoBehaviour
{
    GameCatalog gameCatalog;

    AudioSource audioSource;


    void Start()
    {
        gameCatalog = GameCatalog.Instance;

    }

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    internal AudioClip GetFXSound(FXTypes fXTypes)
    {
        return gameCatalog.GetFXClip(fXTypes);
    }

    internal bool PlayFX(FXTypes fXTypes, float pitch = 0, bool useAudioSourcePitch = false, bool waitPreviosToEnd = false)
    {
        if (useAudioSourcePitch)
        {
            pitch = audioSource.pitch;
        }

        if (!audioSource.isPlaying)
        {
            PlayWithPitch(fXTypes, pitch);
        }
        else
        {
            if (waitPreviosToEnd)
            {
                StartCoroutine(WaitAudioToEnd(fXTypes, pitch));
            }
        }
        return audioSource.isPlaying;


    }

    private void PlayWithPitch(FXTypes fXTypes, float pitch)
    {
        AudioClip audioClip = GetFXSound(fXTypes);
        audioSource.pitch = pitch;
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    IEnumerator WaitAudioToEnd(FXTypes fXTypes, float pitch)
    {
        yield return new WaitForSeconds(0.1f);
        if (!audioSource.isPlaying)
        {
            PlayWithPitch(fXTypes, pitch);
        }
        else
        {
            StartCoroutine(WaitAudioToEnd(fXTypes, pitch));
        }

    }




}
