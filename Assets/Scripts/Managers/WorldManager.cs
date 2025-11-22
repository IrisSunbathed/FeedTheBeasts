using System;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

namespace FeedTheBeasts.Scripts
{
    public class WorldManager : MonoBehaviour
    {
        [SerializeField] PlayerController playerController;
        [SerializeField] UIManager uIManager;

        [SerializeField] SpawnManager spawnManager;
        [SerializeField] ParticleSystemManager particleSystemManager;
        [SerializeField] FoodSelectorManager foodSelectorManager;
        [SerializeField] CameraShaker cameraShaker;
        [SerializeField] Player player;
        [SerializeField] Shooter shooter;
        [SerializeField] ProjectilePool projectilePool;
        [SerializeField] GameObject[] foodProviders;



        void Awake()
        {
            #region ASSERTIONS
            Assert.IsNotNull(playerController, "ERROR: player controller not added not WorldManager");
            Assert.IsNotNull(player, "ERROR: player not added not WorldManager");
            Assert.IsNotNull(uIManager, "ERROR: UI Manager not added to WorldManager");
            Assert.IsNotNull(spawnManager, "ERROR: Spawn Manager not added to WorldManager");
            Assert.IsNotNull(particleSystemManager, "ERROR: ParticleSystem not added to WorldManager");
            Assert.IsNotNull(foodSelectorManager, "ERROR: FoodSelectorManager not added to WorldManager");
            Assert.IsNotNull(cameraShaker, "ERROR: CameraShaker not added to WorldManager");
            Assert.IsNotNull(shooter, "ERROR: Shooter not added to WorldManager");
            Assert.IsNotNull(projectilePool, "ERROR: ProjectilePool is not added to FoodSelectorManager");
            Assert.IsTrue(foodProviders.Length > 0, "ERROR: providers is empty in FoodSelectorManager");

            #endregion
            spawnManager.OnAnimalSpawnEvent += OnAnimalSpawnCallBack;
            player.OnLoseLivePlayerAction += OnLoseLivePlayerActionCallback;
            player.OnGainedLivePlayerAction += OnGainedLiveCallBack;
            player.OnPointsAddedAction += OnPointsAddedCallBack;
            uIManager.RestartGameEvent += RestartGameCallBack;
            foodSelectorManager.OnChangeEquippedItemEvent += OnChangeEquippedItemCallback;

            foreach (var item in foodProviders)
            {
                IRechargeable recharchable = item.GetComponent<IRechargeable>();
                recharchable.OnRechargeEvent += OnRechargeCallBack;
            }

        }

        private void OnChangeEquippedItemCallback(int index, GameObject projectile)
        {
            projectilePool.SetProjectile(projectile);
            uIManager.CurrentProjectile = index;
        }

        private void OnRechargeCallBack(float rechargeTime)
        {
            uIManager.RechargeBar(rechargeTime);
        }

        private void OnGainedLiveCallBack(int points)
        {
            uIManager.ManageLives(points);
        }

        private void RestartGameCallBack()
        {
            playerController.Init();
            spawnManager.Init();
            foodSelectorManager.StartGame();
            player.Init();
        }

        private void OnPointsAddedCallBack(int points)
        {
            uIManager.ManagePoints(points);
        }

        private void OnLoseLivePlayerActionCallback(int lives)
        {
            cameraShaker.ShakeCamera(lives / 0.5f);
            uIManager.ManageLives(lives);
            if (lives == 0)
            {
                uIManager.GameOver();
                playerController.SetDeathState();
                spawnManager.StopSpawning();
                foodSelectorManager.Init();

            }
        }

        private void OnAnimalSpawnCallBack(GameObject goAnimal)
        {
            if (goAnimal.TryGetComponent(out DestroyOutOfBounds destroyOutOfBounds))
            {
                destroyOutOfBounds.OnLoseLifeEvent += OnLoseLifeCallBack;
            }
            if (goAnimal.TryGetComponent(out AggresiveAnimalController aggresiveAnimalController))
            {
                aggresiveAnimalController.OnLoseLifeEvent += OnLoseLifeCallBack;
            }


            AnimalHunger feedPoints = goAnimal.GetComponent<AnimalHunger>();
            feedPoints.OnPointsGainedEvent += OnPointsGainedCallBack;

        }

        private void OnPointsGainedCallBack(int points, Transform transform)
        {
            player.Score += points;
            particleSystemManager.SpawnParticles(transform);

        }

        private void OnLoseLifeCallBack()
        {
            player.Lives--;
        }
    }

}