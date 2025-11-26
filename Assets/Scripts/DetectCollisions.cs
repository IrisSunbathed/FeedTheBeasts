using System.Collections;
using Mono.Cecil;
using UnityEngine;
using UnityEngine.Audio;




namespace FeedTheBeasts.Scripts
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(AudioSource))]
    public class DetectCollisions : MonoBehaviour
    {

        GameCatalog gameCatalog;
        AudioSource audioSource;

        void Start()
        {
            gameCatalog = GameCatalog.Instance;
            audioSource = GetComponent<AudioSource>();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.ANIMAL_TAG))
            {
                StartCoroutine(AudioCoroutine());
                //audioSource.PlayOneShot(gameCatalog.GetFXClip(FXTypes.ClickOnButton));
                AnimalHunger feedPoints = other.GetComponent<AnimalHunger>();
                feedPoints.FeedAnimal(tag);

                gameObject.SetActive(false);
            }
        }
        IEnumerator AudioCoroutine()
        {
            float randomPitch = Random.Range(0.15f, 0.85f);
            audioSource.pitch = randomPitch;
            AudioClip audioClip = gameCatalog.GetFXClip(FXTypes.ClickOnButton);
            audioSource.resource = audioClip;
            audioSource.Play();
            yield return new WaitForSeconds(audioClip.length);
            
        }
    }

}