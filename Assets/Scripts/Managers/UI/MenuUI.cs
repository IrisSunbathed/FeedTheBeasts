using System;
using System.Collections;
using TMPro;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace FeedTheBeasts.Scripts
{
    [RequireComponent(typeof(AudioSource))]
    public class MenuUI : MonoBehaviour
    {
        [Header("Start/Game Over Menu references")]
        [SerializeField] TMP_Text txtHeader;
        [SerializeField] Button bttStartOver;
        [SerializeField] Button bttCredits;
        [SerializeField] Button bttExitGame;
        [Header("Credits references")]
        [SerializeField] GameObject scrollArea;
        [SerializeField] Button bttReturn;
        [SerializeField] Scrollbar scrollbar;

        AudioSource audioSource;
        GameCatalog gameCatalog;

        string lastButtonText;
        Button[] buttons;

        public event Action StartGameEvent;
        public event Action UnPauseGame;

        void Start()
        {
            gameCatalog = GameCatalog.Instance;
        }
        void Awake()
        {
            Assert.IsNotNull(txtHeader, "ERROR: txtGameOver is empty on UIManager");
            Assert.IsNotNull(bttStartOver, "ERROR: bttStart is empty on UIManager");
            Assert.IsNotNull(bttCredits, "ERROR: bttCredits is empty on UIManager");
            Assert.IsNotNull(bttExitGame, "ERROR: bttExit is empty on UIManager");
            Assert.IsNotNull(scrollArea, "ERROR: scrollArea is empty on UIManager");
            Assert.IsNotNull(bttReturn, "ERROR: bttReturn is empty on UIManager");
            Assert.IsNotNull(scrollbar, "ERROR: scrollbar is empty on UIManager");

            audioSource = GetComponent<AudioSource>();

            buttons = new Button[3]
            {
                bttStartOver,
                bttExitGame,
                bttCredits
            };
        }

        internal void Init()
        {

            SetElementsInvisible(buttons);
            SetActiveUIElements(true, Constants.GAME_TITLE, Constants.START_BUTTON_TEXT);
            FadeInUIElements(buttons);
            SetCreditsUIElements(false);
            bttStartOver.onClick.AddListener(StartGame);
            bttCredits.onClick.AddListener(ShowCredits);
            bttExitGame.onClick.AddListener(Exit);
            bttReturn.onClick.AddListener(Return);


        }

        private void FadeInUIElements(Button[] buttons)
        {
            StartCoroutine(FadeInTitle());
            foreach (var item in buttons)
            {
                StartCoroutine(FadeInButtons(item));
            }
        }

        private void SetElementsInvisible(Button[] buttons)
        {
            //Title text
            float txttemp_a = 0f;
            Color txttemp = txtHeader.color;
            txttemp.a = txttemp_a;
            txtHeader.color = txttemp;
            //Buttons
            foreach (var item in buttons)
            {
                Image image = item.GetComponent<Image>();
                float temp_a = 0f;
                Color temp = image.color;
                temp.a = temp_a;
                image.color = temp;
            }
        }

        IEnumerator FadeInTitle()
        {
            Color originalColor = txtHeader.color; //temp
            float temp_a = originalColor.a; //okay
            Color temp = txtHeader.color;
            while (txtHeader.color.a <= 1f)
            {
                temp_a += 0.007f;
                temp.a = temp_a;
                txtHeader.color = temp;
                yield return null;
            }
            temp_a = 1f;
            temp.a = temp_a;
            txtHeader.color = temp;
            yield return null;
        }

        IEnumerator FadeInButtons(Button button)
        {
            Image image = button.GetComponent<Image>();
            Color originalColor = image.color; //temp
            float temp_a = originalColor.a; //okay
            Color temp = image.color;
            while (image.color.a <= 1f)
            {
                temp_a += 0.007f;
                temp.a = temp_a;
                image.color = temp;
                yield return null;
            }
            temp_a = 1f;
            temp.a = temp_a;
            image.color = temp;
            yield return null;
        }


        private void SetCreditsUIElements(bool isActive)
        {
            scrollArea.SetActive(isActive);
            bttReturn.gameObject.SetActive(isActive);
            scrollbar.gameObject.SetActive(isActive);

        }

        internal void Return()
        {
            if (GameStage.gameStageEnum == GameStageEnum.Credits)
            {
                GameStage.gameStageEnum = GameStageEnum.Paused;
            }
            audioSource.PlayOneShot(gameCatalog.GetFXClip(FXTypes.ClickOnButton));
            SetActiveUIElements(true);
            SetCreditsUIElements(false);
        }

        private void ShowCredits()
        {
            if (GameStage.gameStageEnum == GameStageEnum.Paused)
            {
                GameStage.gameStageEnum = GameStageEnum.Credits;

            }
            audioSource.PlayOneShot(gameCatalog.GetFXClip(FXTypes.ClickOnButton));
            SetActiveUIElements(false);
            SetCreditsUIElements(true);
            scrollbar.value = 1f;

        }

        internal void SetActiveUIElements(bool isActive, string headerText = null, string buttonText = null)
        {
            if (isActive)
            {
                if (buttonText != null)
                {

                    TMP_Text tMP_Text = bttStartOver.GetComponentInChildren<TMP_Text>();
                    tMP_Text.text = buttonText;
                    lastButtonText = tMP_Text.text;
                }
                if (headerText != null)
                {
                    txtHeader.text = headerText;
                }
            }

            foreach (var item in buttons)
            {
                item.gameObject.SetActive(isActive);
            }

            txtHeader.enabled = isActive;
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

        internal void StartGame()
        {
            audioSource.PlayOneShot(gameCatalog.GetFXClip(FXTypes.ClickOnButton));
            SetActiveUIElements(false);
            if (GameStage.gameStageEnum == GameStageEnum.Paused)
            {
                UnPauseGame?.Invoke();
            }
            else
            {
                StartGameEvent?.Invoke();
            }
        }


        internal void GameOver()
        {
            SetCreditsUIElements(false);
            SetActiveUIElements(true, Constants.GAMEOVER_TEXT, Constants.GAMEOVER_BUTTON_TEXT);
            bttStartOver.onClick.AddListener(StartGame);
        }

        internal void Win()
        {
            SetCreditsUIElements(false);
            SetActiveUIElements(true, Constants.VICTORY_TEXT, Constants.VICTORY_BUTTON_TEXT);
            bttStartOver.onClick.AddListener(StartGame);
        }
    }

}