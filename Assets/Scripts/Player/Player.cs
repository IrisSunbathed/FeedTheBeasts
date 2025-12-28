using System;
using System.Collections;
using UnityEngine;

namespace FeedTheBeasts.Scripts
{

    public class Player : MonoBehaviour
    {
        [Header("Health Properties")]

        [SerializeField, Range(1, 7)] int initialLives;
        [SerializeField, Range(0.5f, 5f), Tooltip("The amount of time that the characters is invencible")]
        float invensibilityCooldown;
        [SerializeField, Range(0.05f, .5f), Tooltip("The amount of time that the shader color changes when hit")]
        float shaderHitTime;

        [SerializeField] Renderer rendererFarmer;
        int previousNumberOfLifes;

        public event Action<int> OnLoseLivePlayerAction;
        public event Action<int> OnGainedLivePlayerAction;
        public event Action<int> OnPointsAddedAction;
        //public event Action OnMaxScoreReached;

        int lives;

        bool isInvincible;

        public int Lives
        {
            get => lives;
            set
            {
<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream
                lives = value;
                if (lives >= previousNumberOfLifes)
                {
                    OnGainedLivePlayerAction?.Invoke(Lives);
                }
                else
                {
                    if (!isInvincible)
=======
                if (lives < maxLifes)
                {
=======
                if (lives < maxLifes)
                {
>>>>>>> Stashed changes
=======
                if (lives < maxLifes)
                {
>>>>>>> Stashed changes
                    lives = value;
                    if (lives >= previousNumberOfLifes)
>>>>>>> Stashed changes
                    {
                        OnLoseLivePlayerAction?.Invoke(Lives);
                        StartCoroutine(InvencibilityTime());
                        StartCoroutine(HitIndicator());
                    }
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
                if (score >= scoreNextLevel)
                {
                    Lives++;
                  //  OnMaxScoreReached?.Invoke();
                    scoreNextLevel += scoreNewLife;


                }

            }
        }

        [SerializeField, Range(50, 1000)] int scoreNewLife;
        int scoreNextLevel;


        void Awake()
        {
            scoreNextLevel = scoreNewLife;

        }

        internal void Init()
        {
            previousNumberOfLifes = initialLives;
            scoreNextLevel = scoreNewLife;
            Lives = initialLives;
            Score = 0;
        }

        IEnumerator InvencibilityTime()
        {
            isInvincible = true;
            yield return new WaitForSeconds(invensibilityCooldown);
            isInvincible = false;
        }

        IEnumerator HitIndicator()
        {
            Color originalColor = rendererFarmer.material.color;
            rendererFarmer.material.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            rendererFarmer.material.color = originalColor;
        }
    }

}