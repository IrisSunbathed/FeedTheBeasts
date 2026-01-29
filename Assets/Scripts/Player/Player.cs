using System;
using System.Collections;
using UnityEngine;

namespace FeedTheBeasts.Scripts
{

    public class Player : MonoBehaviour
    {
        [Header("Health Properties")]

        [SerializeField, Range(1, 9)]
        internal int initialLives;
        [SerializeField, Range(5, 9)]
        int maxLifes;
        [SerializeField, Range(0.5f, 5f), Tooltip("The amount of time that the characters is invencible")]
        float invensibilityCooldown;
        [SerializeField, Range(0.05f, .5f), Tooltip("The amount of time that the shader color changes when hit")]
        float shaderHitTime;

        [SerializeField] Renderer rendererFarmer;
        int previousNumberOfLifes;

        public event Action<int> OnLoseLivePlayerAction;
        public event Action<int> OnGainedLivePlayerAction;
        //public event Action OnMaxScoreReached;

        int lives;

        bool isInvincible;

        public int Lives
        {
            get => lives;
            set
            {
                if (value >= previousNumberOfLifes & value <= maxLifes)
                {
                    lives = value;
                    OnGainedLivePlayerAction?.Invoke(Lives);
                }
                if (value < previousNumberOfLifes)
                {
                    if (!isInvincible)
                    {
                        lives = value;
                        OnLoseLivePlayerAction?.Invoke(Lives);
                        StartCoroutine(InvencibilityTime());
                        StartCoroutine(HitIndicator());
                    }
                }


                previousNumberOfLifes = lives;

            }
        }

        void OnValidate()
        {
            initialLives = Mathf.Clamp(initialLives, 1, maxLifes);
            maxLifes = Mathf.Clamp(maxLifes, initialLives, 9);
        }

        internal void Init()
        {
            previousNumberOfLifes = initialLives;
            Lives = initialLives;
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