using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FeedTheBeasts.Scripts
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(MeshRenderer))]
    public class StraightProjectile : Projectile
    {

        public event Action<StraightProjectile> OnInvisible;
        CamerasManager camerasManager;
        float projectileXBounds;

        float projectileXBoundsSign;
        float lengthCam;
        float orthographicSize;
        void Start()
        {
            gameCatalog = GameCatalog.Instance;
            camerasManager = CamerasManager.Instance;
            lengthCam = camerasManager.GetCameraLength();
            orthographicSize = camerasManager.OrthographicSize;
        }
        protected override void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            meshRenderer = GetComponent<MeshRenderer>();
            SetUpSpeed();
            projectileXBounds = meshRenderer.bounds.max.x;
        }

        internal void SetUpSpeed()
        {
            currentSpeed = speed;
        }

        // protected override void OnTriggerEnter(Collider other)
        // {
        //     if (other.CompareTag(Constants.ANIMAL_TAG))
        //     {
        //         StartCoroutine(AudioCoroutine(other));

        //     }
        // }

        void Update()
        {
            transform.Translate(currentSpeed * Time.deltaTime * Vector3.forward);
            GetXBounds();
            GetZBounds();
        }


        private void GetZBounds()
        {
            if (transform.position.z < -orthographicSize
              | transform.position.z > orthographicSize)
            {
                InvokeAction();
            }
        }

        internal void InvokeAction()
        {
            OnInvisible?.Invoke(this);
        }

        private void GetXBounds()
        {
            projectileXBoundsSign = projectileXBounds * Mathf.Sign(transform.position.x);
            if (transform.position.x < -lengthCam + projectileXBoundsSign
                | transform.position.x > lengthCam + projectileXBoundsSign)
            {
                InvokeAction();
            }
        }
        // IEnumerator AudioCoroutine(Collider other)
        // {
        //     ConfigureAudio();
        //     AnimalHunger feedPoints = other.GetComponent<AnimalHunger>();
        //     feedPoints.FeedAnimal(tag);
        //     meshRenderer.enabled = false;
        //     yield return new WaitForSeconds(audioSource.clip.length);

        //     gameObject.SetActive(false);
        //     meshRenderer.enabled = true;


        // }

        private void ConfigureAudio()
        {
            float randomPitch = Random.Range(.5f, 1f);
            audioSource.pitch = randomPitch;
            AudioClip audioClip = gameCatalog.GetFXClip(FXTypes.ClickOnButton);
            audioSource.resource = audioClip;
            audioSource.Play();
            OnInvisible?.Invoke(this);
        }

    }



}

