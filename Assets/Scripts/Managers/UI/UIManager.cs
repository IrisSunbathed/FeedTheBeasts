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
using UnityEngine.UI;
<<<<<<< Updated upstream
=======
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using RangeAttribute = UnityEngine.RangeAttribute;
<<<<<<< Updated upstream
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes

namespace FeedTheBeasts.Scripts
{

    public class UIManager : MonoBehaviour
    {
        [Header("Lifes and Points UI references")]
<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream
        [SerializeField] LivesAndPointsUIManager livesAndPointsUIManager;
=======
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
        [SerializeField] ScoreUIManager scoreUIManager;
        [SerializeField] LivesUIManager livesUIManager;
>>>>>>> Stashed changes

        [Header("Start/Game Over Menu references")]
        [SerializeField] MenuUI menuUI;
        [SerializeField] Canvas canvas;
        CamerasManager camerasManager;
        [Header("Food Selector configuration")]
        [SerializeField] Image[] imagesFoodSelector;
        [SerializeField, Range(45f, 180f)] float inventoryMaxRotation;
        [SerializeField, Range(0.001f, 0.25f)] float timeTweenRotation;
        int selectedIndex;

        string lastKeyPressed;
        [Header("Recharge configuration")]
        [SerializeField] Image[] imgRechargeBar;
        internal int CurrentProjectile { get; set; }

        [Header("AnimalsLeftUI Reference")]

        [SerializeField] AnimalsLeftUIManager animalsLeftUIManager;
        public event Action RestartGameEvent;
        public event Action<int> OnSelectedItemInventoryEvent;
        public event Action<int> OnRechargeCompleteEvent;

        [Header("Stampede elements")]

        [SerializeField] TMP_Text txtInGameNotification;
        Coroutine stampedeCoroutine;

        [Header("Sound Managers references")]

        [SerializeField] MusicManager musicManager;
        [SerializeField] FXSoundsManager fXSoundsManager;


        void Start()
        {
            camerasManager = CamerasManager.Instance;

        }
        void Awake()
        {
            #region ASSERTIONS
            Assert.IsTrue(imagesFoodSelector.Length > 0, "ERROR: image food selector is empty on UIManager");
            // Assert.IsNotNull(selectedItemImage, "ERROR: SelectedItemImage is empty on UIManager");
            // Assert.IsNotNull(unselectedItemImage, "ERROR: UnselectedItemImage is empty on UIManager");
            Assert.IsNotNull(scoreUIManager, "ERROR: livesAndPointsUIManager is empty on UIManager");
            Assert.IsNotNull(menuUI, "ERROR: Menu UI is empty on UIManager");
            Assert.IsNotNull(animalsLeftUIManager, "ERROR: animalsLeftUIManager is empty on UIManager");
            Assert.IsNotNull(musicManager, "ERROR: musicManager is empty on UIManager");
<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream
=======
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
            Assert.IsNotNull(fXSoundsManager, "ERROR: fXSoundsManager is empty on UIManager");
            Assert.IsNotNull(livesUIManager, "ERROR: livesUIManager is empty on UIManager");
>>>>>>> Stashed changes
            Assert.IsTrue(imgRechargeBar.Length > 0, "ERROR: rechargeBar is empty on UIManager");
            #endregion
            menuUI.StartGameEvent += StartGame;
        }
        internal void Init()
        {
                    Debug.Log("UImanager");
            camerasManager.SwitchCameras(isGameplayCamera: false);
            CurrentProjectile = 0;
            foreach (var item in imgRechargeBar)
            {
                item.fillAmount = 0;
            }
            StopAllCoroutines();
            ActivateElementsOnMenu(isActive: false);
            selectedIndex = -1;
            InventorySelect(1, false);
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
            foreach (var item in imgRechargeBar)
            {
                item.fillAmount = 0;
            }
            StopAllCoroutines();
            txtInGameNotification.text = string.Empty;
        }
        internal void ManageLives(int lives)
        {
<<<<<<< Updated upstream
            livesAndPointsUIManager.ManageLives(lives);
=======
            livesUIManager.ManageLives(lives);

<<<<<<< Updated upstream
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
        }

        internal void ManagePoints(int points)
        {
            livesAndPointsUIManager.ManageScore(points);
        }

