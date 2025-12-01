using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace FeedTheBeasts.Scripts
{


    public class LivesAndPointsUIManager : MonoBehaviour
    {
        [SerializeField] TMP_Text txtScore;
        [SerializeField] TMP_Text txtScoreStr;
        [SerializeField] GameObject goLives;
        [SerializeField] RectTransform lifeContainer;
        List<GameObject> lifeList;
        int previousNumberOfLifes;

        void Awake()
        {
            Assert.IsNotNull(txtScore, "ERROR: txtScore is empty on UIManager");
            Assert.IsNotNull(txtScoreStr, "ERROR: txtScoreStr is empty on UIManager");
            Assert.IsNotNull(goLives, "ERROR: lifes game object is empty on UIManager");
            Assert.IsNotNull(lifeContainer, "ERROR: life container is empty on UIManager");

            lifeList = new List<GameObject>();
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
                    GameObject go = lifeList[lastIndex];
                    Destroy(go);
                    lifeList.RemoveAt(lastIndex);
                }

            }
            if (previousNumberOfLifes < lives)

            {
                for (int i = previousNumberOfLifes; i < lives; i++)
                {
                    GameObject newLife = Instantiate(goLives, lifeContainer);
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

        internal void ManageScore(int points)
        {

            txtScore.text = points.ToString();
            Debug.Log("test");
            //if score > 0
            StartCoroutine(ScoreTextEffect());
        }

        IEnumerator ScoreTextEffect()
        {
            float originalSize = txtScore.fontSizeMax;
            float sizeToAdd = 5f;
            Debug.Log($"originalSize {originalSize}   originalSize + sizeToAdd {originalSize + sizeToAdd}");

            while (txtScore.fontSizeMax <= originalSize + sizeToAdd)
            {
                txtScore.fontSizeMax += 0.25f;
                yield return null;
            }

            while (txtScore.fontSizeMax >= originalSize)
            {
                txtScore.fontSizeMax -= 0.25f;
                yield return null;
            }
            txtScore.fontSizeMax = originalSize;

        }
        
    }

}