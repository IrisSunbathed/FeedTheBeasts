using System;
using System.Collections;
using Mono.Cecil;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;




namespace FeedTheBeasts.Scripts
{
    [RequireComponent(typeof(MeshFilter), typeof(AudioSource), typeof(MeshRenderer))]
    [RequireComponent(typeof(Collider))]
    public class DetectCollisions : MonoBehaviour
    {
        AudioSource audioSource;
        MeshRenderer meshRenderer;
        Collider colAnimal;

        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            meshRenderer = GetComponent<MeshRenderer>();
            colAnimal = GetComponent<Collider>();

        }



        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.ANIMAL_TAG))
            {
                StartCoroutine(AudioCoroutine(other));
                //Proviver.AddCollisionPoint()

            }
        }

        

        IEnumerator AudioCoroutine(Collider other)
        {
            AnimalHunger feedPoints = other.GetComponent<AnimalHunger>();
            feedPoints.FeedAnimal(tag);
            ConfigureAudio(feedPoints.IsPreferred);
            meshRenderer.enabled = false;
            colAnimal.enabled = false;
            yield return new WaitForSeconds(audioSource.clip.length);
            gameObject.SetActive(false);
            meshRenderer.enabled = true;
            colAnimal.enabled = true;
        }

        private void ConfigureAudio(bool isPreferred)
        {

            if (isPreferred)
            {
                float randomPitch = Random.Range(.5f, 1f);
                audioSource.pitch = randomPitch;
                audioSource.volume = 1f;
            }
            else
            {
                float randomPitch = Random.Range(.1f, .25f);
                audioSource.pitch = randomPitch;
                audioSource.volume = 0.5f;
            }
            audioSource.Play();
        }
    }

}