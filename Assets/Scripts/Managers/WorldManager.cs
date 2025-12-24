using System;
using System.Collections;
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
        [SerializeField] BossManager bossManager;
        [SerializeField] ScoreManager scoreManager;
        [SerializeField] ConsecutiveShootsManager consecutiveShootsManager;
        [SerializeField] OutroController outroController;
        [Header("Others")]
        [SerializeField] GameObject[] foodProviders;
        [SerializeField] CameraShaker cameraShaker;
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
            Assert.IsNotNull(bossManager, "ERROR: bossManager not added to WorldManager");
            Assert.IsNotNull(difficultyManager, "ERROR: difficultyManager is not added to FoodSelectorManager");
            Assert.IsNotNull(animalsLeftUIManager, "ERROR: animalsLeftUIManager is not added to FoodSelectorManager");
            Assert.IsNotNull(camerasManager, "ERROR: camerasManager is not added to FoodSelectorManager");
            Assert.IsNotNull(musicManager, "ERROR: musicManager is not added to FoodSelectorManager");
            Assert.IsNotNull(scoreManager, "ERROR: scoreManager is not added to FoodSelectorManager");
            Assert.IsNotNull(plantingState, "ERROR: plantingState is not added to FoodSelectorManager");
            Assert.IsNotNull(fruitProvider, "ERROR: fruitProvider is not added to FoodSelectorManager");
            Assert.IsNotNull(playerUIManager, "ERROR: playerUIManager is not added to FoodSelectorManager");
            Assert.IsNotNull(outroController, "ERROR: outroController is not added to FoodSelectorManager");
            Assert.IsNotNull(consecutiveShootsManager, "ERROR: consecutiveShootsManager is not added to FoodSelectorManager");
            Assert.IsTrue(foodProviders.Length > 0, "ERROR: providers is empty in FoodSelectorManager");
            #endregion
            spawnManager.OnAnimalSpawnEvent += OnAnimalSpawnCallBack;
            player.OnLoseLivePlayerAction += OnLoseLivePlayerActionCallback;
            player.OnGainedLivePlayerAction += OnGainedLiveCallBack;
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
            scoreManager.Init();
            difficultyManager.Init();
            levelManager.Init();
        }

        private void OnLoseLivePlayerActionCallback(int lives)
        {
            uIManager.ManageLives(lives);
            //consecutiveShootsManager.Reset();
            if (lives <= 0)
            {
                uIManager.GameOver();
                playerController.SetDeathState();
                spawnManager.StopSpawning();
                foodSelectorManager.EndGame();
                DestroyAnimals();
                bossManager.GameOver();
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
                //Add if animal is outside of bounds
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

        private void OnPointsGainedCallBack(int points, AnimalHunger animalHunger, bool isFed)
        {
            scoreManager.Score += points;
            if (isFed)
            {
                levelManager.CurrentFedAnimals++;
                levelManager.LevelAnimalCheck();
                particleSystemManager.SpawnParticles(animalHunger.transform);
                animalHunger.OnPointsGainedEvent -= OnPointsGainedCallBack;
            }


        }

        internal void NextRound()

        {
            uIManager.InGameWarning(4f, "Round Completed!");
            consecutiveShootsManager.CalculatePoints();
            if (uIManager.CheckPointsCalc())
            {
                //UI Round completed
                levelManager.NextRound();
                StartCoroutine(spawnManager.StartCouroutines());
            }
            else
            {
                StartCoroutine(WaitForPointsCalcCoroutine());
            }
        }

        internal void Win()
        {

            if (uIManager.CheckPointsCalc())
            {
                outroController.OutroStart();
                foodSelectorManager.EndGame();
                playerController.CanMove = false;
                uIManager.Win();
                animalsLeftUIManager.Init();
                musicManager.FadeCurrentMusic(0, 0.5f); ;
                playerUIManager.CancelFill();
            }
            else
            {
                StartCoroutine(WaitForPointsCalcCoroutine());
            }
        }

        IEnumerator WaitForPointsCalcCoroutine()
        {

            if (uIManager.CheckPointsCalc())
            {
                yield return new WaitForSeconds(2f);
                levelManager.NextRound();
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
                StartCoroutine(WaitForPointsCalcCoroutine());
            }
        }

        private void OnLoseLifeCallBack(bool hasScaped = false)
        {
            if (hasScaped)
            {
                levelManager.EscapedAnimals++;
                levelManager.LevelAnimalCheck();
            }
            player.Lives--;
        }
    }

}