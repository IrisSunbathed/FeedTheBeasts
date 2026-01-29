using System;
using System.Collections;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;
using RangeAttribute = UnityEngine.RangeAttribute;

namespace FeedTheBeasts.Scripts
{

    public class LevelManager : MonoBehaviour
    {
        static LevelManager instance;
        public static LevelManager Instance => instance;

        public Levels CurrentLevel { get; set; }

        [Header("Configuration")]
        [SerializeField] internal int feedAnimalsGoal;
        [SerializeField] float timeGameEnding;
        [Header("References")]
        [SerializeField] AnimalsLeftUIManager animalsLeftUIManager;
        [SerializeField] WorldManager worldManager;
        [SerializeField] DifficultyManager difficultyManager;
        [SerializeField] BossManager bossManager;
        [SerializeField] OutroController outroController;
        [SerializeField] ConsecutiveShootsManager consecutiveShootsManager;
        [SerializeField] SpawnManager spawnManager;
        [SerializeField] UpdateMenuUIManager updateMenuUIManager;

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
            Assert.IsNotNull(updateMenuUIManager, "ERROR: updateMenuUIManager is not added to FoodSelectorManager");
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

        void Update()
        {
            ///
            /// TEST
            /// 
            if (Input.GetKeyDown(KeyCode.U))
            {

                EndRound();
            }
        }

        internal void LevelAnimalCheck()
        {

            animalsLeftUIManager.AdjustBar(LevelAnimalGoal, currentFedAnimalsRound + escapedAnimalsRound, (int)CurrentLevel == Enum.GetNames(typeof(Levels)).Length - 1);
            if (currentFedAnimalsRound + escapedAnimalsRound == LevelAnimalGoal)
            {
                EndRound();
            }
        }

        private void EndRound()
        {
            spawnManager.StopSpawning();
            worldManager.EndRound();
            if ((int)CurrentLevel != Enum.GetNames(typeof(Levels)).Length)
            {
                currentFedAnimalsRound = 0;
                escapedAnimalsRound = 0;
            }
        }

        internal void NextRound()
        {
            CurrentLevel++;
            if ((int)CurrentLevel != Enum.GetNames(typeof(Levels)).Length)
            {
                GetAnimalsLevel();
                animalsLeftUIManager.NextRound();
            }

            difficultyManager.AddDifficultyLevel();
            // animalsLeftUIManager.AdjustBar(LevelAnimalGoal, currentFedAnimalsRound + escapedAnimalsRound);

        }

    }

}