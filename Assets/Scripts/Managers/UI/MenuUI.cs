using System;
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
        [SerializeField] GameObject scrollArea;
        [SerializeField] Button bttReturn;
        [SerializeField] Scrollbar scrollbar;
        AudioSource audioSource;
        GameCatalog gameCatalog;

        public event Action StartGameEvent;

        void Start()
        {
            gameCatalog = GameCatalog.Instance;
        }
        void Awake()
        {
            Assert.IsNotNull(txtHeader, "ERROR: txtGameOver is empty on UIManager");
            Assert.IsNotNull(bttStartOver, "ERROR: bttStart is empty on UIManager");
            Assert.IsNotNull(bttExitGame, "ERROR: bttExit is empty on UIManager");
            Assert.IsNotNull(scrollArea, "ERROR: scrollArea is empty on UIManager");
            Assert.IsNotNull(bttReturn, "ERROR: bttReturn is empty on UIManager");
            Assert.IsNotNull(scrollbar, "ERROR: scrollbar is empty on UIManager");

            audioSource = GetComponent<AudioSource>();

        }

        internal void Init()
        {
            txtHeader.text = Constants.GAME_TITLE;
            //xtHeader.color = new Color(0.04405483f, 0.8490566f, 0.6523646f);
            TMP_Text tMP_Text = bttStartOver.GetComponentInChildren<TMP_Text>();
            tMP_Text.text = Constants.START_BUTTON_TEXT;
            SetActiveUIElements(true);
            SetCreditsUIElements(false);
            bttStartOver.onClick.AddListener(StartGame);
            bttCredits.onClick.AddListener(ShowCredits);
            bttExitGame.onClick.AddListener(Exit);
            bttReturn.onClick.AddListener(Return);

        }


        private void SetCreditsUIElements(bool isActive)
        {
            scrollArea.SetActive(isActive);
            bttReturn.gameObject.SetActive(isActive);
            scrollbar.gameObject.SetActive(isActive);

        }

        private void Return()
        {
            SetActiveUIElements(true);
            SetCreditsUIElements(false);
        }

        private void ShowCredits()
        {
            SetActiveUIElements(false);
            SetCreditsUIElements(true);
            scrollbar.value = 1f;

        }

        internal void SetActiveUIElements(bool isActive)
        {
            bttStartOver.gameObject.SetActive(isActive);
            bttCredits.gameObject.SetActive(isActive);
            bttExitGame.gameObject.SetActive(isActive);
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
            StartGameEvent?.Invoke();
        }


        internal void GameOver()
        {
            SetActiveUIElements(true);
            txtHeader.text = Constants.GAMEOVER_TEXT;
            TMP_Text tMP_Text = bttStartOver.GetComponentInChildren<TMP_Text>();
            tMP_Text.text = Constants.GAMEOVER_BUTTON_TEXT;
            bttStartOver.onClick.AddListener(StartGame);
        }

        internal void Win()
        {
            SetActiveUIElements(true);
            txtHeader.text = Constants.VICTORY_TEXT;
            TMP_Text tMP_Text = bttStartOver.GetComponentInChildren<TMP_Text>();
            tMP_Text.text = Constants.VICTORY_BUTTON_TEXT;
            bttStartOver.onClick.AddListener(StartGame);
        }
    }

}