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
    [RequireComponent(typeof(Collider), typeof(StraightProjectile))]
    public class DetectCollisions : MonoBehaviour
    {
        AudioSource audioSource;
        MeshRenderer meshRenderer;
        Collider colAnimal;
        CamerasManager camerasManager;

        float pitch;

        float lengthCam;
        float orthographicSize;

        Vector3 projectileBounds;
        float projectileXBoundsSign;

        public event Action<GameObject> OnInvisible;
        public event Action<DetectCollisions, bool> OnHitAction;
        public event Action<DetectCollisions> OnFedAction;

        public event Action<DetectCollisions> OnMissAction;

        void Start()
        {
            camerasManager = CamerasManager.Instance;
            lengthCam = camerasManager.GetCameraLength();
            orthographicSize = camerasManager.OrthographicSize;
        }
        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            meshRenderer = GetComponent<MeshRenderer>();
            colAnimal = GetComponent<Collider>();
            pitch = 0.4f;
            projectileBounds = meshRenderer.bounds.max;

        }


        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.ANIMAL_TAG))
            {
                StartCoroutine(AudioCoroutine(other));
            }
        }

        void Update()
        {
          
            var result = camerasManager.IsOutOfBounds(transform.position, projectileBounds);
            if (result.Item1 | result.Item2 | result.Item3 | result.Item4)
            {
                OnMissAction?.Invoke(this);
                InvokeAction();
            }
            // GetXBounds();
            // GetZBounds();
        }

        // private void GetZBounds()
        // {
        //     if (transform.position.z < -orthographicSize
        //       | transform.position.z > orthographicSize)
        //     {
        //         OnMissAction?.Invoke(this);
        //         InvokeAction();
        //     }
        // }

        // private void GetXBounds()
        // {
        //     projectileXBoundsSign = projectileXBounds * Mathf.Sign(transform.position.x);
        //     if (transform.position.x < -lengthCam + projectileXBoundsSign
        //         | transform.position.x > lengthCam + projectileXBoundsSign)
        //     {
        //         OnMissAction?.Invoke(this);
        //         InvokeAction();
        //     }
        // }

        internal void InvokeAction()
        {
            OnInvisible?.Invoke(gameObject);
        }


        IEnumerator AudioCoroutine(Collider other)
        {
            AnimalHunger feedPoints = other.GetComponent<AnimalHunger>();
            feedPoints.FeedAnimal(tag);
            if (feedPoints.CurrentHunger <= 0)
            {
                OnHitAction?.Invoke(this, true);
                OnFedAction?.Invoke(this);
            }
            else
            {
                OnHitAction?.Invoke(this, false);

            }
            ConfigureAudio(feedPoints.IsPreferred);
            meshRenderer.enabled = false;
            colAnimal.enabled = false;
            StraightProjectile straightProjectile = GetComponent<StraightProjectile>();
            straightProjectile.currentSpeed = 0;

            yield return new WaitForSeconds(audioSource.clip.length);
            InvokeAction();

        }

        private void ConfigureAudio(bool isPreferred)
        {

            if (isPreferred)
            {
                if (pitch <= 1f)
                {
                    pitch += 0.1f;
                }
                else
                {
                    pitch = 0.4f;
                }
                audioSource.pitch = pitch;
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