using UnityEngine;

namespace FeedTheBeasts.Scripts
{

    public abstract class Projectile : MonoBehaviour
    {
        public float speed;

        protected float currentSpeed;
        protected abstract void Awake();

        protected abstract void OnTriggerEnter(Collider other);

        protected GameCatalog gameCatalog;
        protected AudioSource audioSource;

        protected MeshRenderer meshRenderer;

    }
}

