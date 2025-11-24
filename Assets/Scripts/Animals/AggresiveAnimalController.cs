using System;
using FeedTheBeasts.Scripts;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

namespace FeedTheBeasts.Scripts
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AggresiveAnimalController : MonoBehaviour
    {
        NavMeshAgent navMeshAgent;

        Transform traPlayer;

        public event Action OnLoseLifeEvent;
        void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            traPlayer = GameObject.FindWithTag(Constants.PLAYER_TAG).GetComponent<Transform>();

        }

        // Update is called once per frame
        void Update()
        {
            navMeshAgent.SetDestination(traPlayer.position);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.PLAYER_TAG))
            {
                OnLoseLifeEvent?.Invoke();
            }
        }
    }

}