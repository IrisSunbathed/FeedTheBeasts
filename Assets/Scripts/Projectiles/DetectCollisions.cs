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

<<<<<<< Updated upstream
=======
        float pitch;

        Vector3 projectileBounds;

        public event Action<GameObject> OnInvisible;
        public event Action<DetectCollisions, bool> OnHitAction;
        public event Action<DetectCollisions> OnFedAction;

        public event Action<DetectCollisions> OnMissAction;

        void Start()
        {
            camerasManager = CamerasManager.Instance;
        }
>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
        
=======
        void Update()
        {
          
            var result = camerasManager.IsOutOfBounds(transform.position, projectileBounds);
            if (result.Item1 | result.Item2 | result.Item3 | result.Item4)
            {
                OnMissAction?.Invoke(this);
                InvokeAction();
            }

        }

        internal void InvokeAction()
        {
            OnInvisible?.Invoke(gameObject);
        }

>>>>>>> Stashed changes

        IEnumerator AudioCoroutine(Collider other)
        {
            AnimalHunger feedPoints = other.GetComponent<AnimalHunger>();
            feedPoints.FeedAnimal(tag);
<<<<<<< Updated upstream
=======
            if (feedPoints.CurrentHunger <= 0)
            {
                OnHitAction?.Invoke(this, true);
                OnFedAction?.Invoke(this);
            }
            else
            {

                OnHitAction?.Invoke(this, false);

            }
>>>>>>> Stashed changes
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