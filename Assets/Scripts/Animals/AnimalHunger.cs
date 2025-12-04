using System;
using System.Collections;
using NUnit.Framework;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace FeedTheBeasts.Scripts
{
    [RequireComponent(typeof(UIAnimalScoreController), typeof(Collider), typeof(AnimalDisappearManager))]
    public class AnimalHunger : MonoBehaviour
    {

        [SerializeField] FoodTypes preferredFood;
        [SerializeField] float hungerTotal;
        AnimalDisappearManager animalDisapearManager;
        UIAnimalScoreController uIAnimalScoreController;

        internal bool IsPreferred { get; private set; }
        internal float CurrentHunger { get; private set; }

        // [SerializeField] Transform hungerBar;
        [SerializeField] Image hungerBar;
        public int feedPoints;
        int points;
        int multiplyier;

        public event Action<int, Transform, bool> OnPointsGainedEvent;
        void Awake()
        {
            #region ASSERTIONS
            Assert.IsTrue(feedPoints > 0, "ERROR: feed points not configured in one animal");
            Assert.IsNotNull(hungerBar, "ERROR, Hunger Bar is empty");
            #endregion
            #region VARIABLES
            uIAnimalScoreController = GetComponent<UIAnimalScoreController>();
            animalDisapearManager = GetComponent<AnimalDisappearManager>();

            multiplyier = 1;
            CurrentHunger = hungerTotal;
            #endregion

        }

        internal void FeedAnimal(string fedFood)
        {
            IsPreferred = fedFood == preferredFood.ToString();

            if (IsPreferred)
            {
                CurrentHunger -= 2f;
                multiplyier++;
            }
            else
            {
                CurrentHunger -= 0.5f;
            }
            float progress = Mathf.Clamp01(CurrentHunger / hungerTotal);
            hungerBar.fillAmount = 1 * progress;
            points += feedPoints * multiplyier;
            OnPointsGainedEvent?.Invoke(points, transform, false);
            uIAnimalScoreController.AddMarker(points,IsPreferred);
            if (CurrentHunger <= 0)
            {
                OnPointsGainedEvent?.Invoke(points, transform, true);
                animalDisapearManager.Disappear();

            }
        }



        internal FoodTypes GetPreferredFood()
        {
            return preferredFood;
        }
    }

}