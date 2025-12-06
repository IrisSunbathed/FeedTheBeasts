using System;
using System.Collections;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FeedTheBeasts.Scripts
{

    public class OutroController : MonoBehaviour
    {
        [SerializeField] TMP_Text[] txtOutro;

        [SerializeField] GameObject background;
        [SerializeField] float timeBetweenTexts;
        [SerializeField] FinalScoreManager finalScoreManager;
        [SerializeField] UIManager uIManager;

        CamerasManager camerasManager;
        int index;
        bool canBeClickedAway;



        void Start()
        {
            camerasManager = CamerasManager.Instance;
            camerasManager.SwitchCameras(isGameplayCamera: true);
        }
        void Awake()
        {
            Assert.IsNotNull(finalScoreManager, "ERROR: musicManager is not added");
            Assert.IsTrue(txtOutro.Length > 0, "ERROR: txtOutro is empty");

            index = 0;
            Init();
        }

        private void Init()
        {


            foreach (var item in txtOutro)
            {

                Color temp = item.color;
                temp.a = 0f;
                item.color = temp;
                item.gameObject.SetActive(true);
            }

        }

        internal void OutroStart()
        {
            txtOutro[0].text = Constants.OUTRO_TEXT_1;
            txtOutro[1].text = Constants.OUTRO_TEXT_2;
            txtOutro[2].text = Constants.OUTRO_TEXT_3;
            txtOutro[3].text = Constants.OUTRO_TEXT_4;
            background.SetActive(true);
            foreach (var item in txtOutro)
            {
                item.gameObject.SetActive(true);
            }
            uIManager.ActivateElementsOnMenu(false);

            StartCoroutine(FadeInBackground());
        }

        IEnumerator FadeInBackground()
        {
            Image backgroundimg = background.GetComponent<Image>();
            Color originalColor = backgroundimg.color; //temp
            float temp_a = originalColor.a; //okay
            Color temp = backgroundimg.color;
            while (backgroundimg.color.a <= .6f)
            {
                temp_a += 0.002f;
                temp.a = temp_a;
                backgroundimg.color = temp;
                yield return null;
            }
            temp_a = 1f;
            temp.a = temp_a;
            yield return null;
            StartCoroutine(TextEffectCourutine());
        }
        IEnumerator TextEffectCourutine()
        {
            Color originalColor = txtOutro[index].color; //temp
            float temp_a = originalColor.a; //okay
            Color temp = txtOutro[index].color;
            while (txtOutro[index].color.a <= 1f)
            {
                temp_a += 0.007f;
                temp.a = temp_a;
                txtOutro[index].color = temp;
                yield return null;
            }
            temp_a = 1f;
            temp.a = temp_a;
            txtOutro[index].color = temp;

            yield return new WaitForSeconds(timeBetweenTexts);
            if (index < txtOutro.Length - 1)
            {
                index++;
                Debug.Log($"OutroController index: {index}");
                StartCoroutine(TextEffectCourutine());
            }
            else
            {
                StartCoroutine(FadeInAndOut());
                canBeClickedAway = true;
            }
        }

        IEnumerator FadeOutTexts(int index)
        {
            Color originalColor = txtOutro[index].color; //temp
            float temp_a = originalColor.a; //okay
            Color temp = txtOutro[index].color;
            while (txtOutro[index].color.a >= 0f)
            {
                temp_a -= 0.007f;
                temp.a = temp_a;
                txtOutro[index].color = temp;
                yield return null;
            }
            temp_a = 1f;
            temp.a = temp_a;
            yield return null;

        }

        internal void FadeOutBackgroud()
        {
            StartCoroutine(FadeOutBackgroundCoroutine());
        }
        IEnumerator FadeOutBackgroundCoroutine()
        {
            Image backgroundimg = background.GetComponent<Image>();
            Color originalColor = backgroundimg.color; //temp
            float temp_a = originalColor.a; //okay
            Color temp = backgroundimg.color;
            while (backgroundimg.color.a >= 0f)
            {
                temp_a -= 0.007f;
                temp.a = temp_a;
                backgroundimg.color = temp;
                yield return null;
            }
            temp_a = 1f;
            temp.a = temp_a;
            yield return null;
        }

        IEnumerator FadeInAndOut()
        {
            Color originalColor = txtOutro[^1].color; //temp
            float temp_a = originalColor.a; //okay
            Color temp = txtOutro[^1].color;
            while (txtOutro[^1].color.a >= 0f)
            {
                temp_a -= 0.007f;
                temp.a = temp_a;
                txtOutro[^1].color = temp;
                yield return null;
            }
            temp_a = 0f;
            temp.a = temp_a;
            txtOutro[^1].color = temp;
            yield return new WaitForSeconds(1f);
            while (txtOutro[^1].color.a <= 1f)
            {
                temp_a += 0.007f;
                temp.a = temp_a;
                txtOutro[^1].color = temp;
                yield return null;
            }
            temp_a = 1f;
            temp.a = temp_a;
            txtOutro[^1].color = temp;
            yield return new WaitForSeconds(1f);
            StartCoroutine(FadeInAndOut());


        }


        void Update()
        {
            if (canBeClickedAway && Input.GetMouseButtonDown(0))
            {
                canBeClickedAway = false;
                StartCoroutine(FinalScoreTransition());

            }
        }

        IEnumerator FinalScoreTransition()
        {
            index = 0;
            for (int i = 0; i < txtOutro.Length; i++)
            {
                StartCoroutine(FadeOutTexts(i));
            }
            //StartCoroutine(FadeOutBackground());
            yield return new WaitForSeconds(3f);
            foreach (var item in txtOutro)
            {
                item.gameObject.SetActive(false);
            }
            canBeClickedAway = true;
            finalScoreManager.Init();
            StopAllCoroutines();
        }


    }

}