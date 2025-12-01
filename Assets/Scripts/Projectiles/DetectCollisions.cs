using System.Collections;
using Mono.Cecil;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;




namespace FeedTheBeasts.Scripts
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(MeshRenderer))]
    public class DetectCollisions : MonoBehaviour
    {

        GameCatalog gameCatalog;
        AudioSource audioSource;

        MeshRenderer meshRenderer;


        void Start()
        {
            gameCatalog = GameCatalog.Instance;
        }

        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            meshRenderer = GetComponent<MeshRenderer>();

        }



        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.ANIMAL_TAG))
            {
                StartCoroutine(AudioCoroutine(other));
                //audioSource.PlayOneShot(gameCatalog.GetFXClip(FXTypes.ClickOnButton));

            }
        }

        IEnumerator AudioCoroutine(Collider other)
        {
            ConfigureAudio();
            AnimalHunger feedPoints = other.GetComponent<AnimalHunger>();
            feedPoints.FeedAnimal(tag);
            meshRenderer.enabled = false;
            yield return new WaitForSeconds(audioSource.clip.length);

            gameObject.SetActive(false);
            meshRenderer.enabled = true;


        }

        private void ConfigureAudio()
        {
            float randomPitch = Random.Range(.5f, 1f);
            audioSource.pitch = randomPitch;
            AudioClip audioClip = gameCatalog.GetFXClip(FXTypes.ClickOnButton);
            audioSource.resource = audioClip;
            audioSource.Play();
        }
    }

}