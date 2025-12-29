using UnityEngine;

namespace FeedTheBeasts.Scripts
{

    public abstract class Projectile : MonoBehaviour
    {
        public float speed;

        internal float currentSpeed;


        protected GameCatalog gameCatalog;
        protected AudioSource audioSource;

        protected MeshRenderer meshRenderer;

        protected TrailRenderer trailRenderer;


        protected FoodTypes foodTypes;

        protected abstract void Awake();

    }
}

