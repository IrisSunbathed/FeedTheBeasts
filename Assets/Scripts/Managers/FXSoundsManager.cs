using FeedTheBeasts.Scripts;
using UnityEngine;

public class FXSoundsManager : MonoBehaviour
{
    GameCatalog gameCatalog;

 
    void Start()
    {
        gameCatalog = GameCatalog.Instance;
    }

    internal AudioClip GetFXSound(FXTypes fXTypes)
    {
        return gameCatalog.GetFXClip(FXTypes.ClickOnButton);
    }
<<<<<<< Updated upstream
=======

    internal bool PlayFX(FXTypes fXTypes, float pitch = 1, float volumne = 1, bool waitPreviosToEnd = false)
    {
       
        audioSource.volume = volumne;

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




>>>>>>> Stashed changes
}
