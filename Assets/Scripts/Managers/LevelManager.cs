using System;
using NUnit.Framework;
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
        int currentFedAnimals;
        internal int EscapedAnimals { get; set; }
      //  int remainingAnimals;
        int animalGoalPerLevel;

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


            CurrentLevel = Levels.Level1;

            animalGoalPerLevel = feedAnimalsGoal / Enum.GetNames(typeof(Levels)).Length;
        }

        internal void AnimalFed()
        {
            currentFedAnimals++;
            
            AnimalsLeft = feedAnimalsGoal - currentFedAnimals + EscapedAnimals;
           // remainingAnimals = feedAnimalsGoal - currentFedAnimals + EscapedAnimals;
            animalsLeftUIManager.AdjustBar(feedAnimalsGoal, currentFedAnimals + EscapedAnimals);

            Debug.Log($"remainingAniamls: {AnimalsLeft}");

            if (AnimalsLeft % animalGoalPerLevel == 0)
            {
                if ((int)CurrentLevel == Enum.GetNames(typeof(Levels)).Length)
                {
                    worldManager.Win();
                }
                else
                {
                    CurrentLevel++;
                    difficultyManager.AddDifficultyLevel();
                }
            }
        }
    }

}