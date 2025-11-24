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
        [SerializeField] TMP_Text txtPoints;
        [SerializeField] TMP_Text txtPointsStr;
        [SerializeField] GameObject goLives;
        [SerializeField] RectTransform lifeContainer;
        List<GameObject> lifeList;
        int previousNumberOfLifes;
        [Header("Start/Game Over Menu references")]
        [SerializeField] MenuUI menuUI;
        CamerasManager camerasManager;
        [Header("Food Selector references")]
        [SerializeField] Image[] imagesFoodSelector;
        [SerializeField] Sprite selectedItemImage;
        [SerializeField] Sprite unselectedItemImage;
        string lastKeyPressed;
        [Header("Recharge configuration")]
        [SerializeField] Image[] imgRechargeBar;
        internal int CurrentProjectile { get; set; }
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
            Assert.IsNotNull(txtPoints, "ERROR: txtPointsStr is empty on UIManager");
            Assert.IsNotNull(txtPointsStr, "ERROR: txtPoints is empty on UIManager");
            Assert.IsTrue(imagesFoodSelector.Length > 0, "ERROR: image food selector is empty on UIManager");
            Assert.IsNotNull(selectedItemImage, "ERROR: SelectedItemImage is empty on UIManager");
            Assert.IsNotNull(unselectedItemImage, "ERROR: UnselectedItemImage is empty on UIManager");
            Assert.IsNotNull(goLives, "ERROR: lifes game object is empty on UIManager");
            Assert.IsNotNull(lifeContainer, "ERROR: life container is empty on UIManager");
            Assert.IsNotNull(menuUI, "ERROR: Menu UI is empty on UIManager");
            Assert.IsTrue(imgRechargeBar.Length > 0, "ERROR: rechargeBar is empty on UIManager");
            #endregion
            lifeList = new List<GameObject>();
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
           // camerasManager.SetUpMenuCamera();
            menuUI.Init();
            
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
            return Input.GetKeyDown(KeyCode.Alpha1) | Input.GetKeyDown(KeyCode.Alpha2);
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


        internal void ManageLives(int lives)
        {
            //  if (lives >= 0)
            if (previousNumberOfLifes > lives)
            {

                int index = lives <= 0 ? 0 : lives - 1;
                Debug.Log(index);
                GameObject life = lifeList[index];
                Destroy(life);
            }
            else
            {
                for (int i = previousNumberOfLifes; i < lives; i++)
                {
                    GameObject newLife = Instantiate(goLives, lifeContainer);
                    lifeList.Add(newLife);
                }

            }
            previousNumberOfLifes = lives;
        }

        internal void GameOver()
        {
            ActivateElementsOnMenu(false);
            menuUI.GameOver();
            camerasManager.SwitchCameras(isGameplayCamera: false);
        }

        private void ActivateElementsOnMenu(bool isActive)
        {
            foreach (var sprite in imagesFoodSelector)
            {
                sprite.gameObject.SetActive(isActive);
            }
            txtPoints.gameObject.SetActive(isActive);
            txtPointsStr.gameObject.SetActive(isActive);
        }

        private void StartGame()
        {
            ActivateElementsOnMenu(true);
            camerasManager.SwitchCameras(isGameplayCamera: true);
            RestartGameEvent?.Invoke();
        }

        internal void ManagePoints(int points)
        {
            txtPoints.text = points.ToString();
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

    }
}
