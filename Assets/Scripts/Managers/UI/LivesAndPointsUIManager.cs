using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Assertions;

namespace FeedTheBeasts.Scripts
{


    public class LivesAndPointsUIManager : MonoBehaviour
    {
        [SerializeField] TMP_Text txtScore;
        [SerializeField] TMP_Text txtScoreStr;
        [SerializeField] UnityEngine.GameObject goLives;
        [SerializeField] RectTransform lifeContainer;
        [SerializeField, Range(0.5f, 2.5f)] float sizeToAdd;
        [SerializeField, Range(0.25f, .75f)] float increaseSizePerFrame;
        List<UnityEngine.GameObject> lifeList;
        int previousNumberOfLifes;
        float originalSize;

        void Awake()
        {
            Assert.IsNotNull(txtScore, "ERROR: txtScore is empty on UIManager");
            Assert.IsNotNull(txtScoreStr, "ERROR: txtScoreStr is empty on UIManager");
            Assert.IsNotNull(goLives, "ERROR: lifes game object is empty on UIManager");
            Assert.IsNotNull(lifeContainer, "ERROR: life container is empty on UIManager");

            lifeList = new List<UnityEngine.GameObject>();
            originalSize = txtScore.fontSizeMax;
        }


        internal void Init()
        {
            txtScore.text = 0.ToString();
        }
        // Update is called once per frame
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
            txtScore.gameObject.SetActive(isActive);
            txtScoreStr.gameObject.SetActive(isActive);
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

        internal void ManageScore(int score)
        {
            txtScore.text = score.ToString();
            if (score > 0)
            {
                StartCoroutine(ScoreTextEffect());
            }
        }

        IEnumerator ScoreTextEffect()
        {
            float currentSizeToAdd = sizeToAdd;
            while (txtScore.fontSizeMax <= originalSize + currentSizeToAdd)
            {
                txtScore.fontSizeMax += increaseSizePerFrame;
                yield return null;
            }

            while (txtScore.fontSizeMax >= originalSize)
            {
                txtScore.fontSizeMax -= increaseSizePerFrame;
                yield return null;
            }
            txtScore.fontSizeMax = originalSize;

        }

    }

}