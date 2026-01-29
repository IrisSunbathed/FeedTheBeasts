using System;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;

namespace FeedTheBeasts.Scripts
{
    public class PowerUpsManager : MonoBehaviour
    {
        List<PowerUps> activatedPowerUp;
        [Header("References", order = 1)]
        [Header("Managers", order = 2)]

        [SerializeField] InventoryUIManager inventoryUIManager;
        [SerializeField] ScoreManager scoreManager;
        GameCatalog gameCatalog;
        [Header("State", order = 2)]
        [SerializeField] RunState runState;
        [Header("Food throw", order = 2)]
        [Header("Object pools", order = 3)]
        [SerializeField] CarrotObjectPool carrotObjectPool;
        [SerializeField] BeefObjectPool beefObjectPool;
        [Header("Providers", order = 3)]
        [SerializeField] BeefProvider beefProvider;
        [SerializeField] CarrotProvider carrotProvider;
        [SerializeField] BoneProvider boneProvider;
        [SerializeField] FruitProvider fruitProvider;
        [SerializeField] FoodSelectorManager foodSelectorManager;

        void Start()
        {
            gameCatalog = GameCatalog.Instance;
        }

        void Awake()
        {
            Assert.IsNotNull(inventoryUIManager, "inventoryUIManager is not added");
            Assert.IsNotNull(scoreManager, "scoreManager is not added");
            Assert.IsNotNull(runState, "runState is not added");
            Assert.IsNotNull(carrotObjectPool, "carrotObjectPool is not added");
            Assert.IsNotNull(beefObjectPool, "beefObjectPool is not added");
            Assert.IsNotNull(beefProvider, "beefProvider is not added");
            Assert.IsNotNull(carrotProvider, "carrotProvider is not added");
            Assert.IsNotNull(boneProvider, "boneProvider is not added");
            Assert.IsNotNull(fruitProvider, "fruitProvider is not added");
            Assert.IsNotNull(foodSelectorManager, "foodSelectorManager is not added");

            activatedPowerUp = new List<PowerUps>();
        }

        internal void OnPowerUpClick(PowerUps powerUp)
        {
            if (!activatedPowerUp.Contains(powerUp))
            {
                activatedPowerUp.Add(powerUp);
            }
            switch (powerUp)
            {
                case PowerUps.GetBone:
                    inventoryUIManager.UnlockBone();
                    break;
                case PowerUps.GetFruitBasket:
                    inventoryUIManager.UnlockBasket();
                    break;
                case PowerUps.MoreLives:
                    scoreManager.ScorePowerUp();
                    break;
                case PowerUps.SpeedUp:
                    runState.IncreaseMovementSpeed();
                    break;
                case PowerUps.FoodSpeedUpgrade:
                    carrotObjectPool.SetSpeedPowerUp();
                    beefObjectPool.SetSpeedPowerUp();
                    beefProvider.DecreaseCooldownPowerUp();
                    carrotProvider.DecreaseCooldownPowerUp();
                    boneProvider.DecreaseCooldownPowerUp();
                    fruitProvider.DecreaseCooldownPowerUp();
                    break;

                case PowerUps.AddBullets:
                    beefProvider.AddBulletsPowerUp();
                    Debug.Log($"beefProvider.shootCount: {beefProvider.shootCount}");
                    carrotProvider.AddBulletsPowerUp();
                    foodSelectorManager.RefreshBullets();

                    break;
                case PowerUps.TrackAnimals:
                    carrotProvider.doesTrackAnimal = true;
                    beefProvider.doesTrackAnimal = true;
                    break;
            }
        }

        internal bool CanPowerUpBeStacked(PowerUps powerUps)
        {
            // Debug.Log($"activatedPowerUp.Contains(powerUps) {activatedPowerUp.Contains(powerUps)}");
            // Debug.Log($"!gameCatalog.CanPowerUpBeStacked(powerUps): {!gameCatalog.CanPowerUpBeStacked(powerUps)} ");
            // Debug.Log($"Final result: {activatedPowerUp.Contains(powerUps) && !gameCatalog.CanPowerUpBeStacked(powerUps)}");
            return activatedPowerUp.Contains(powerUps) && !gameCatalog.CanPowerUpBeStacked(powerUps);
        }

        
    }

}