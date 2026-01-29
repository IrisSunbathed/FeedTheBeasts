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

        [SerializeField] internal FoodTypes preferredFood;
        [SerializeField] Image hungerBar;
        [SerializeField] DestroyOutOfBounds destroyOutOfBounds;
        internal float CurrentHunger { get; private set; }
        public float hungerTotal;
        public int feedPoints;
        int points;
        internal bool IsPreferred { get; private set; }
        [Header("Feed effect configuration")]
        [SerializeField] float scaleEffectMax;
        [SerializeField, Range(0.01f, .5f)] float scaleTime;
        Vector3 defualtScale;
        [Header("Other")]
        [SerializeField] bool isBoss;
        AnimalDisappearManager animalDisapearManager;
        UIAnimalScoreController uIAnimalScoreController;
        Collider colAnimal;

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
            colAnimal = GetComponent<Collider>();

            CurrentHunger = hungerTotal;
            #endregion
            defualtScale = transform.localScale;
        }

        internal void FeedAnimal(string fedFood)
        {
            IsPreferred = fedFood == preferredFood.ToString();

            if (CurrentHunger > 0f)
            {

                if (IsPreferred)
                {
                    CurrentHunger -= 2f;
                    if (CurrentHunger <= 0 & !isBoss)
                    {
                        // Animal animal = GetComponent<Animal>();
                        // animal.navMeshAgent.isStopped = true;
                        colAnimal.enabled = false;
                        if (destroyOutOfBounds != null)
                        {
                            destroyOutOfBounds.enabled = false;
                        }
                        Vector3 addedScale = new Vector3(scaleEffectMax, scaleEffectMax, scaleEffectMax);
                        transform.DOScale(transform.localScale + addedScale, scaleTime).OnComplete(OnDoScaleCompleteFed);
                    }
                    if (CurrentHunger > 0)
                    {
                        Vector3 addedScale = new Vector3(scaleEffectMax, scaleEffectMax, scaleEffectMax);
                        transform.DOScale(transform.localScale + addedScale, scaleTime).OnComplete(OnDoScaleCompleteHit);
                    }

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
                HungerBarManagement();
                points += feedPoints;
                uIAnimalScoreController.AddMarker(points, IsPreferred);
            }
        }

        private void HungerBarManagement()
        {
            float progress = Mathf.Clamp01(CurrentHunger / hungerTotal);
            if (isBoss)
            {
                OnBossFeedEvent?.Invoke(progress);
            }
            else
            {
                hungerBar.fillAmount = 1 * progress;
            }
        }

        private void OnDoScaleCompleteHit()
        {
            transform.DOScale(defualtScale, scaleTime);
            OnPointsGainedEvent?.Invoke(points, transform, false);
        }
        private void OnDoScaleCompleteFed()
        {
            animalDisapearManager.Disappear();
            OnPointsGainedEvent?.Invoke(points, transform, true);

        }

        internal FoodTypes GetPreferredFood()
        {
            return preferredFood;
        }
    }

}