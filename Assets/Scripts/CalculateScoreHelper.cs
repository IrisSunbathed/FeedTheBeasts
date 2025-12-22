using System;
using System.Collections;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using Random = UnityEngine.Random;
using RangeAttribute = UnityEngine.RangeAttribute;

namespace FeedTheBeasts.Scripts
{
    public class CalculateScoreHelper : MonoBehaviour
    {
        [SerializeField] FXSoundsManager fXSoundsManager;
        [SerializeField] ScoreUIManager scoreUIManager;
        [SerializeField, Range(0.1f, 0.25f)] float effectIntensityIncrease;
        [SerializeField, Range(0.5f, 1.5f)] float initialEffectSpeed;
        Coroutine textEffectCoroutine;
        Quaternion originalRotation;
        float pitch;
        float randomRotaion;
        bool hasCoroutineEnded;

        void Awake()
        {
            Assert.IsNotNull(fXSoundsManager, "ERROR: fXSoundsManager is not added");
            Assert.IsNotNull(scoreUIManager, "ERROR: scoreUIManager is not added");
        }

        internal void Configure(RectTransform rectTransform)
        {
            originalRotation = rectTransform.localRotation;
        }
        internal void Calculate(int multiplayer)
        {
            scoreUIManager.IsScoreCalc = false;

            StartCoroutine(InitialMultEffect(multiplayer));
            textEffectCoroutine = null;
        }

        IEnumerator InitialMultEffect(int mult)
        {
            float originalSize = scoreUIManager.scoreMult.fontSize;
            while (scoreUIManager.scoreMult.fontSize < 20)
            {
                scoreUIManager.scoreMult.fontSize += initialEffectSpeed;
                yield return null;
            }
            fXSoundsManager.PlayFX(FXTypes.ClickOnButton, 2f);
            while (scoreUIManager.scoreMult.fontSize > originalSize)
            {
                scoreUIManager.scoreMult.fontSize -= initialEffectSpeed;
                yield return null;
            }
            yield return new WaitForSeconds(0.15f);
            while (scoreUIManager.extraScore.fontSize < 20)
            {
                scoreUIManager.extraScore.fontSize += initialEffectSpeed;
                yield return null;
            }
            scoreUIManager.scoreMult.fontSize = originalSize;
            int finalResult = scoreUIManager.currentExtraScore * mult;
            SetExtraScore(finalResult);
            fXSoundsManager.PlayFX(FXTypes.ClickOnButton, 3f, true);
            while (scoreUIManager.extraScore.fontSize > originalSize)
            {
                scoreUIManager.extraScore.fontSize -= initialEffectSpeed;
                yield return null;
            }
            scoreUIManager.extraScore.fontSize = originalSize;
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(AddSumScore(finalResult));
        }

        private void SetExtraScore(int value)
        {
            scoreUIManager.extraScore.text = $"+{value}";
            scoreUIManager.currentExtraScore = value;
        }

        IEnumerator AddSumScore(int sum)
        {
            hasCoroutineEnded = false;
            float time = 0;
            RectTransform rectTransform = scoreUIManager.txtScore.GetComponent<RectTransform>();
            if (int.TryParse(scoreUIManager.txtScore.text, out int result) & sum > 0)
            {

                int finalResult;
                scoreUIManager.scoreMult.text = "x1";
                pitch = 2;
                finalResult = sum + result;
                while (result <= finalResult)
                {
                    time += Time.deltaTime;
                    result += 1 * Mathf.CeilToInt(time);
                    SetExtraScore(scoreUIManager.currentExtraScore - 1 * Mathf.CeilToInt(time));
                    // scoreUIManager.currentExtraScore -= 1 * Mathf.CeilToInt(time);
                    fXSoundsManager.PlayFX(FXTypes.Points, pitch);
                    pitch = AudioEffect(pitch);
                    // scoreUIManager.extraScore.text = $"+{scoreUIManager.currentExtraScore}";
                    textEffectCoroutine ??= StartCoroutine(TextEffectCoroutine(scoreUIManager.txtScore, 0));
                    scoreUIManager.txtScore.text = result.ToString();
                    yield return null;
                }
                scoreUIManager.txtScore.text = finalResult.ToString();
            }
            SetExtraScore(0);
            hasCoroutineEnded = true;
            StopCoroutine(textEffectCoroutine);
            textEffectCoroutine = null;
            scoreUIManager.IsScoreCalc = true;
            rectTransform.localRotation = originalRotation;
            StartCoroutine(CorrectRotationCoroutine(rectTransform, scoreUIManager.txtScore));

        }

        private float AudioEffect(float pitch)
        {
            if (fXSoundsManager.PlayFX(FXTypes.Points, pitch))
            {
                if (pitch < 3f)
                {
                    pitch += 0.0005f;
                }
                else
                {
                    pitch = 3f;
                }
            }

            return pitch;
        }

        IEnumerator TextEffectCoroutine(TMP_Text tMP_Text, float intensity)
        {
            if (intensity > 25f)
            {
                intensity = 25f;
            }
            randomRotaion = Random.Range(-intensity, intensity);
            RectTransform rectTranform = tMP_Text.GetComponent<RectTransform>();
            rectTranform.Rotate(0, 0, randomRotaion);
            if (tMP_Text.fontSize >= 25)
            {
                tMP_Text.fontSize = 25;
                tMP_Text.fontSize -= 8f;
                yield return new WaitForSeconds(0.05f);
                tMP_Text.fontSize += 8f;
                rectTranform.Rotate(0, 0, -randomRotaion);
                yield return new WaitForSeconds(0.05f);
            }
            else
            {
                tMP_Text.fontSize += intensity;
                yield return new WaitForSeconds(0.05f);
                rectTranform.Rotate(0, 0, -randomRotaion);
                tMP_Text.fontSize -= intensity - effectIntensityIncrease;
                yield return new WaitForSeconds(0.05f);

            }
            if (hasCoroutineEnded)
            {
                if (textEffectCoroutine != null)
                {
                    StopCoroutine(textEffectCoroutine);
                    tMP_Text.fontSize = 10;
                    textEffectCoroutine = null;
                }
            }
            else
            {
                textEffectCoroutine = StartCoroutine(TextEffectCoroutine(tMP_Text, intensity + effectIntensityIncrease));
            }
        }
        IEnumerator CorrectRotationCoroutine(RectTransform rectTransform, TMP_Text score)
        {
            yield return new WaitForSeconds(0.25f);
            if (rectTransform.localRotation != originalRotation && score.fontSize != 10)
            {
                rectTransform.localRotation = originalRotation;
                score.fontSize = 10;
                StartCoroutine(CorrectRotationCoroutine(rectTransform, score));
            }
            else
            {
                StopAllCoroutines();
            }
        }


    }
}
