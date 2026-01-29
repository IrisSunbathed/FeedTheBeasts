using System;
using NUnit.Framework;
using UnityEngine;
using RangeAttribute = UnityEngine.RangeAttribute;

namespace FeedTheBeasts.Scripts

{
    public class ScoreManager : MonoBehaviour
    {
        [Header("Score Properties")]
        [SerializeField, Range(100, 1000)] int scoreNewLife;
        int score;
        int scoreNextLevel;
        int scorePreviousLife;
        int lifesObtained = 1;

        public int Score
        {
            get => score;

            set
            {
                score = value;
                livesAndPointsUIManager.ManageScore(Score);
                if (score - scorePreviousLife >= scoreNextLevel)

                {
                    player.Lives += lifesObtained;
                    scoreNextLevel += scoreNewLife;
                    scorePreviousLife = value;

                }

            }
        }

        [Header("References")]

        [SerializeField] Player player;
        [SerializeField] ScoreUIManager livesAndPointsUIManager;


        void Awake()
        {

            Assert.IsNotNull(player, "ERROR: player not added");
            Assert.IsNotNull(livesAndPointsUIManager, "ERROR: uIManager not added");
            scoreNextLevel = scoreNewLife;

        }

        internal void Init()
        {
            scoreNextLevel = scoreNewLife;
            Score = 0;
            lifesObtained = 1;
        }

        internal void ScorePowerUp()
        {
            scoreNextLevel /= 2;
            lifesObtained += 2;
        }
    }

}

