using System;
using System.Collections;
using DG.Tweening;
using NUnit.Framework;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using RangeAttribute = UnityEngine.RangeAttribute;

namespace FeedTheBeasts.Scripts
{
    [RequireComponent(typeof(UIAnimalScoreController), typeof(Collider), typeof(AnimalDisappearManager))]
    public class AnimalHunger : MonoBehaviour
    {

        [Header("Food configuration")]

        [SerializeField] FoodTypes preferredFood;
        [SerializeField] Image hungerBar;
        internal float CurrentHunger { get; private set; }
        public float hungerTotal;
        public int feedPoints;
        int points;
        int multiplyier;
        internal bool IsPreferred { get; private set; }
        [Header("Feed effect configuration")]
        [SerializeField] float scaleEffectMax;
        [SerializeField, Range(0.01f, .5f)] float scaleTime;
        Vector3 defualtScale;
        [Header("Other")]
        [SerializeField] bool isBoss;
        AnimalDisappearManager animalDisapearManager;
        UIAnimalScoreController uIAnimalScoreController;

        public event Action<float> OnBossFeedEvent;

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
            defualtScale = transform.localScale;
        }

        internal void FeedAnimal(string fedFood)
        {
            IsPreferred = fedFood == preferredFood.ToString();

            
                if (IsPreferred)
                {
                    CurrentHunger -= 2f;
                    multiplyier++;
                    Vector3 addedScale = new Vector3(scaleEffectMax, scaleEffectMax, scaleEffectMax);
                    transform.DOScale(transform.localScale + addedScale, scaleTime).OnComplete(OnDoScaleComplete);
                }
                else
                {
                    CurrentHunger -= 0.25f;
                    if (CurrentHunger <= 0 & !isBoss)
                    {
                        animalDisapearManager.Disappear();
                        OnPointsGainedEvent?.Invoke(points, transform, true);

                    }
                }

            
            float progress = Mathf.Clamp01(CurrentHunger / hungerTotal);
            if (isBoss)
            {
                OnBossFeedEvent?.Invoke(progress);
            }
            else
            {
                hungerBar.fillAmount = 1 * progress;
            }
            points += feedPoints * multiplyier;
            OnPointsGainedEvent?.Invoke(points, transform, false);
            uIAnimalScoreController.AddMarker(points, IsPreferred);
        }

        private void OnDoScaleComplete()
        {
            bool isCompleted = false;
            if (CurrentHunger <= 0 & !isBoss & !isCompleted)
            {

                isCompleted = true;
                OnPointsGainedEvent?.Invoke(points, transform, true);
                animalDisapearManager.Disappear();
                Debug.Log("Disappear");

            }
            else
            {
                transform.DOScale(defualtScale, scaleTime);

            }
        }

        internal FoodTypes GetPreferredFood()
        {
            return preferredFood;
        }
    }

}