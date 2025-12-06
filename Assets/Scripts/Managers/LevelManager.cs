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

        [SerializeField] internal int feedAnimalsGoal;

        [SerializeField] AnimalsLeftUIManager animalsLeftUIManager;
        [SerializeField] WorldManager worldManager;
        [SerializeField] DifficultyManager difficultyManager;
        [SerializeField] BossManager bossManager;
        [SerializeField] OutroController outroController;
        [SerializeField] float timeGameEnding;
        internal int currentFedAnimals;
        internal int EscapedAnimals { get; set; }
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
            StartCoroutine(WaitUntilWin());

        }

        IEnumerator WaitUntilWin()
        {
            
            worldManager.Win();
            yield return new WaitForSeconds(timeGameEnding);
            outroController.OutroStart();

        }

        internal void Init()
        {
            CurrentLevel = Levels.Level1;
            currentFedAnimals = 0;
            EscapedAnimals = 0;


        }

        internal void AnimalFed()
        {
            currentFedAnimals++;
            animalsLeftUIManager.AdjustBar(feedAnimalsGoal, currentFedAnimals + EscapedAnimals);
            Debug.Log($"AnimalGoalPerLevel: {AnimalGoalPerLevel} currentFedAnimals: {currentFedAnimals} EscapedAnimals: {EscapedAnimals} ");
            if ((currentFedAnimals + EscapedAnimals) % AnimalGoalPerLevel == 0)
            {
                if ((int)CurrentLevel != Enum.GetNames(typeof(Levels)).Length)
                {
                    CurrentLevel++;
                    Debug.Log(CurrentLevel);
                    difficultyManager.AddDifficultyLevel();
                }

            }
        }
    }

}