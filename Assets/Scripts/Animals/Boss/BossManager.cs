using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;

namespace FeedTheBeasts.Scripts
{
    public class BossManager : MonoBehaviour
    {
        [SerializeField] GameObject goBoss;
        [SerializeField] UIManager uIManager;
        [SerializeField] float spawnTime;
        [SerializeField] Player player;
        [SerializeField] AnimalsLeftUIManager animalsLeftUIManager;
        [SerializeField] MusicManager musicManager;

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
            musicManager.StopMusic();
            uIManager.InGameWarning(spawnTime, Constants.SPAWN_MOOSE_TEXT);
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
            animalHunger = spawnedBoss.GetComponent<AnimalHunger>();
            animalsLeftUIManager.BossHungerSetUp(animalHunger);
            BossController bossController = spawnedBoss.GetComponent<BossController>();
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
                OnBossDefeatedEvent?.Invoke();
                isSpawned = false;
            }
        }

    }

}