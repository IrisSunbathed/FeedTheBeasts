using System;
using System.Collections;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

namespace FeedTheBeasts.Scripts
{

    public class LevelManager : MonoBehaviour
    {
        static LevelManager instance;
        public static LevelManager Instance => instance;

        public Levels CurrentLevel { get; set; }

        [Header("References")]
        [SerializeField] AnimalsLeftUIManager animalsLeftUIManager;
        [SerializeField] WorldManager worldManager;
        [SerializeField] DifficultyManager difficultyManager;
        [SerializeField] BossManager bossManager;
        [SerializeField] OutroController outroController;
        [SerializeField] ConsecutiveShootsManager consecutiveShootsManager;
        [Header("Configuration")]
        [SerializeField] internal int feedAnimalsGoal;
        [SerializeField] float timeGameEnding;

        int currentFedAniamls;
        internal int CurrentFedAnimals
        {
            get => currentFedAniamls;
            set
            {
                currentFedAniamls = value;
            }
        }

        int escapedAnimals;
        internal int EscapedAnimals
        {
            get => escapedAnimals;
            set
            {
                escapedAnimals = value;
            }
        }
        internal int AnimalGoalPerLevel { get; private set; }

        public int AnimalsLeft { get; set; }




        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            Assert.IsNotNull(animalsLeftUIManager, "ERROR: animalsLeftUIManager is not added to FoodSelectorManager");
            Assert.IsNotNull(worldManager, "ERROR: worldManager is not added to FoodSelectorManager");
            Assert.IsNotNull(difficultyManager, "ERROR: difficultyManager is not added to FoodSelectorManager");
            Assert.IsNotNull(bossManager, "ERROR: bossManager is not added to FoodSelectorManager");
            Assert.IsNotNull(outroController, "ERROR: outroController is not added to FoodSelectorManager");
            Assert.IsNotNull(consecutiveShootsManager, "ERROR: consecutiveShootsManager is not added to FoodSelectorManager");


            bossManager.OnBossDefeatedEvent += OnBossDefeatedCallBack;
            if (feedAnimalsGoal % (Enum.GetNames(typeof(Levels)).Length - 1) != 0)
            {
                while (feedAnimalsGoal % (Enum.GetNames(typeof(Levels)).Length - 1) != 0)
                {
                    feedAnimalsGoal++;
                }
                Debug.LogWarning($"The number of animals has to fit the number of levels. The number of animals has been risen to: {feedAnimalsGoal}");
            }
            AnimalGoalPerLevel = feedAnimalsGoal / (Enum.GetNames(typeof(Levels)).Length - 1);
            Init();
        }

        private void OnBossDefeatedCallBack()
        {
            worldManager.Win();
        }
        internal void Init()
        {
            CurrentLevel = Levels.Level1;
            CurrentFedAnimals = 0;
            EscapedAnimals = 0;


        }

        internal void LevelAnimalCheck()
        {
            animalsLeftUIManager.AdjustBar(feedAnimalsGoal, CurrentFedAnimals + EscapedAnimals);

            Debug.Log($"(currentFedAnimals {CurrentFedAnimals} + EscapedAnimals {EscapedAnimals}) % AnimalGoalPerLevel {AnimalGoalPerLevel}== 0");
            if ((CurrentFedAnimals + EscapedAnimals) % AnimalGoalPerLevel == 0)
            {
                if ((int)CurrentLevel != Enum.GetNames(typeof(Levels)).Length)
                {
                    Debug.Log($"(int)CurrentLevel {(int)CurrentLevel} != Enum.GetNames(typeof(Levels)).Length {Enum.GetNames(typeof(Levels)).Length}");

                    worldManager.NextRound();
                }

            }
        }

        internal void NextRound()
        {
            CurrentLevel++;
            Debug.Log(CurrentLevel);
            difficultyManager.AddDifficultyLevel();

        }

    }

}