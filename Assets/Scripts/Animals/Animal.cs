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
        [SerializeField] internal bool doesFetch;
        [SerializeField] internal bool doesEatBasket;
        [SerializeField] internal bool doesTurn;
        [SerializeField, Range(1f, 5f)] float timeTransitionMovement;

        AudioSource audioSource;

        bool hasAudioPlayed;

        GameCatalog gameCatalog;

        internal AnimalStatus animalStatus;

        Animator animator;

        [SerializeField,Range(5f, 10f)] float destinationOffset = 4f;
        internal NavMeshAgent navMeshAgent;

        Vector3 destination;

        float destinationZ;
        Coroutine coroutine;


        [SerializeField, Range(5f, 60f)] float timeWaitNewLocation;


        CamerasManager camerasManager;

        Vector3 defaultDestination;

        Vector3 currentDestinion;

        float time;

        void Start()
        {
            camerasManager = CamerasManager.Instance;
            gameCatalog = GameCatalog.Instance;

            destinationZ = -camerasManager.OrthographicSize - destinationOffset;
            defaultDestination = new Vector3(transform.position.x, transform.position.y, destinationZ);
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

        public virtual void Update()
        {

            if (animalStatus != AnimalStatus.Returning)
            {

                time += Time.deltaTime;
                if (doesFetch && !TryFetch() & coroutine == null)
                {
                    coroutine = StartCoroutine(SetDestinationCoroutine(currentDestinion));
                }

                if (doesEatBasket && !TryEatBasket() & coroutine == null)
                {
//                    Debug.Log($"doesEatBasket: {doesEatBasket} !TryEatBasket() {!TryEatBasket()} coroutine == null {coroutine == null}");
                    coroutine = StartCoroutine(SetDestinationCoroutine(currentDestinion));
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
        }
        IEnumerator SetDestinationCoroutine(Vector3 newDestination)
        {
            SetDestination(newDestination.x, newDestination.y, newDestination.z);
            yield return new WaitForSeconds(timeWaitNewLocation);
            coroutine = null;
        }

        private bool TryEatBasket()
        {
            UnityEngine.GameObject platable = UnityEngine.GameObject.FindGameObjectWithTag(Constants.PLANTABLE_TAG);
            if (platable != null)
            {

                SetMovingAnimation();
                animalStatus = AnimalStatus.Fetching;
                if (coroutine == null)
                {
                    Vector3 newDestination = new Vector3(platable.transform.position.x, transform.position.y, platable.transform.position.z);
                    coroutine = StartCoroutine(SetDestinationCoroutine(newDestination));
                }


                TryEat();
            }
            else
            {
                if (animalStatus != AnimalStatus.Returning)
                {
                    currentDestinion = defaultDestination;
                }
                SetMovingAnimation();
            }
            return platable != null;
        }

        private void TryEat()
        {
            if (!navMeshAgent.pathPending)
            {
                if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
                {
                    if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                    {
                        SetEatingAnimation();
                    }
                }
            }
        }

        protected bool TryFetch()
        {
            UnityEngine.GameObject throwable = UnityEngine.GameObject.FindGameObjectWithTag(Constants.THROWABLE_TAG);
            if (throwable != null)
            {

                if (animalType == AnimalType.Dog & !hasAudioPlayed)
                {

                    StartCoroutine(BarkCoroutine());
                    hasAudioPlayed = true;

                }
                SetMovingAnimation();
                animalStatus = AnimalStatus.Fetching;
                if (coroutine == null)
                {
                    Vector3 newDestination = new Vector3(throwable.transform.position.x, transform.position.y, throwable.transform.position.z);
                    coroutine = StartCoroutine(SetDestinationCoroutine(newDestination));
                }
                TryEat();
            }
            else
            {
                if (animalStatus != AnimalStatus.Returning)
                {
                    currentDestinion = defaultDestination;
                }
                SetMovingAnimation();

            }
            return throwable != null;
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

        internal void SetEatingAnimation()
        {
            animalStatus = AnimalStatus.Stopped;
            animator.SetBool(Constants.ANIM_BOOL_EAT, true);
            animator.SetFloat(Constants.ANIM_FLOAT_SPEED, 0);
            destinationZ = transform.position.z;
        }

        private void SetMovingAnimation()
        {
            animator.SetBool(Constants.ANIM_BOOL_EAT, false);
            animator.SetFloat(Constants.ANIM_FLOAT_SPEED, 1);

            animalStatus = animalStatus != AnimalStatus.Fetching ? AnimalStatus.Running : AnimalStatus.Fetching;
        }

        private void RandomizeMovement()
        {
            float length = camerasManager.GetCameraLength();
            float newXdestination = Random.Range(length, -length);
            SetDestination(newXdestination, transform.position.y, destinationZ);

        }

        internal void SetDestination(float destinationX, float destinationY, float destinationZ)
        {
            destination = new Vector3(destinationX, destinationY, destinationZ);
            currentDestinion = destination;
            navMeshAgent.SetDestination(currentDestinion);
        }

    }

}