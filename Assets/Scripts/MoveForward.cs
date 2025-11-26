using System;
using UnityEngine;

namespace FeedTheBeasts.Scripts
{
    public class MoveForward : MonoBehaviour
    {

        public float speed = 10;

        float currentSpeed;


        void Awake()
        {
            SetSpeed();
        }
        internal void SetSpeed(float speedValue)
        {
            currentSpeed = speedValue;
        }

        internal void SetSpeed()
        {
            currentSpeed = speed;
        }


        // Update is called once per frame
        void Update()
        {
            transform.Translate(currentSpeed * Time.deltaTime * Vector3.forward);
        }

      
    }

}