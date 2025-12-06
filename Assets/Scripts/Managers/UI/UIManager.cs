using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace FeedTheBeasts.Scripts
{

    public class UIManager : MonoBehaviour
    {
        [Header("Lifes and Points UI references")]
        [SerializeField] LivesAndPointsUIManager livesAndPointsUIManager;

        [Header("Start/Game Over Menu references")]
        [SerializeField] MenuUI menuUI;
        [SerializeField] Canvas canvas;
        CamerasManager camerasManager;
        [Header("Food Selector references")]
        [SerializeField] Image[] imagesFoodSelector;
        [SerializeField] Sprite selectedItemImage;
        [SerializeField] Sprite unselectedItemImage;
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

        [SerializeField] MusicManager musicManager;


        void Start()
        {
            camerasManager = CamerasManager.Instance;
           
        }
        void Awake()
        {
            #region ASSERTIONS
            Assert.IsTrue(imagesFoodSelector.Length > 0, "ERROR: image food selector is empty on UIManager");
            Assert.IsNotNull(selectedItemImage, "ERROR: SelectedItemImage is empty on UIManager");
            Assert.IsNotNull(unselectedItemImage, "ERROR: UnselectedItemImage is empty on UIManager");
            Assert.IsNotNull(livesAndPointsUIManager, "ERROR: livesAndPointsUIManager is empty on UIManager");
            Assert.IsNotNull(menuUI, "ERROR: Menu UI is empty on UIManager");
            Assert.IsNotNull(animalsLeftUIManager, "ERROR: animalsLeftUIManager is empty on UIManager");
            Assert.IsNotNull(musicManager, "ERROR: musicManager is empty on UIManager");
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
            InventorySelect(1);
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
            livesAndPointsUIManager.ManageLives(lives);
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

        private void InventorySelect(int result)
        {
            for (int i = 0; i < imagesFoodSelector.Length; i++)
            {
                if (i == result - 1)
                {
                    imagesFoodSelector[i].sprite = selectedItemImage;
                    OnSelectedItemInventoryEvent?.Invoke(i);
                }
                else
                {
                    imagesFoodSelector[i].sprite = unselectedItemImage;
                }
            }
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

            livesAndPointsUIManager.ActivateElementsOnMenu(isActive);

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
            //menuUI.Win();
            //canvas.renderMode = RenderMode.ScreenSpaceOverlay;
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


        }
    }
}
