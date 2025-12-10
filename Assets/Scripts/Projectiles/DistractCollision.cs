using System;
using System.Collections;
using UnityEngine;




namespace FeedTheBeasts.Scripts
{
    public class DistractCollision : MonoBehaviour
    {
        [SerializeField] int resistance;
        [SerializeField] float timeBetweenBites;

        public event Action<GameObject> OnWastedItem;

        void Awake()
        {
            resistance = 5;
        }
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.ANIMAL_TAG))
            {
                StartCoroutine(DistractionCoroutine());
            }
        }

        void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(Constants.ANIMAL_TAG))
            {
                StartCoroutine(DistractionCoroutine());
            }
        }

        IEnumerator DistractionCoroutine()
        {
            yield return new WaitForSeconds(timeBetweenBites);
            resistance--;
            if (resistance >= 0)
            {
                StartCoroutine(DistractionCoroutine());
            }
            else
            {
                OnWastedItem?.Invoke(gameObject);
            }
        }

        // internal void SetUp(int resistance, float timeBetweenBites)
        // {
        //     this.resistance = resistance;
        //     this.timeBetweenBites = timeBetweenBites;
        // }
    }



}