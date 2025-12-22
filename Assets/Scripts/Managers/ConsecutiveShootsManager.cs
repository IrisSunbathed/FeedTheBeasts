using System;
using System.Collections;
using System.Threading;
using FeedTheBeasts.Scripts;
using NUnit.Framework;
using UnityEngine;
using RangeAttribute = UnityEngine.RangeAttribute;
namespace FeedTheBeasts
{

    public class ConsecutiveShootsManager : MonoBehaviour
    {

        int multiplayer;
        int maxRoundMultiplayer;

        [Header("Consecutive shoots configuration")]
        [SerializeField, Range(5, 15)] int pointsPerConsecutiveShoot;
        int consecutiveShootsCurrent;
        [SerializeField, Range(5, 15)] int consecutiveShootsBullets;
        [SerializeField, Range(5, 20)] int gainedBulletsCarrot;
        [SerializeField, Range(3, 10)] int gainedBulletsBeef;
        [SerializeField, Range(5, 10)] float maxIdleTime;

        [Header("References to provicers")]
        [SerializeField] CarrotProvider carrotProvider;
        [SerializeField] BeefProvider beefProvider;

        [Header("Other")]

        [SerializeField] ScoreUIManager livesAndPointsUIManager;

        Coroutine coroutine;

        void Awake()
        {
            Assert.IsNotNull(carrotProvider, "ERROR: carrotProvider not added");
            Assert.IsNotNull(beefProvider, "ERROR: beefProvider not added");
            Assert.IsNotNull(livesAndPointsUIManager, "ERROR: livesAndPointsUIManager not added");
            multiplayer = 1;
            maxRoundMultiplayer = multiplayer;
        }
        internal void SubscribeToEvents(DetectCollisions detectCollisions)
        {
            detectCollisions.OnHitAction += OnHitAction;
            detectCollisions.OnMissAction += OnMissCallBack;
            detectCollisions.OnFedAction += OnFedCallBack;
        }

        private void OnHitAction(DetectCollisions collisions, bool isLastHit)
        {
            if (isLastHit)
            {
                collisions.OnMissAction -= OnMissCallBack;
                collisions.OnHitAction -= OnHitAction;
            }
            else
            {
                UnsubscribeFromEvents(collisions);
            }
            // consecutiveShootsCurrent += consecutiveShootsAdd;
            consecutiveShootsCurrent++;

            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            //coroutine = StartCoroutine(TimeOutMult());

            if (consecutiveShootsCurrent % consecutiveShootsBullets == 0)
            {
                switch (collisions.gameObject.tag)
                {

                    case "Carrot":
                        carrotProvider.shootCount -= gainedBulletsCarrot;
                        //notify UI
                        // Debug.Log($"{carrotProvider.name} got added {gainedBulletsCarrot} bullets");
                        break;

                    case "Beef":
                        beefProvider.shootCount -= gainedBulletsBeef;
                        //notify UI
                        // Debug.Log($"{beefProvider.name} got added {gainedBulletsBeef} bullets");
                        break;
                }
            }
            if (consecutiveShootsCurrent >= 2)
            {
                livesAndPointsUIManager.AddSum(pointsPerConsecutiveShoot);
            }

        }

        private void OnMissCallBack(DetectCollisions collisions)
        {
            UnsubscribeFromEvents(collisions);
           // maxRoundMultiplayer = multiplayer;
            multiplayer = 0;
            //Reset();
        }

        internal void Reset()
        {

            livesAndPointsUIManager.CalculatePoints(multiplayer);
            multiplayer = 1;
            maxRoundMultiplayer = multiplayer;
            consecutiveShootsCurrent = 0;
        }

        private void OnFedCallBack(DetectCollisions collisions)
        {
            UnsubscribeFromEvents(collisions);
            multiplayer++;
            if (maxRoundMultiplayer < multiplayer)
            {
                maxRoundMultiplayer = multiplayer;
                livesAndPointsUIManager.AddMultiplayer(maxRoundMultiplayer);
            }



        }

        private void UnsubscribeFromEvents(DetectCollisions collisions)
        {
            collisions.OnMissAction -= OnMissCallBack;
            collisions.OnHitAction -= OnHitAction;
            collisions.OnFedAction -= OnFedCallBack;
        }

        internal void CalculatePoints()
        {
             livesAndPointsUIManager.CalculatePoints(maxRoundMultiplayer);
            multiplayer = 1;
            maxRoundMultiplayer = multiplayer;
            consecutiveShootsCurrent = 0;
        }

        // IEnumerator TimeOutMult()
        // {
        //     yield return new WaitForSeconds(maxIdleTime);
        //     Reset();
        // }
    }

}