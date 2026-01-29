using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Microsoft.Unity.VisualStudio.Editor;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using RangeAttribute = UnityEngine.RangeAttribute;

namespace FeedTheBeasts.Scripts
{

    public class LivesUIManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Player player;
        [SerializeField] FXSoundsManager fXSoundsManager;
        [Header("Configuration")]
        [SerializeField] GameObject goLives;
        [SerializeField] RectTransform lifeContainer;
        [SerializeField, Range(0.25f, 5f)] float fadeOutDuration;
        [SerializeField, Range(0.5f, 2f)] float lifesTimeOnScreen;
        [SerializeField] GameObject newLifeParticles;

        int initialLives;
        int livesCounter;

        int previousNumberOfLifes;
        List<GameObject> lifeList;

        Coroutine coroutine;


        void Awake()
        {
            Assert.IsNotNull(goLives, "ERROR: lifes game object is empty on UIManager");
            Assert.IsNotNull(lifeContainer, "ERROR: life container is empty on UIManager");
            Assert.IsNotNull(fXSoundsManager, "ERROR: fXSoundsManager is empty on UIManager");
            Assert.IsNotNull(player, "ERROR: player is empty on UIManager");
            initialLives = player.initialLives;
            lifeList = new List<UnityEngine.GameObject>();
            ActivateElementsOnMenu(false);
        }

        void Update()
        {
            //TESTING AREA
            if (Input.GetKeyDown(KeyCode.N))
            {
                ManageLives(previousNumberOfLifes + 1);
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                ManageLives(previousNumberOfLifes - 1);
            }

        }

        private void ShowLifes()
        {
            foreach (var life in lifeList)
            {
                Image image = life.GetComponent<Image>();
                image.DOKill();
                Color endValue = new Color(image.color.r, image.color.g, image.color.b, 80);
                image.color = endValue;
            }

            coroutine = StartCoroutine(FadeOutCoroutine());
        }

        IEnumerator FadeOutCoroutine()
        {
            yield return new WaitForSeconds(lifesTimeOnScreen);
            foreach (var life in lifeList)
            {
                Image image = life.GetComponent<Image>();
                Color endValue = new Color(image.color.r, image.color.g, image.color.b, 0f);
                image.DOColor(endValue, fadeOutDuration);
            }
            coroutine = null;
        }

        internal void ManageLives(int lives)
        {
            StopAllCoroutines();

            if (previousNumberOfLifes > lives)
            {
                int lifesToDestroy = previousNumberOfLifes - lives;
                lifesToDestroy = Mathf.Min(lifesToDestroy, lifeList.Count);
                for (int i = lifesToDestroy; i > 0; i--)
                {
                    int lastIndex = lifeList.Count - 1;
                    GameObject go = lifeList[lastIndex];
                    Destroy(go);
                    lifeList.RemoveAt(lastIndex);
                }

            }
            if (previousNumberOfLifes < lives)

            {

                for (int i = previousNumberOfLifes; i < lives; i++)
                {
                    livesCounter++;
                    if (livesCounter > initialLives)
                    {
                        GameObject newLife = Instantiate(goLives, lifeContainer);
                        lifeList.Add(newLife);
                        ParticleSystem fxParticleSys = newLifeParticles.GetComponent<ParticleSystem>();
                        var main = fxParticleSys.main;
                        main.maxParticles = lives;
                        fxParticleSys.Play();
                        fXSoundsManager.PlayFX(FXTypes.GetLife, forcedPlay: true);
                    }
                    else
                    {

                        GameObject newLife = Instantiate(goLives, lifeContainer);
                        lifeList.Add(newLife);
                    }
                }

            }
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            ShowLifes();
            previousNumberOfLifes = lives;
        }

        internal void ActivateElementsOnMenu(bool isActive)
        {
            if (lifeList != null)
            {
                foreach (var item in lifeList)
                {
                    if (item != null)
                    {
                        item.SetActive(isActive);

                    }
                }

            }
            livesCounter = 0;
        }
    }

}