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
        public event Action OnMaxScoreReached;

        int lives;

        bool isInvincible;

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
                    if (!isInvincible)
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
                score = value;
                if (score >= scoreNextLevel)
                {
                    Debug.Log(scoreGetLife);
                    Lives++;
                    OnMaxScoreReached?.Invoke();
                    scoreNextLevel += scoreGetLife;


                }

            }
        }

        [SerializeField, Range(50, 1000)] int scoreGetLife;
        int scoreNextLevel;


        void Awake()
        {
            scoreNextLevel = scoreGetLife;

        }

        internal void Init()
        {
            previousNumberOfLifes = initialLives;
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