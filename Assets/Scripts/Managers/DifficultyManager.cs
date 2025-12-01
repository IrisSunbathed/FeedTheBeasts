using System;
using NUnit.Framework;
using UnityEngine;

namespace FeedTheBeasts.Scripts
{
    public class DifficultyManager : MonoBehaviour
    {

        [Header("Difficulty configuration")]
        LevelManager levelManager;
        [SerializeField] float startDelay;
        float startDelaySum;
        public float StartDelay { get => startDelaySum; private set { startDelaySum = value; } }
        [SerializeField] float startDelayAggressiveAnimals;
        float startDelayAggressiveAnimalsSum;
        public float StartDelayAggressiveAnimals { get => startDelayAggressiveAnimalsSum; private set { startDelayAggressiveAnimalsSum = value; } }
        [SerializeField] float intervalSpawnMin;
        float intervalSpawnMinSum;
        public float IntervalSpawnMin { get => intervalSpawnMinSum; private set { intervalSpawnMinSum = value; } }
        [SerializeField] float intervalSpawnMax;
        float intervalSpawnMaxSum;
        public float IntervalSpawnMax { get => intervalSpawnMaxSum; private set { intervalSpawnMaxSum = value; } }
        [SerializeField] float intervalSpawnDecrese;
        float intervalSpawnDecreseSum;
        public float IntervalSpawnDecrese { get => intervalSpawnDecreseSum; private set { intervalSpawnDecreseSum = value; } }

        [SerializeField] float intervalSpawnAggressiveMin;
        float intervalSpawnAggressiveMinSum;
        public float IntervalSpawnAggressiveMin { get => intervalSpawnAggressiveMinSum; private set { intervalSpawnAggressiveMinSum = value; } }
        [SerializeField] float intervalSpawnAggressiveMax;
        float intervalSpawnAggressiveMaxSum;
        public float IntervalSpawnAggressiveMax { get => intervalSpawnAggressiveMaxSum; private set { intervalSpawnAggressiveMaxSum = value; } }
        [SerializeField] float intervalAggresiveSpawnDecrease;
        float intervalAggresiveSpawnDecreaseSum;
        public float IntervalAggresiveSpawnDecrease { get => intervalAggresiveSpawnDecreaseSum; private set { intervalAggresiveSpawnDecreaseSum = value; } }

        [Header("Refences")]

        [SerializeField] SpawnManager spawnManager;

        void Start()
        {
            levelManager = LevelManager.Instance;
        }
        void Awake()
        {
            Assert.IsNotNull(spawnManager, "Spawn Manager is not added");
            Init();

        }

        internal void Init()
        {
            SetDeafualtValues();
        }
        private void SetDeafualtValues()
        {
            StartDelay = startDelay;
            StartDelayAggressiveAnimals = startDelayAggressiveAnimals;
            IntervalSpawnMin = intervalSpawnMin;
            IntervalSpawnMax = intervalSpawnMax;
            IntervalSpawnDecrese = intervalSpawnDecrese;
            IntervalSpawnAggressiveMin = intervalSpawnAggressiveMin;
            IntervalSpawnAggressiveMax = intervalSpawnAggressiveMax;
            IntervalAggresiveSpawnDecrease = intervalAggresiveSpawnDecrease;
        }

        internal void AddDifficultyLevel()
        {
            switch (levelManager.CurrentLevel)
            {
                case Levels.Level1:
                    IntervalSpawnMin -= IntervalSpawnDecrese;
                    IntervalSpawnMax -= IntervalSpawnDecrese;
                    IntervalSpawnAggressiveMin -= IntervalAggresiveSpawnDecrease;
                    IntervalSpawnAggressiveMax -= IntervalAggresiveSpawnDecrease;
                    break;
                case Levels.Level2:
                    spawnManager.Stampede(levelManager.AnimalsLeft / 10);
                    IntervalSpawnMin -= IntervalSpawnDecrese;
                    IntervalSpawnMax -= IntervalSpawnDecrese;
                    IntervalSpawnAggressiveMin -= IntervalAggresiveSpawnDecrease;
                    IntervalSpawnAggressiveMax -= IntervalAggresiveSpawnDecrease;
                    break;
                case Levels.Level3:
                    spawnManager.Stampede(levelManager.AnimalsLeft / 7);
                    IntervalSpawnMin -= IntervalSpawnDecrese;
                    IntervalSpawnMax -= IntervalSpawnDecrese;
                    IntervalSpawnAggressiveMin -= IntervalAggresiveSpawnDecrease;
                    IntervalSpawnAggressiveMax -= IntervalAggresiveSpawnDecrease;
                    break;
                case Levels.Level4:
                    spawnManager.Stampede(levelManager.AnimalsLeft / 5);
                    IntervalSpawnMin -= IntervalSpawnDecrese;
                    IntervalSpawnMax -= IntervalSpawnDecrese;
                    IntervalSpawnAggressiveMin -= IntervalAggresiveSpawnDecrease;
                    IntervalSpawnAggressiveMax -= IntervalAggresiveSpawnDecrease;
                    break;
            }

        }

    }

}