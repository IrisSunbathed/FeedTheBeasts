using NUnit.Framework;
using UnityEngine;

namespace FeedTheBeasts.Scripts
{

    public class PauseManager : MonoBehaviour
    {
        static PauseManager instance;

        static public PauseManager Instance => instance;

        [Header("References")]
        [SerializeField] MenuUI menuUI;
        internal bool isPaused;

        float previousTimeScale;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            GameStage.gameStageEnum = GameStageEnum.NotPausable;

            Assert.IsNotNull(menuUI, "ERROR: MenuUI not added");

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) & GameStage.gameStageEnum != GameStageEnum.NotPausable)
            {
                if (GameStage.gameStageEnum == GameStageEnum.NotPaused)
                {

                    PauseGame();
                    Debug.Log($"GameStage: {GameStage.gameStageEnum}");
                    return;
                }
                if (GameStage.gameStageEnum == GameStageEnum.Paused)
                {
                    UnpauseGame();
                    Debug.Log($"GameStage: {GameStage.gameStageEnum}");
                    return;
                }
                if (GameStage.gameStageEnum == GameStageEnum.Credits)
                {
                    menuUI.Return();
                    GameStage.gameStageEnum = GameStageEnum.Paused;
                }

            }

        }

        private void UnpauseGame()
        {
            menuUI.SetActiveUIElements(false, Constants.PAUSED_TEXT, Constants.UNPAUSE_BUTTON_TEXT);
            menuUI.UnPauseGame -= UnpauseGame;
            Time.timeScale = previousTimeScale;
            GameStage.gameStageEnum = GameStageEnum.NotPaused;
        }

        private void PauseGame()
        {
            menuUI.SetActiveUIElements(true, Constants.PAUSED_TEXT, Constants.UNPAUSE_BUTTON_TEXT);
            menuUI.UnPauseGame += UnpauseGame;
            GameStage.gameStageEnum = GameStageEnum.Paused;
            previousTimeScale = Time.timeScale;
            Time.timeScale = 0;
            Debug.Log($"Game is paused. TimeScale = {Time.timeScale}");
        }
    }

}
