using System.Collections;
using UnityEngine;

namespace FeedTheBeasts.Scripts
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(MeshRenderer))]
    public class StraightProjectile : Projectile
    {
        void Start()
        {
            gameCatalog = GameCatalog.Instance;
        }
        protected override void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            meshRenderer = GetComponent<MeshRenderer>();
            currentSpeed = speed;
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.ANIMAL_TAG))
            {
                StartCoroutine(AudioCoroutine(other));

            }
        }

        void Update()
        {
            transform.Translate(currentSpeed * Time.deltaTime * Vector3.forward);
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
          internal void SetUpSpeed()
        {
            currentSpeed = speed;
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

