using System;
using System.Collections;
using NUnit.Framework;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FeedTheBeasts.Scripts
{

    public class UIAnimalScoreController : MonoBehaviour
    {
        [SerializeField] TMP_Text[] txtScore;
        [SerializeField] float timeText;
        [SerializeField] float posAddPerFrame;
        [SerializeField] float txtIncreasePerFrame;
        [SerializeField] float transparencyPerFrame;
        [SerializeField] Color[] colorsText;

        int index;

        void Awake()
        {
            Assert.IsTrue(txtScore.Length > 0, "ERROR: array is empty");
            index = 0;

            foreach (var item in txtScore)
            {
                item.gameObject.SetActive(false);
            }
        }

        internal void AddMarker(int points, bool isPreferred)
        {
            if (index == txtScore.Length - 1)
            {
                index = 0;
            }
            else
            {
                index++;
            }
            if (!txtScore[index].gameObject.activeSelf)
            {
                txtScore[index].gameObject.SetActive(true);
            }
            if (isPreferred)
            {
                txtScore[index].text = $"+ {points}";
                int indexColor = Random.Range(0, colorsText.Length);
                txtScore[index].color = colorsText[indexColor];
                StartCoroutine(TextEffectSize(index));
            }
            StartCoroutine(TextEffectMovement(index));
            StartCoroutine(TextTransparentEffect(index));
        }

        IEnumerator TextEffectMovement(int index)
        {
            Vector2 randomDirection = Random.insideUnitCircle;
            RectTransform txtRecTransfrom = txtScore[index].GetComponent<RectTransform>();
            float addedMovement = 0;
            float time = 0;
            while (time <= timeText)
            {
                time += Time.deltaTime;
                addedMovement += posAddPerFrame;
                Vector2 newDirection = new Vector2(randomDirection.x + addedMovement, randomDirection.y + addedMovement);
                txtRecTransfrom.anchoredPosition = newDirection;
                yield return null;
            }

            txtScore[index].gameObject.SetActive(false);
        }
        IEnumerator TextEffectSize(int index)
        {
            float addedSize = 0;
            float time = 0;
            while (time <= timeText)
            {
                time += Time.deltaTime;
                addedSize += txtIncreasePerFrame;
                txtScore[index].fontSize += addedSize;
                yield return null;
            }

            txtScore[index].gameObject.SetActive(false);
        }

        IEnumerator TextTransparentEffect(int index)
        {
            Color originalColor = txtScore[index].color; //temp
            float temp_a = originalColor.a; //okay
            Color temp = txtScore[index].color;
            while (txtScore[index].color.a >= 0)
            {
                temp_a -= transparencyPerFrame;
                temp.a = temp_a;
                txtScore[index].color = temp;
                yield return null;
            }
        }
    }

}