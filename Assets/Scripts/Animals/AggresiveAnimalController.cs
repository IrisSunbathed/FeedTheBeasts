using System;
using FeedTheBeasts.Scripts;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

namespace FeedTheBeasts.Scripts
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AggresiveAnimalController : Animal
    {

        Transform traPlayer;

        public event Action<bool> OnLoseLifeEvent;
        void Awake()
        {
            traPlayer = UnityEngine.GameObject.FindWithTag(Constants.PLAYER_TAG).GetComponent<Transform>();
            tag = Constants.ANIMAL_TAG;

        }

        // Update is called once per frame
        protected override void Update()
        {
            if (doesFetch)
            {
                TryFetch();
            }
            if (animalStatus == AnimalStatus.Running)
            {
                navMeshAgent.SetDestination(traPlayer.position);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.PLAYER_TAG))
            {
                OnLoseLifeEvent?.Invoke(false);
            }
        }
    }

}