using System;
using FeedTheBeasts.Scripts;
using NUnit.Framework;
using UnityEngine;

namespace FeedTheBeasts.Scripts
{
    [RequireComponent(typeof(MeshRenderer))]
    public class DestroyOutOfBounds : MonoBehaviour
    {
        
        Camera mainCam;

        float upperLimitCamera;

        public event Action OnLoseLifeEvent;


        void Awake()
        {
            mainCam = Camera.main;
            upperLimitCamera = mainCam.orthographicSize;

        }
        void Update()
        {
            if (transform.position.z < -upperLimitCamera & gameObject.CompareTag(Constants.ANIMAL_TAG))
            {
                Destroy(gameObject);
                OnLoseLifeEvent?.Invoke();
            }
        }
    }

}