        void Update()
        {

            if (IsValidKey())
            {
                lastKeyPressed = Input.inputString;
                if (int.TryParse(lastKeyPressed, out int result))
                {
                    if (imagesFoodSelector.Length >= result)
                    {
                        InventorySelect(result);
                    }
                }
            }

        }

        private bool IsValidKey()
        {
            return Input.GetKeyDown(KeyCode.Alpha1) |
            Input.GetKeyDown(KeyCode.Alpha2) |
            Input.GetKeyDown(KeyCode.Alpha3) |
            Input.GetKeyDown(KeyCode.Alpha4);
        }

        private void InventorySelect(int result, bool playSound = true)
        {

            int newIndex = result - 1;
            // Si es el mismo elemento, no hacemos nada
            if (selectedIndex == newIndex)
                return;

            // Desrotar el anterior
            if (selectedIndex != -1)
            {
                RectTransform prev = imagesFoodSelector[selectedIndex].GetComponent<RectTransform>();
                prev.DOKill();
                prev.DOLocalRotate(Vector3.zero, timeTweenRotation);
            }

            // Rotar el nuevo
            RectTransform current = imagesFoodSelector[newIndex].GetComponent<RectTransform>();
            current.DOKill();
            current.DOLocalRotate(new Vector3(0, 0, inventoryMaxRotation), timeTweenRotation);
            if (playSound)
            {
                fXSoundsManager.PlayFX(FXTypes.SelectItem, pitch: 1, volumne: 0.3f);
            }

            selectedIndex = newIndex;
            OnSelectedItemInventoryEvent?.Invoke(newIndex);
        }



        internal void GameOver()
        {
            ActivateElementsOnMenu(false);
            menuUI.GameOver();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            camerasManager.SwitchCameras(isGameplayCamera: false);
            StopWarningEffect();
        }

        internal void ActivateElementsOnMenu(bool isActive)
        {
            foreach (var sprite in imagesFoodSelector)
            {
                sprite.gameObject.SetActive(isActive);
            }

<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream
            livesAndPointsUIManager.ActivateElementsOnMenu(isActive);
=======
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
            scoreUIManager.ActivateElementsOnMenu(isActive);
            livesUIManager.ActivateElementsOnMenu(isActive);
>>>>>>> Stashed changes

            foreach (var item in imgRechargeBar)
            {
                item.gameObject.SetActive(isActive);
            }

        }

        internal void RechargeBar(float rechargeTime)
        {
            StartCoroutine(RechargeCoroutine(rechargeTime));
        }
        IEnumerator RechargeCoroutine(float rechargeTime)
        {
            float time = 0f;
            
            int currentReloadProjectile = CurrentProjectile;

            while (time <= rechargeTime + .1f)
            {
                time += Time.deltaTime;

                float progress = Mathf.Clamp01(time / rechargeTime);
                imgRechargeBar[currentReloadProjectile].fillAmount = 1 * progress;
                yield return null;
            }
            OnRechargeCompleteEvent?.Invoke(currentReloadProjectile);
            imgRechargeBar[currentReloadProjectile].fillAmount = 0;
        }
        internal void Win()
        {
            ActivateElementsOnMenu(false);
            StopWarningEffect();
        }

        internal void InGameWarning(float stampedeTime, string textWarning)
        {
            StartCoroutine(InGameWarningWarningCoroutine(stampedeTime, textWarning));
        }

        IEnumerator InGameWarningWarningCoroutine(float textTime, string textWarning)
        {
            txtInGameNotification.text = textWarning;
            stampedeCoroutine ??= StartCoroutine(TextTransparentEffect(txtInGameNotification));
            yield return new WaitForSeconds(textTime);
            StopWarningEffect();
        }

        private void StopWarningEffect()
        {
            if (stampedeCoroutine != null)
            {
                StopCoroutine(stampedeCoroutine);
                stampedeCoroutine = null;
            }
            txtInGameNotification.text = "";

        }

        IEnumerator TextTransparentEffect(TMP_Text txtToEffect)
        {
            Color originalColor = txtToEffect.color; //temp
            float temp_a = originalColor.a; //okay
            Color temp = txtToEffect.color;
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
            stampedeCoroutine = StartCoroutine(TextTransparentEffect(txtToEffect));


<<<<<<< Updated upstream
=======
        internal bool CheckPointsCalc()
        {
            return scoreUIManager.IsScoreCalc;
<<<<<<< Updated upstream
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
        }
    }
}
