using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FeedTheBeasts.Scripts
{
    [RequireComponent(typeof(AudioSource), typeof(MeshRenderer))]
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
            SetUpSpeed();
        }

        internal void SetUpSpeed()
        {
            currentSpeed = speed;
        }


        void Update()
        {
            transform.Translate(currentSpeed * Time.deltaTime * Vector3.forward);

        }
    }
}

