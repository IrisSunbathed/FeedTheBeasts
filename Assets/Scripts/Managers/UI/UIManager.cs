using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TMPro;
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


        void Start()
        {
            camerasManager = CamerasManager.Instance;
            camerasManager.SwitchCameras(isGameplayCamera: false);
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
            Assert.IsTrue(imgRechargeBar.Length > 0, "ERROR: rechargeBar is empty on UIManager");
            #endregion
         
            CurrentProjectile = 0;
            foreach (var item in imgRechargeBar)
            {
                item.fillAmount = 0;
            }

            menuUI.StartGameEvent += StartGame;

            Init();
        }
        private void Init()
        {

            ActivateElementsOnMenu(isActive: false);
            InventorySelect(1);
           
            menuUI.Init();
            animalsLeftUIManager.Init();

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
        }

        private void ActivateElementsOnMenu(bool isActive)
        {
            foreach (var sprite in imagesFoodSelector)
            {
                sprite.gameObject.SetActive(isActive);
            }

           livesAndPointsUIManager.ActivateElementsOnMenu( isActive);

            foreach (var item in imgRechargeBar)
            {
                item.gameObject.SetActive(isActive);
            }

        }

        private void StartGame()
        {
            ActivateElementsOnMenu(true);
            camerasManager.SwitchCameras(isGameplayCamera: true);
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            animalsLeftUIManager.StartGame();

            RestartGameEvent?.Invoke();
        }

    
        internal void RechargeBar(float rechargeTime)
        {
            StartCoroutine(RechargeCoroutine(rechargeTime));
        }
        IEnumerator RechargeCoroutine(float rechargeTime)
        {
            float time = 0f;
            int currentReloadProjectile = CurrentProjectile;

            while (time <= rechargeTime)
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
            menuUI.Win();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            camerasManager.SwitchCameras(isGameplayCamera: false);
        }
    }
}
