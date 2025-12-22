using System;
using NUnit.Framework;
using UnityEngine;
using RangeAttribute = UnityEngine.RangeAttribute;

namespace FeedTheBeasts.Scripts

{
    public class ScoreManager : MonoBehaviour
    {
        [Header("Score Properties")]
        int score;
        [SerializeField, Range(100, 1000)] int scoreNewLife;
        int scoreNextLevel;

        public int Score
        {
            get => score;

            set
            {
                score = value;
                livesAndPointsUIManager.ManageScore(Score);
                if (score >= scoreNextLevel)
                {
                    player.Lives++;
                    scoreNextLevel += scoreNewLife;
                }

            }
        }

        int scoreMult;

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
        }

        internal void InsertMultiplayer()
        {
            Score *= scoreMult;
        }
    }

}

