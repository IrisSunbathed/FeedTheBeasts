using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NUnit.Framework;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using RangeAttribute = UnityEngine.RangeAttribute;


namespace FeedTheBeasts.Scripts
{

    public class UIManager : MonoBehaviour
    {


        [Header("Menu references")]
        [SerializeField] MenuUI menuUI;
        [SerializeField] Canvas canvas;
        CamerasManager camerasManager;


        [Header("In Game Notifictations", order = 1)]
        [Header("In Game Notifictations Configuration", order = 2)]
        [SerializeField] Color warningColor;
        [SerializeField] Color successColor;
        [SerializeField] Color deafaultColor;

        [Header("In Game Notifictations References", order = 2)]

        [SerializeField] TMP_Text txtInGameNotification;
        Coroutine stampedeCoroutine;


        [Header("UI references")]

        [SerializeField] ScoreUIManager scoreUIManager;
        [SerializeField] LivesUIManager livesUIManager;

        [SerializeField] AnimalsLeftUIManager animalsLeftUIManager;
        [SerializeField] InventoryUIManager inventoryUIManager;
        [Header("Other references")]

        [SerializeField] MusicManager musicManager;


        public event Action RestartGameEvent;
        void Start()
        {
            camerasManager = CamerasManager.Instance;

        }
        void Awake()
        {
            #region ASSERTIONS
            Assert.IsNotNull(scoreUIManager, "ERROR: livesAndPointsUIManager is empty on UIManager");
            Assert.IsNotNull(menuUI, "ERROR: Menu UI is empty on UIManager");
            Assert.IsNotNull(animalsLeftUIManager, "ERROR: animalsLeftUIManager is empty on UIManager");
            Assert.IsNotNull(musicManager, "ERROR: musicManager is empty on UIManager");
            Assert.IsNotNull(inventoryUIManager, "ERROR: inventoryUIManager is empty on UIManager");
            Assert.IsNotNull(livesUIManager, "ERROR: livesUIManager is empty on UIManager");
            #endregion
            menuUI.StartGameEvent += StartGame;
        }
        internal void Init()
        {
            camerasManager.SwitchCameras(isGameplayCamera: false);

            StopAllCoroutines();
            ActivateElementsOnMenu(isActive: false);
            inventoryUIManager.Init();
            StopAllCoroutines();
            menuUI.Init();
            animalsLeftUIManager.Init();
            txtInGameNotification.text = string.Empty;
            musicManager.PlayMusic(MusicThemes.MainMenu);
            musicManager.FadeCurrentMusic(1f, 1f);

        }

        private void StartGame()
        {
            ActivateElementsOnMenu(isActive: true);
            camerasManager.SwitchCameras(isGameplayCamera: true);
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            animalsLeftUIManager.StartGame();
            scoreUIManager.StartGame();
            RestartGameEvent?.Invoke();

            StopAllCoroutines();
            inventoryUIManager.StartGame();
            txtInGameNotification.text = string.Empty;
        }
        internal void ManageLives(int lives)
        {

            livesUIManager.ManageLives(lives);
        }

        internal void GameOver()
        {
            ActivateElementsOnMenu(false);
            menuUI.GameOver();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            camerasManager.SwitchCameras(isGameplayCamera: false);
            inventoryUIManager.StopAllCoroutines();
            scoreUIManager.GameOver();
            StopWarningEffect(txtInGameNotification);
        }

        internal void ActivateElementsOnMenu(bool isActive)
        {
            inventoryUIManager.ActivateElementsOnMenu(isActive);
            scoreUIManager.ActivateElementsOnMenu(isActive);
            livesUIManager.ActivateElementsOnMenu(isActive);

        }

        internal void Win()
        {
            ActivateElementsOnMenu(false);
            StopWarningEffect(txtInGameNotification);
        }

        internal void InGameNotification(float warningTime, string textWarning, bool doesBlink = false, NotificationType notificationType = NotificationType.Default)
        {
            StartCoroutine(InGameWarningWarningCoroutine(warningTime, textWarning, doesBlink, notificationType));
        }

        IEnumerator InGameWarningWarningCoroutine(float textTime, string textWarning, bool doesBlink = false, NotificationType notificationType = NotificationType.Default)
        {
            switch (notificationType)
            {
                case NotificationType.Warnining:
                    txtInGameNotification.color = warningColor;
                    break;
                case NotificationType.Success:
                    txtInGameNotification.color = successColor;
                    break;
                case NotificationType.Default:
                    txtInGameNotification.color = deafaultColor;
                    break;
            }
            txtInGameNotification.text = textWarning;
            stampedeCoroutine ??= StartCoroutine(TextBlinkEffect(txtInGameNotification, doesBlink));
            yield return new WaitForSeconds(textTime);
            StopWarningEffect(txtInGameNotification);
        }

        private void StopWarningEffect(TMP_Text txtToEffect)
        {
            if (stampedeCoroutine != null)
            {
                StopCoroutine(stampedeCoroutine);
                stampedeCoroutine = null;
            }
            if (txtToEffect.color.a != 0)
            {
                Color endColor = new Color(txtToEffect.color.r, txtToEffect.color.g, txtToEffect.color.b, 0);
                txtToEffect.DOColor(endColor, 0.5f);
            }

        }

        IEnumerator TextBlinkEffect(TMP_Text txtToEffect, bool doesBlink)
        {
            Color originalColor = txtToEffect.color; //temp
            float temp_a = originalColor.a; //okay
            Color temp = txtToEffect.color;

            while (txtToEffect.color.a <= 1f)
            {
                temp_a += 0.007f;
                temp.a = temp_a;
                txtToEffect.color = temp;
                yield return null;
            }
            temp_a = 1f;
            temp.a = temp_a;
            txtToEffect.color = temp;
            if (doesBlink)
            {
                while (txtToEffect.color.a >= 0f)
                {
                    temp_a -= 0.007f;
                    temp.a = temp_a;
                    txtToEffect.color = temp;
                    yield return null;
                }
                temp_a = 0f;
                temp.a = temp_a;
                txtToEffect.color = temp;
                stampedeCoroutine = StartCoroutine(TextBlinkEffect(txtToEffect, doesBlink));
            }
        }



        internal bool CheckPointsCalc()
        {
            return scoreUIManager.IsScoreCalc;
        }
    }
}

