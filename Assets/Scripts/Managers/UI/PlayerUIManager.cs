using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

namespace FeedTheBeasts.Scripts
{

    public class PlayerUIManager : MonoBehaviour
    {

        Coroutine coroutine;

        public event Action OnCompletionEvent;

        [SerializeField] Image progressBar;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
            Assert.IsNotNull(progressBar, "ERROR: progress bar is not added");
            progressBar.fillAmount = 0;
        }

        // Update is called once per frame
        internal void FillBar(float totalTime)
        {
            coroutine ??= StartCoroutine(FillBarCoroutine(totalTime));
        }
        IEnumerator FillBarCoroutine(float totalTime)
        {
            float currentTime = 0;
            while (currentTime <= totalTime)
            {
                currentTime += Time.deltaTime;
                Debug.Log($"currentTime {currentTime}");
                float progress = Mathf.Clamp01(currentTime / totalTime);
                progressBar.fillAmount = 1 - progress;
                yield return null;
            }
            OnCompletionEvent?.Invoke();
            coroutine = null;

        }

        internal void CancelFill()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
                progressBar.fillAmount = 0;
            }
        }
    }

}