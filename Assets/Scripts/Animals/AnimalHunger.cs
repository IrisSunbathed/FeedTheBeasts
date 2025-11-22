using System;
using NUnit.Framework;
using Unity.Mathematics;
using UnityEngine;

namespace FeedTheBeasts.Scripts
{


    public class AnimalHunger : MonoBehaviour
    {

        [SerializeField] FoodTypes preferredFood;
        [SerializeField] int hungerLevel;

        [SerializeField] Transform hungerBar;

        public int feedPoints;
        int points;

        float BarLength;
        float percentagePerFeed;

        public event Action<int, Transform> OnPointsGainedEvent;
        void Awake()
        {
            #region ASSERTIONS
            Assert.IsTrue(feedPoints > 0, "ERROR: feed points not configured in one animal");
            Assert.IsNotNull(hungerBar, "ERROR, Hunger Bar is empty");
            #endregion
            #region VARIABLES
            BarLength = hungerBar.localScale.x;
            percentagePerFeed = BarLength / hungerLevel;
            #endregion

        }

        internal void FeedAnimal(string fedFood)
        {

            if (fedFood == preferredFood.ToString())
            {
                Debug.Log("Yummy!");
                hungerLevel -= 2;
                feedPoints *= 2;
            }
            else
            {
                hungerLevel--;
            }
            Vector2 scaleToReduceX = new Vector2(hungerBar.localScale.x - percentagePerFeed, hungerBar.localScale.y);
            hungerBar.localScale = scaleToReduceX;

            //x 100 = 1
            //hungerlvel / 100
            if (hungerLevel <= 0)
            {
                points += feedPoints;
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