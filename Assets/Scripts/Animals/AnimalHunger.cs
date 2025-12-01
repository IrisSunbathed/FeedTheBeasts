using System;
using NUnit.Framework;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace FeedTheBeasts.Scripts
{


    public class AnimalHunger : MonoBehaviour
    {

        [SerializeField] FoodTypes preferredFood;
        [SerializeField] float hungerTotal;
        float currentHunger;

        // [SerializeField] Transform hungerBar;
        [SerializeField] Image hungerBar;
        public int feedPoints;
        int points;
        int multiplyier;

        public event Action<int, Transform> OnPointsGainedEvent;
        void Awake()
        {
            #region ASSERTIONS
            Assert.IsTrue(feedPoints > 0, "ERROR: feed points not configured in one animal");
            Assert.IsNotNull(hungerBar, "ERROR, Hunger Bar is empty");
            #endregion
            #region VARIABLES

            multiplyier = 1;
            currentHunger = hungerTotal;
            #endregion

        }

        internal void FeedAnimal(string fedFood)
        {

            if (fedFood == preferredFood.ToString())
            {
                currentHunger -= 2f;
                multiplyier++;
            }
            else
            {
                currentHunger -= 0.5f;
            }
            float progress = Mathf.Clamp01(currentHunger / hungerTotal);
            hungerBar.fillAmount = 1 * progress;

            //x 100 = 1
            //hungerlvel / 100
            if (currentHunger <= 0)
            {
                points += feedPoints * multiplyier;
                OnPointsGainedEvent?.Invoke(points, transform);

                Destroy(gameObject);

            }
        }

        internal FoodTypes GetPreferredFood()
        {
            return preferredFood;
        }
    }

}