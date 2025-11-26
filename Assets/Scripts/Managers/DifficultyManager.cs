using System;
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

        void Start()
        {
            levelManager = LevelManager.Instance;
        }
        void Awake()
        {
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
                    IntervalSpawnMin -= IntervalSpawnDecrese;
                    IntervalSpawnMax -= IntervalSpawnDecrese;
                    IntervalSpawnAggressiveMin -= IntervalAggresiveSpawnDecrease;
                    IntervalSpawnAggressiveMax -= IntervalAggresiveSpawnDecrease;
                    break;
                case Levels.Level3:
                    IntervalSpawnMin -= IntervalSpawnDecrese;
                    IntervalSpawnMax -= IntervalSpawnDecrese;
                    IntervalSpawnAggressiveMin -= IntervalAggresiveSpawnDecrease;
                    IntervalSpawnAggressiveMax -= IntervalAggresiveSpawnDecrease;
                    break;
                case Levels.Level4:
                    IntervalSpawnMin -= IntervalSpawnDecrese;
                    IntervalSpawnMax -= IntervalSpawnDecrese;
                    IntervalSpawnAggressiveMin -= IntervalAggresiveSpawnDecrease;
                    IntervalSpawnAggressiveMax -= IntervalAggresiveSpawnDecrease;
                    break;
            }

        }

    }

}