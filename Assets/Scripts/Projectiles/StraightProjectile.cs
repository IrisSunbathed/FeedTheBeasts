using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using RangeAttribute = UnityEngine.RangeAttribute;

namespace FeedTheBeasts.Scripts
{
    [RequireComponent(typeof(AudioSource), typeof(MeshRenderer))]
    [RequireComponent(typeof(StraightProjectile))]
    [RequireComponent(typeof(DetectCollisions))]
    public class StraightProjectile : Projectile
    {
        [Header("TrackAnimals Configuration")]
        [SerializeField, Range(0.5f, 1.5f), Tooltip("If the TrackAnimals Power UP is activated, the amount time for every check")]
        float nearbyAnimalCheck;
        [SerializeField, Range(5f, 15f)]
        float sphereRadius;
        internal Transform followedAnimal;
        Collider[] enteredAnimals;
        Vector3 direction;
        int numberFollowedAnimals;
        bool doesFollowAnimals;
        DetectCollisions detectCollisions;




        void Start()
        {
            gameCatalog = GameCatalog.Instance;

        }
        protected override void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            meshRenderer = GetComponent<MeshRenderer>();
            detectCollisions = GetComponent<DetectCollisions>();
            direction = Vector3.forward;
            enteredAnimals = new Collider[4];

        }

        internal void SetUp(float speed, Transform playerTransform, bool followAnimals = false)
        {
            currentSpeed = speed;
            doesFollowAnimals = followAnimals;
            transform.rotation = playerTransform.transform.rotation;
            direction = Vector3.forward;
        }


        void Update()
        {
            CheckPowerUp();

            transform.Translate(currentSpeed * Time.deltaTime * direction);

        }

        private void CheckPowerUp()
        {
            if (!doesFollowAnimals) return;

            if (followedAnimal != null)
            {

                direction = followedAnimal.position - transform.position;
                transform.rotation = Quaternion.LookRotation(direction);
            }
            else
            {
                TryFindObjective();
            }
        }

        private void TryFindObjective()
        {
            numberFollowedAnimals = Physics.OverlapSphereNonAlloc(transform.position, sphereRadius, enteredAnimals, LayerMask.GetMask(Constants.ANIMAL_TAG));
            if (numberFollowedAnimals > 0)
            {
                Collider closestAnimal = enteredAnimals[0];

                if (closestAnimal != null && closestAnimal.gameObject.TryGetComponent(out AnimalHunger animalHunger))
                {
                    detectCollisions.enabled = false;
                    followedAnimal = gameObject.CompareTag(animalHunger.preferredFood.ToString()) ? closestAnimal.transform : null;
                }


            }
            else
            {
                if (!detectCollisions.enabled)
                {
                    detectCollisions.enabled = true;
                }
                direction = Vector3.forward;
                followedAnimal = null;
            }

        }


    }
}

