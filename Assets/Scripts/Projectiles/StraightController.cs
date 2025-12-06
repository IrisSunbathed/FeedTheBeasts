using System;
using Unity.VisualScripting;
using UnityEngine;

namespace FeedTheBeasts.Scripts
{
    public class StraightController : MonoBehaviour
    {

        public float speed = 10;

        float currentSpeed;

        LineRenderer lineRenderer;




        void Awake()
        {
            SetUpController();
            
        }

        internal void SetUpController()
        {
            SetSpeed();
            enabled = true;
            if (!TryGetComponent(out DetectCollisions _))
            {
                gameObject.AddComponent<DetectCollisions>();
            }
            if (TryGetComponent(out ThrowableController throwableController))
            {
                throwableController.Deactivate();
            }
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

        internal void Deactivate()
        {
            if (TryGetComponent(out DetectCollisions detectCollisions))
            {
                detectCollisions.enabled = false;
                Destroy(this);
            }
        }
    }

}