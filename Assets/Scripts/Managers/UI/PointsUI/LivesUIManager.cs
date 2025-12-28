using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace FeedTheBeasts.Scripts
{

    public class LivesUIManager : MonoBehaviour
    {
        [Header("lives references and configuration")]
        [SerializeField] GameObject goLives;
        [SerializeField] RectTransform lifeContainer;
        int previousNumberOfLifes;
        List<GameObject> lifeList;


        void Awake()
        {
            Assert.IsNotNull(goLives, "ERROR: lifes game object is empty on UIManager");
            Assert.IsNotNull(lifeContainer, "ERROR: life container is empty on UIManager");
            lifeList = new List<UnityEngine.GameObject>();
            ActivateElementsOnMenu(false);

        }

        internal void ManageLives(int lives)
        {
            if (previousNumberOfLifes > lives)
            {
                int lifesToDestroy = previousNumberOfLifes - lives;
                lifesToDestroy = Mathf.Min(lifesToDestroy, lifeList.Count);
                for (int i = lifesToDestroy; i > 0; i--)
                {
                    int lastIndex = lifeList.Count - 1;
                    UnityEngine.GameObject go = lifeList[lastIndex];
                    Destroy(go);
                    lifeList.RemoveAt(lastIndex);
                }

            }
            if (previousNumberOfLifes < lives)

            {

                for (int i = previousNumberOfLifes; i < lives; i++)
                {
                    UnityEngine.GameObject newLife = Instantiate(goLives, lifeContainer);
                    lifeList.Add(newLife);
                }

            }
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
        }
    }

}