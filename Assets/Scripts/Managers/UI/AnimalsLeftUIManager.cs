using System;
using System.Collections;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
namespace FeedTheBeasts.Scripts
{
    public class AnimalsLeftUIManager : MonoBehaviour
    {

        [SerializeField] Image animalsLeftBar;

        [SerializeField] TMP_Text animalsLeftText;

        float totalBar;




        void Awake()
        {
            Assert.IsNotNull(animalsLeftBar, "animalsLeftBar is empty");
            Assert.IsNotNull(animalsLeftText, "animalsLeftText is empty");

            totalBar = animalsLeftBar.fillAmount;

        }
        internal void Init()
        {
            animalsLeftBar.gameObject.SetActive(false);
            animalsLeftText.gameObject.SetActive(false);
        }

        internal void AdjustBar(int totalAnimals, int currentFedAnimals)
        {
            float progress = Mathf.Clamp01((float)currentFedAnimals / (float)totalAnimals);
            animalsLeftBar.fillAmount = totalBar - progress;
            if (animalsLeftBar.fillAmount == 0)
            {
                animalsLeftText.text = string.Empty;
            }

        }

        internal void StartGame()
        {
            animalsLeftBar.gameObject.SetActive(true);
            animalsLeftText.gameObject.SetActive(true);

            animalsLeftBar.fillAmount = totalBar;
        }

        internal void BossHungerSetUp(AnimalHunger animalHunger)
        {
            animalsLeftText.text = Constants.BOSS_BAR_TEXT;
            StartCoroutine(FillBossHungerBar());
            animalHunger.OnBossFeedEvent += SetBossHungerBar;
        }
        IEnumerator FillBossHungerBar()
        {
            float progress = 0;
            while (animalsLeftBar.fillAmount < 1)
            {
                progress += 0.0015f;
                animalsLeftBar.fillAmount = progress;
                yield return null;
            }
            animalsLeftBar.fillAmount = 1;
        }

        internal void SetBossHungerBar(float progress)
        {
            animalsLeftBar.fillAmount = 1 * progress;
            
        }
    }
}
