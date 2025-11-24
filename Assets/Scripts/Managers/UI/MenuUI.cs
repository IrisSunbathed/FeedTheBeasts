using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace FeedTheBeasts.Scripts
{
    public class MenuUI : MonoBehaviour
    {
        [Header("Start/Game Over Menu references")]
        [SerializeField] TMP_Text txtHeader;
        [SerializeField] Button bttStartOver;
        [SerializeField] Button bttExitGame;

        public event Action StartGameEvent;
        void Awake()
        {
            Assert.IsNotNull(txtHeader, "ERROR: txtGameOver is empty on UIManager");
            Assert.IsNotNull(bttStartOver, "ERROR: bttStart is empty on UIManager");
            Assert.IsNotNull(bttExitGame, "ERROR: bttExit is empty on UIManager");

        }

        internal void Init()
        {
            txtHeader.text = Constants.GAME_TITLE;
            //xtHeader.color = new Color(0.04405483f, 0.8490566f, 0.6523646f);
            TMP_Text tMP_Text = bttStartOver.GetComponentInChildren<TMP_Text>();
            tMP_Text.text = Constants.START_BUTTON_TEXT;
            SetActiveUIElements(true);
            bttStartOver.onClick.AddListener(StartGame);
            bttExitGame.onClick.AddListener(Exit);

        }

        private void SetActiveUIElements(bool isActive)
        {
            bttStartOver.gameObject.SetActive(isActive);
            bttExitGame.gameObject.SetActive(isActive);
            txtHeader.enabled = isActive;
        }

        private void Exit()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }

        internal void StartGame()
        {

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
    }

}