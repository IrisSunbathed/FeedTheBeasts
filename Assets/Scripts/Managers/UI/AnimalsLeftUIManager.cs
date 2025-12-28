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

        internal float TotalBar { get => totalBar; set { totalBar = value; } }




        void Awake()
        {
            Assert.IsNotNull(animalsLeftBar, "animalsLeftBar is empty");
            Assert.IsNotNull(animalsLeftText, "animalsLeftText is empty");
            animalsLeftBar.fillAmount = 1;
            TotalBar = animalsLeftBar.fillAmount;

        }
        internal void Init()
        {
            animalsLeftBar.gameObject.SetActive(false);
            animalsLeftText.gameObject.SetActive(false);
        }

        internal void AdjustBar(int totalAnimals, int currentFedAnimals)
        {
            float progress = Mathf.Clamp01((float)currentFedAnimals / (float)totalAnimals);
            animalsLeftBar.fillAmount = TotalBar - progress;
            if (animalsLeftBar.fillAmount == 0)
            {
                animalsLeftText.text = string.Empty;
            }
            else
            {
                animalsLeftText.text = Constants.ANIMALS_LEFT_TEXT;
            }

        }

        internal void StartGame()
        {
            animalsLeftBar.gameObject.SetActive(true);
            animalsLeftText.gameObject.SetActive(true);

            animalsLeftBar.fillAmount = TotalBar;
        }

        internal void BossHungerSetUp(AnimalHunger animalHunger)
        {
            Debug.Log("BossHungerSetUp");
            animalsLeftText.text = Constants.BOSS_BAR_TEXT;
            StartCoroutine(FillBossHungerBar());
            animalHunger.OnBossFeedEvent += SetBossHungerBar;
        }
        IEnumerator FillBossHungerBar()
        {
            Debug.Log("FillBossHungerBar");
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
            Debug.Log("SetBossHungerBar");
            animalsLeftBar.fillAmount = 1 * progress;
            
        }
    }
}
