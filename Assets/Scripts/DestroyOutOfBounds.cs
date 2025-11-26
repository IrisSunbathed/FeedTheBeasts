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
        bool flag;


        void Awake()
        {
            mainCam = Camera.main;
            upperLimitCamera = mainCam.orthographicSize;
            flag = false;

        }
        void Update()
        {
            if (transform.position.z < -upperLimitCamera & gameObject.CompareTag(Constants.ANIMAL_TAG) & !flag)
            {
                flag = true;
                OnLoseLifeEvent?.Invoke();
                Destroy(gameObject);
            }
        }
    }

}