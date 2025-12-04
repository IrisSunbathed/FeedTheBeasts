using System;
using System.Collections;
using System.Numerics;
using System.Security.Cryptography;
using NUnit.Framework;
using Unity.Burst.CompilerServices;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;
using RangeAttribute = UnityEngine.RangeAttribute;
using Vector3 = UnityEngine.Vector3;

namespace FeedTheBeasts.Scripts
{
    [RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
    [RequireComponent(typeof(AudioSource))]
    public class Animal : MonoBehaviour
    {

        [SerializeField] AnimalType animalType;
        [Header("Animal behaviour")]
        [SerializeField] bool doesAnimalStop;
        [SerializeField] protected bool doesFetch;
        [SerializeField] protected bool doesEatBasket;
        [SerializeField] protected bool doesTurn;
        [SerializeField, Range(1f, 5f)] float timeTransitionMovement;

        AudioSource audioSource;

        bool hasAudioPlayed;

        GameCatalog gameCatalog;

        protected AnimalStatus animalStatus;

        Animator animator;

        readonly float destinationOffset = 4f;
        internal NavMeshAgent navMeshAgent;

        Vector3 destination;

        float destinationZ;


        CamerasManager camerasManager;

        float time;



        void Start()
        {
            camerasManager = CamerasManager.Instance;
            gameCatalog = GameCatalog.Instance;

            destinationZ = -camerasManager.OrthographicSize - destinationOffset;

            Awake();
        }

        void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();

            SetDestination(transform.position.x, transform.position.y, destinationZ);
            SetMovingAnimation();


        }

        protected virtual void Update()
        {

            time += Time.deltaTime;
            if (doesFetch)
            {
                TryFetch();
            }

            if (doesEatBasket)
            {
                TryEatBasket();
            }

            if (doesTurn & animalStatus == AnimalStatus.Running)
            {
                if (time >= timeTransitionMovement)
                {
                    RandomizeMovement();
                    time = 0;
                    float randomNumber = Random.Range(1, 4);

                    if (randomNumber == 3 & doesAnimalStop)
                    {
                        StartCoroutine(StopWalkingCoroutine());

                    }
                }
            }
        }

        private void TryEatBasket()
        {
            GameObject platable = GameObject.FindGameObjectWithTag(Constants.PLANTABLE_TAG);
            if (platable != null)
            {

                SetMovingAnimation();
                animalStatus = AnimalStatus.Fetching;
                Vector3 newDestination = platable.transform.position;
                SetDestination(newDestination.x, transform.position.y, newDestination.z);
                if (navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete)
                {
                    SetEatingAnimation();
                }
            }
            else
            {
                SetMovingAnimation();
            }
        }

        protected void TryFetch()
        {
            GameObject throwable = GameObject.FindGameObjectWithTag(Constants.THROWABLE_TAG);
            if (throwable != null)
            {

                if (animalType == AnimalType.Dog & !hasAudioPlayed)
                {

                    StartCoroutine(BarkCoroutine());
                    hasAudioPlayed = true;

                }
                SetMovingAnimation();
                animalStatus = AnimalStatus.Fetching;
                Vector3 newDestination = throwable.transform.position;
                SetDestination(newDestination.x, transform.position.y, newDestination.z);
                if (navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete)
                {
                    SetEatingAnimation();
                }
            }
            else
            {
                SetMovingAnimation();
            }
        }

        IEnumerator BarkCoroutine()
        {
            AudioClip clip = gameCatalog.GetFXClip(FXTypes.DogBone);
            int random = Random.Range(1, 4);

            for (int i = 0; i <= random; i++)
            {
                audioSource.resource = clip;
                audioSource.Play();
                float randomTime = Random.Range(audioSource.clip.length, audioSource.clip.length * 2);
                yield return new WaitForSeconds(randomTime);
            }
        }

        IEnumerator StopWalkingCoroutine()
        {
            SetEatingAnimation();
            float stoppedTimeMin = animator.GetCurrentAnimatorClipInfo(0).Length;
            yield return new WaitForSeconds(stoppedTimeMin);
            SetMovingAnimation();
        }

        private void SetEatingAnimation()
        {
            animalStatus = AnimalStatus.Stopped;
            animator.SetBool(Constants.ANIM_BOOL_EAT, true);
            animator.SetFloat(Constants.ANIM_FLOAT_SPEED, 0);
        }

        private void SetMovingAnimation()
        {
            animator.SetBool(Constants.ANIM_BOOL_EAT, false);
            animator.SetFloat(Constants.ANIM_FLOAT_SPEED, 1);
            animalStatus = AnimalStatus.Running;
        }

        private void RandomizeMovement()
        {
            float length = camerasManager.GetCameraLength();
            float newXdestination = Random.Range(length, -length);
            SetDestination(newXdestination, transform.position.y, destinationZ);

        }

        private void SetDestination(float destinationX, float destinationY, float destinationZ)
        {
            destination = new Vector3(destinationX, destinationY, destinationZ);
            navMeshAgent.SetDestination(destination);

        }

    }

    public class Moose : Animal
    {
        static bool IsDefeated { get; set; }

        
    }

}