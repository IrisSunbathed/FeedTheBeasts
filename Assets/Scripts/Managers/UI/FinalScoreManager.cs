using System.Collections;
using NUnit.Framework;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace FeedTheBeasts.Scripts
{
    [RequireComponent(typeof(AudioSource))]
    public class FinalScoreManager : MonoBehaviour
    {
        [Header("Final Score UI references")]
        [SerializeField] TMP_Text txtFinalScoreInt;
        [SerializeField] TMP_Text txtTotalFedAnimals;
        [SerializeField] TMP_Text txtEscapedAnimals;
        [SerializeField] Button bttEnd;
        [Header("Other references")]
        [SerializeField] Player player;
        LevelManager levelManager;
        AudioSource audioSource;
        GameCatalog gameCatalog;


        void Start()
        {
            levelManager = LevelManager.Instance;
            gameCatalog = GameCatalog.Instance;


        }

        void Awake()
        {
            Assert.IsNotNull(txtFinalScoreInt, "ERROR: txtScore is not added");
            Assert.IsNotNull(player, "ERROR: player is not added");
            audioSource = GetComponent<AudioSource>();
            bttEnd.onClick.AddListener(Exit);

        }

        private void Exit()
        {
            audioSource.PlayOneShot(gameCatalog.GetFXClip(FXTypes.ClickOnButton));
            #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
            #else
            Application.Quit();
            #endif
        }

        internal void Init()
        {
            txtTotalFedAnimals.text = $"Fed Animals: {levelManager.currentFedAnimals}";
            txtEscapedAnimals.text = $"Escaped Animals: {levelManager.EscapedAnimals}";
            txtFinalScoreInt.text = $"Final Score: {player.Score}";

            TMP_Text[] texts = new TMP_Text[]

            {
                txtTotalFedAnimals,
                txtEscapedAnimals,
                txtFinalScoreInt
            };

            foreach (var item in texts)
            {
                StartCoroutine(TextEffectCourutine(item));
            }


        }


        IEnumerator TextEffectCourutine(TMP_Text text)
        {
            Color originalColor = text.color; //temp
            float temp_a = originalColor.a; //okay
            Color temp = text.color;
            while (text.color.a <= 1f)
            {
                temp_a += 0.007f;
                temp.a = temp_a;
                text.color = temp;
                yield return null;
            }
            yield return new WaitForSeconds(2f);
            StartCoroutine(FadeInButton());
        }

        IEnumerator FadeInButton()
        {
            bttEnd.gameObject.SetActive(true);
            Image image = bttEnd.GetComponent<Image>();
            Color originalColor = image.color; //temp
            float temp_a = originalColor.a; //okay
            Color temp = image.color;
            while (image.color.a <= 1f)
            {
                temp_a += 0.0035f;
                temp.a = temp_a;
                image.color = temp;
                yield return null;
            }
            temp_a = 1f;
            temp.a = temp_a;
            image.color = temp;
            yield return null;
        }
    }

}