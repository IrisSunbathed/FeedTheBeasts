using System;
using System.Collections;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using RangeAttribute = UnityEngine.RangeAttribute;

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
        [SerializeField] SpawnManager spawnManager;
        [Header("Configuration")]
        [SerializeField] internal int feedAnimalsGoal;
        [SerializeField] float timeGameEnding;

        int currentFedAniamls;
        internal int CurrentFedAnimals
        {
            get => currentFedAniamls;
            set
            {
                if (value > currentFedAniamls)
                {
                    currentFedAnimalsRound++;
                }
                currentFedAniamls = value;



            }
        }

        int escapedAnimals;
        internal int EscapedAnimals
        {
            get => escapedAnimals;
            set
            {
                if (value > escapedAnimals)
                {
                    escapedAnimalsRound++;
                }
                escapedAnimals = value;
            }
        }

        int escapedAnimalsRound;
        int currentFedAnimalsRound;
        internal int LevelAnimalGoal { get; private set; }

        int avarageAniamlsPerLevel;
        int[] matrixEnemies;
        int baseMatrix;
      //  public int AnimalsLeft { get; set; }

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
            Assert.IsNotNull(spawnManager, "ERROR: spawnManager is not added to FoodSelectorManager");
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

            avarageAniamlsPerLevel = feedAnimalsGoal / (Enum.GetNames(typeof(Levels)).Length - 1);
            matrixEnemies = new int[Enum.GetNames(typeof(Levels)).Length - 1];
            baseMatrix = (Enum.GetNames(typeof(Levels)).Length - 1) / 2;

            for (int i = -baseMatrix; i < baseMatrix; i++)
            {
                int temp = i;
                if (temp >= 0)
                {
                    temp = i + 1;
                }
                matrixEnemies[i + baseMatrix] = temp + temp;
            }


            Init();
            GetAnimalsLevel();
        }

        private void GetAnimalsLevel()
        {
            LevelAnimalGoal = avarageAniamlsPerLevel + matrixEnemies[(int)CurrentLevel - 1];
            // for (int i = 1; i <= Enum.GetNames(typeof(Levels)).Length - 1; i++)
            // {
            //     Debug.Log($"avarage: {avarageAniamlsPerLevel} + matrixEnemies[i - 1] { matrixEnemies[i - 1]}");
            //     Debug.Log($"Level {i} avarage: {avarageAniamlsPerLevel + matrixEnemies[i - 1]}");
            // }
        }

        private void OnBossDefeatedCallBack()
        {
            worldManager.Win();
        }
        internal void Init()
        {
            CurrentLevel = Levels.Level1;
            CurrentFedAnimals = 0;
            escapedAnimalsRound = 0;
            EscapedAnimals = 0;
            currentFedAnimalsRound = 0;


        }

        internal void LevelAnimalCheck()
        {

            animalsLeftUIManager.AdjustBar(feedAnimalsGoal, CurrentFedAnimals + EscapedAnimals);
           Debug.Log($"(currentFedAnimalsRound {currentFedAnimalsRound} + escapedAnimalsRound {escapedAnimalsRound}) ==  {LevelAnimalGoal}");
            if (currentFedAnimalsRound + escapedAnimalsRound == LevelAnimalGoal)
            {

                spawnManager.StopSpawning();
                if ((int)CurrentLevel != Enum.GetNames(typeof(Levels)).Length)
                {
                    currentFedAnimalsRound = 0;
                    escapedAnimalsRound = 0;
                    worldManager.NextRound();
                }
            }
        }

        internal void NextRound()
        {
            CurrentLevel++;
            if ((int)CurrentLevel != Enum.GetNames(typeof(Levels)).Length)
            {
                GetAnimalsLevel();
            }
            difficultyManager.AddDifficultyLevel();

        }

    }

}