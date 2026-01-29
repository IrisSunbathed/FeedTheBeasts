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
        [SerializeField] InventoryUIManager inventoryUIManager;
        [SerializeField] UpdateMenuUIManager updateMenuUIManager;
        [SerializeField] PowerUpsManager powerUpsManager;
        [Header("Others")]
        [SerializeField] GameObject[] foodProviders;
        [SerializeField] CameraShaker cameraShaker;
        [SerializeField] FruitProvider fruitProvider;
        [SerializeField] PlantingState plantingState;
        LevelManager levelManager;


        void Start()
        {
            levelManager = LevelManager.Instance;
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
            Assert.IsNotNull(inventoryUIManager, "ERROR: inventoryUIManager is not added to FoodSelectorManager");
            Assert.IsNotNull(consecutiveShootsManager, "ERROR: consecutiveShootsManager is not added to FoodSelectorManager");
            Assert.IsNotNull(powerUpsManager, "ERROR: powerUpsManager is not added to FoodSelectorManager");
            Assert.IsTrue(foodProviders.Length > 0, "ERROR: providers is empty in FoodSelectorManager");
            #endregion
            spawnManager.OnAnimalSpawnEvent += OnAnimalSpawnCallBack;
            player.OnLoseLivePlayerAction += OnLoseLivePlayerActionCallback;
            player.OnGainedLivePlayerAction += OnGainedLiveCallBack;
            plantingState.OnCancelledAction += OnCacelledActionCallback;
            uIManager.RestartGameEvent += RestartGameCallBack;
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
            DestroyObjectsInScene();
        }

        private void OnCompletionEventCallback()
        {
            Vector3 position = playerController.GetFrontPosition();

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

        private static void DestroyObjectsInScene()
        {
            foreach (var item in UnityEngine.GameObject.FindGameObjectsWithTag(Constants.THROWABLE_TAG))
            {
                Destroy(item);
            }
            foreach (var item in UnityEngine.GameObject.FindGameObjectsWithTag(Constants.PLANTABLE_TAG))
            {
                Destroy(item);
            }
        }


        private void OnRechargeCallBack(float rechargeTime)
        {
            inventoryUIManager.RechargeBar(rechargeTime);
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
            spawnManager.Init();
            GameStage.gameStageEnum = GameStageEnum.NotPaused;

        }
        private void GameOver()
        {
            GameStage.gameStageEnum = GameStageEnum.NotPausable;
            foodSelectorManager.EndGame();
            uIManager.GameOver();
            playerController.SetDeathState();
            spawnManager.StopSpawning();
            StopAllCoroutines();
            DestroyObjectsInScene();
            DestroyAnimals();
            bossManager.GameOver();
            musicManager.PlayMusic(MusicThemes.Lose);
            animalsLeftUIManager.Init();
            playerUIManager.CancelFill();

        }

        
        internal void Win()
        {

            playerController.CanMove = false;
            if (uIManager.CheckPointsCalc())
            {
                outroController.OutroStart();
                foodSelectorManager.EndGame();
                uIManager.Win();
                animalsLeftUIManager.Init();
                musicManager.FadeCurrentMusic(0, 0.5f); ;
                playerUIManager.CancelFill();
                GameStage.gameStageEnum = GameStageEnum.NotPausable;

            }
            else
            {
                StartCoroutine(WaitForPointsCalcCoroutine());
            }
        }

        internal void EndRound()
        {
            uIManager.InGameNotification(2f, Constants.ROUND_COMPLETED_TEXT, false, NotificationType.Success);
            spawnManager.StopSpawning();
            consecutiveShootsManager.CalculatePoints();
            StartCoroutine(WaitForPointsCalcCoroutine());
        }

        internal void NextRound()

        {
            uIManager.InGameNotification(2f, Constants.NEW_WAVE_TEXT, false, NotificationType.Warnining);
            levelManager.NextRound();
            playerController.CanMove = true;
        }
        private void OnLoseLivePlayerActionCallback(int lives)
        {
            uIManager.ManageLives(lives);
            //consecutiveShootsManager.Reset();
            if (lives == 0)
            {
                GameOver();
            }
            else
            {
                camerasManager.ShakeCurrentCamera(lives);
            }
        }



        private static void DestroyAnimals()
        {
            GameObject[] animals = GameObject.FindGameObjectsWithTag(Constants.ANIMAL_TAG);
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

        private void OnPointsGainedCallBack(int points, Transform transform, bool isFed)
        {
            scoreManager.Score += points;
            if (isFed)
            {
                levelManager.CurrentFedAnimals++;
                levelManager.LevelAnimalCheck();
                particleSystemManager.SpawnFedParticles(transform);
            }
        }




        IEnumerator WaitForPointsCalcCoroutine()
        {

            if (uIManager.CheckPointsCalc())
            {
                yield return new WaitForSeconds(.5f);
                updateMenuUIManager.SetActivePowerUpMenu();
                playerController.CanMove = false;
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