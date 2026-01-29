using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Animations;

namespace FeedTheBeasts.Scripts
{
    public class BossManager : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] GameObject goBoss;
        [SerializeField] float spawnTime;
        [Header("References")]
        [SerializeField] Player player;
        [SerializeField] UIManager uIManager;
        [SerializeField] AnimalsLeftUIManager animalsLeftUIManager;
        [SerializeField] MusicManager musicManager;
        [SerializeField] ScoreManager scoreManager;
        [SerializeField] ParticleSystemManager particleSystemManager;
        BossController bossController;

        LevelManager levelManager;
        AnimalHunger animalHunger;

        GameObject spawnedBoss;
        CamerasManager camerasManager;

        readonly float offset = 4f;
        bool isSpawned;
        public event Action OnBossDefeatedEvent;



        void Start()
        {
            camerasManager = CamerasManager.Instance;
            levelManager = LevelManager.Instance;
        }

        void Awake()
        {
            Assert.IsNotNull(goBoss, "Error: goBoss not added");
            Assert.IsNotNull(uIManager, "Error: uIManager not added");
            Assert.IsNotNull(musicManager, "Error: musicManager not added");
            Assert.IsNotNull(animalsLeftUIManager, "Error: animalsLeftUIManager not added");
            isSpawned = false;

        }
        internal void SpawnBoss()
        {
            musicManager.FadeCurrentMusic(0, 2f);
            uIManager.InGameNotification(spawnTime, Constants.SPAWN_MOOSE_TEXT, true, NotificationType.Warnining);
            StartCoroutine(SpawningWaitingTime());
        }

        IEnumerator SpawningWaitingTime()

        {
            yield return new WaitForSeconds(spawnTime);

            Vector3 spawnPosition = new Vector3(0,
                                      goBoss.transform.position.y,
                                      camerasManager.UpperLimitCamera + offset * -Mathf.Sign(camerasManager.UpperLimitCamera));

            spawnedBoss = Instantiate(goBoss, spawnPosition, goBoss.transform.rotation);

            musicManager.PlayMusic(MusicThemes.Boss);
            musicManager.FadeCurrentMusic(1, 2f);
            animalHunger = spawnedBoss.GetComponent<AnimalHunger>();
            animalsLeftUIManager.BossHungerSetUp(animalHunger);
            bossController = spawnedBoss.GetComponent<BossController>();
            bossController.idleStateBoss.OnSpawnEvent += OnSpawnCallBack;
            isSpawned = true;

        }

        private void OnSpawnCallBack(DestroyOutOfBounds bounds, bool hasScaped)
        {
            bounds.OnLoseLifeEvent += OnLoseLifeCallBack;
        }

        private void OnLoseLifeCallBack(bool hasScaped)
        {
            if (hasScaped)
            {
                levelManager.EscapedAnimals++;
            }
            player.Lives--;
        }

        void Update()
        {
            if (isSpawned && animalHunger.CurrentHunger <= 0)
            {
                PlayerController playerController = player.GetComponent<PlayerController>();
                playerController.CanMove = false;
                OnBossDefeatedEvent?.Invoke();
                bossController.IsFed();
                isSpawned = false;
            }
        }


        internal void GameOver()
        {
            StopAllCoroutines();
            BossController bossController = goBoss.GetComponent<BossController>();
            bossController.bossStates = bossController.runStateBoss;
            if (spawnedBoss != null)
            {
                Destroy(spawnedBoss);
            }
        }

        private void OnPointsGainedCallBack(int points, AnimalHunger animalHunger, bool isFed)
        {
            scoreManager.Score += points;
            if (isFed)
            {
                // levelManager.LevelAnimalCheck();
                particleSystemManager.SpawnFedParticles(animalHunger.transform);
            }


        }

    }

}