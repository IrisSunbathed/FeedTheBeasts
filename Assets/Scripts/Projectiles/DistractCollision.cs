using System;
using System.Collections;
using UnityEngine;




namespace FeedTheBeasts.Scripts
{
    public class DistractCollision : MonoBehaviour
    {
        [SerializeField, Range(5, 15)] int resistance;
        [SerializeField] float timeBetweenBites;
        float time = 0;

        public event Action<GameObject> OnWastedItem;

        void OnTriggerStay(Collider other)
        {
            
            if (other.CompareTag(Constants.ANIMAL_TAG) && other.GetComponent<Animal>().doesFetch & time == 0)
            {
                StartCoroutine(DistractionCoroutine());
            }
        }

        IEnumerator DistractionCoroutine()
        {
            time += Time.deltaTime;

            yield return new WaitForSeconds(timeBetweenBites);
            Debug.Log("Bone bitten");
            resistance--;
            if (resistance == 0)
            {
                OnWastedItem?.Invoke(gameObject);
            }

            time = 0;
        }

        // internal void SetUp(int resistance, float timeBetweenBites)
        // {
        //     this.resistance = resistance;
        //     this.timeBetweenBites = timeBetweenBites;
        // }
    }



}