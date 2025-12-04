using System.Collections;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

namespace FeedTheBeasts.Scripts
{
    public class FruitBasket : MonoBehaviour
    {
        //[SerializeField] int totalResistance;

        [SerializeField] float totalTimeInScene;
        [SerializeField] Image progressBar;


        void Awake()
        {
            progressBar.fillAmount = 1;

            StartCoroutine(FillBarCoroutine());
        }

        IEnumerator FillBarCoroutine()
        {
            float currentTime = 0;
            while (currentTime <= totalTimeInScene)
            {
                currentTime += Time.deltaTime;
                float progress = Mathf.Clamp01(currentTime / totalTimeInScene);
                progressBar.fillAmount = 1 - progress;
                yield return null;
            }
            Destroy(gameObject);

        }
    }
}

