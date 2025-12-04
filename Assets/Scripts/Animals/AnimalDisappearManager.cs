using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;

namespace FeedTheBeasts.Scripts

{
    [RequireComponent(typeof(Collider),typeof(NavMeshAgent))]
    public class AnimalDisappearManager : MonoBehaviour
    {
        [SerializeField] GameObject canvas;
        [SerializeField] SkinnedMeshRenderer meshRendererAnimal;

        [SerializeField] float timeDisappear;
        NavMeshAgent navMeshAgent;
        Collider colAnimal;
        void Awake()
        {
            Assert.IsNotNull(canvas, "Error: canvas not added");
            Assert.IsNotNull(meshRendererAnimal, "Error: meshRendererAnimal not added");
            colAnimal = GetComponent<Collider>();
            navMeshAgent = GetComponent<NavMeshAgent>();
    
        }
        internal void Disappear()
        {
            canvas.SetActive(false);
            meshRendererAnimal.enabled = false;
            colAnimal.enabled = false;
            navMeshAgent.isStopped = true;
            gameObject.tag = Constants.UNTAGGED_TAG;
            Destroy(gameObject, timeDisappear);
        }
    }

}

