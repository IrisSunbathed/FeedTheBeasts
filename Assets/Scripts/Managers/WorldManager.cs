using System;
using System.Diagnostics;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;

namespace FeedTheBeasts.Scripts
{
    public class WorldManager : MonoBehaviour
    {
        [Header("Player references")]
        [SerializeField] PlayerController playerController;
        [SerializeField] Player player;
        [Header("Other managers references")]
        [SerializeField] UIManager uIManager;
        [SerializeField] SpawnManager spawnManager;
        [SerializeField] ParticleSystemManager particleSystemManager;
        [SerializeField] FoodSelectorManager foodSelectorManager;
        [SerializeField] DifficultyManager difficultyManager;
        [SerializeField] CamerasManager camerasManager;
        [SerializeField] MusicManager musicManager;
        [SerializeField] AnimalsLeftUIManager animalsLeftUIManager;
        [SerializeField] PlayerUIManager playerUIManager;
        [Header("Others")]
        [SerializeField] UnityEngine.GameObject[] foodProviders;
        [SerializeField] CameraShaker cameraShaker;
        [SerializeField] Shooter shooter;
        [SerializeField] ProjectilePool projectilePool;
        [SerializeField] FruitProvider fruitProvider;
        [SerializeField] PlantingState plantingState;
        LevelManager levelManager;


        void Start()
        {
            levelManager = LevelManager.Instance;
            // musicManager.PlayMusic(MusicThemes.MainMenu);
        }
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
            Assert.IsNotNull(difficultyManager, "ERROR: difficultyManager is not added to FoodSelectorManager");
            Assert.IsNotNull(animalsLeftUIManager, "ERROR: animalsLeftUIManager is not added to FoodSelectorManager");
            Assert.IsNotNull(camerasManager, "ERROR: camerasManager is not added to FoodSelectorManager");
            Assert.IsNotNull(musicManager, "ERROR: musicManager is not added to FoodSelectorManager");
            Assert.IsNotNull(plantingState, "ERROR: plantingState is not added to FoodSelectorManager");
            Assert.IsNotNull(fruitProvider, "ERROR: fruitProvider is not added to FoodSelectorManager");
            Assert.IsNotNull(playerUIManager, "ERROR: playerUIManager is not added to FoodSelectorManager");
            Assert.IsTrue(foodProviders.Length > 0, "ERROR: providers is empty in FoodSelectorManager");
            #endregion
            spawnManager.OnAnimalSpawnEvent += OnAnimalSpawnCallBack;
            player.OnLoseLivePlayerAction += OnLoseLivePlayerActionCallback;
            player.OnGainedLivePlayerAction += OnGainedLiveCallBack;
            player.OnPointsAddedAction += OnPointsAddedCallBack;
            plantingState.OnCancelledAction += OnCacelledActionCallback;
            // player.OnMaxScoreReached += OnDifficultyAddCallback;
            uIManager.RestartGameEvent += RestartGameCallBack;
            foodSelectorManager.OnChangeEquippedItemEvent += OnChangeEquippedItemCallback;
            fruitProvider.OnPlantEvent += OnPlantCallBack;
            playerUIManager.OnCompletionEvent += OnCompletionEventCallback;
            foreach (var item in foodProviders)
            {
                if (item.TryGetComponent(out IRechargeable recharchable))
                {
                    recharchable = item.GetComponent<IRechargeable>();
                    recharchable.OnRechargeEvent += OnRechargeCallBack;

                }
            }
        }

        private void OnCompletionEventCallback()
        {
            Vector3 position = playerController.GetFontPosition();
            fruitProvider.Plant(position);

        }

        private void OnCacelledActionCallback()
        {
            playerUIManager.CancelFill();
        }

        private void OnPlantCallBack(float plantingTime)
        {
            playerUIManager.FillBar(plantingTime);
            playerController.SetPlantingState();

        }

        private void OnChangeEquippedItemCallback(int index, UnityEngine.GameObject projectile)
        {

            projectilePool.SetProjectile(projectile);
            uIManager.CurrentProjectile = index;
        }

        private void OnRechargeCallBack(float rechargeTime)
        {
            uIManager.RechargeBar(rechargeTime);
        }

        private void OnGainedLiveCallBack(int lives)
        {
            uIManager.ManageLives(lives);
        }

        private void RestartGameCallBack()
        {
            musicManager.PlayMusic(MusicThemes.InGame);
            playerController.Init();
            spawnManager.Init();
            foodSelectorManager.StartGame();
            player.Init();
            difficultyManager.Init();
            levelManager.Init();
            spawnManager.Init();
        }

        private void OnPointsAddedCallBack(int points)
        {
            uIManager.ManagePoints(points);
        }

        private void OnLoseLivePlayerActionCallback(int lives)
        {
            uIManager.ManageLives(lives);
            if (lives <= 0)
            {
                uIManager.GameOver();
                playerController.SetDeathState();
                spawnManager.StopSpawning();
                foodSelectorManager.Init();
                DestroyAnimals();
                musicManager.PlayMusic(MusicThemes.Lose);
                animalsLeftUIManager.Init();
                playerUIManager.CancelFill();
            }
            else
            {
                camerasManager.ShakeCurrentCamera(lives);
            }
        }

        private static void DestroyAnimals()
        {
            UnityEngine.GameObject[] animals = UnityEngine.GameObject.FindGameObjectsWithTag(Constants.ANIMAL_TAG);
            foreach (var animal in animals)
            {
                animal.SetActive(false);
            }
        }

        private void OnAnimalSpawnCallBack(UnityEngine.GameObject goAnimal)
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

        private void OnPointsGainedCallBack(int points, Transform transform, bool isFed)
        {
            player.Score += points;
            if (isFed)
            {
                levelManager.AnimalFed();
                particleSystemManager.SpawnParticles(transform);
            }


        }

        internal void Win()
        {
            //playerController.WinState();
            //spawnManager.StopSpawning();
            uIManager.Win();
            animalsLeftUIManager.Init();
            foodSelectorManager.Init();
            //DestroyAnimals();
            musicManager.FadeCurrentMusic(0, 0.5f);;
            playerUIManager.CancelFill();
        }

        private void OnLoseLifeCallBack(bool hasScaped = false)
        {
            if (hasScaped)
            {
                levelManager.AnimalFed();
                levelManager.EscapedAnimals++;
            }
            player.Lives--;
        }
    }

}