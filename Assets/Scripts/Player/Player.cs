using System;
using UnityEngine;

namespace FeedTheBeasts.Scripts
{

    public class Player : MonoBehaviour
    {
        [Header("Health Properties")]

        [SerializeField, Range(1, 7)] int initialLives;
        int previousNumberOfLifes;

        public event Action<int> OnLoseLivePlayerAction;
        public event Action<int> OnGainedLivePlayerAction;
        public event Action<int> OnPointsAddedAction;

        int lives;

        public int Lives
        {
            get => lives;
            set
            {
                lives = value;
                if (lives >= previousNumberOfLifes)
                {
                    OnGainedLivePlayerAction?.Invoke(Lives);
                }
                else
                {
                    OnLoseLivePlayerAction?.Invoke(Lives);
                }
                previousNumberOfLifes = lives;

            }
        }


        [Header("Score Properties")]
        int score;

        public int Score
        {
            get => score;

            set
            {
                score = value;
                OnPointsAddedAction?.Invoke(Score);
                score = value;
                if (score >= scoreGetLifeCounter)
                {
                    Debug.Log(scoreGetLife);
                    Lives++;
                    scoreGetLifeCounter += scoreGetLife;
                }

            }
        }

        [SerializeField, Range(50, 1000)] int scoreGetLife;
        int scoreGetLifeCounter;


        void Awake()
        {
            scoreGetLifeCounter = scoreGetLife;

        }

        internal void Init()
        {
            previousNumberOfLifes = initialLives;
            Lives = initialLives;
            Score = 0;
        }
    }

}