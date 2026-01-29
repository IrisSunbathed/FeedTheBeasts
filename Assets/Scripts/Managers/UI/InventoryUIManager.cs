using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace FeedTheBeasts.Scripts
{


    public class InventoryUIManager : MonoBehaviour
    {
        [Header("Food Selector configuration")]
        [SerializeField] Image[] imgFoodContainer;
        [SerializeField, Range(45f, 180f)] float inventoryMaxRotation;
        [SerializeField, Range(0.001f, 0.25f)] float timeTweenRotation;

        [SerializeField] Image[] imgRechargeBar;

        [SerializeField, Range(2, 4)] internal int initialNumberWeapons;
        [SerializeField] RectTransform[] foodLayoutUnlock;
        internal int CurrentProjectile { get; set; }
        int selectedIndex;
        string lastKeyPressed;

        List<Image> lstActiveInventoryItems;


        [Header("References")]
        [SerializeField] FoodSelectorManager foodSelectorManager;
        [SerializeField] FXSoundsManager fXSoundsManager;
        int boneInd;
        int basketInd;









        public event Action<int, float, float> OnSelectedItemInventoryEvent;
        public event Action<int> OnRechargeCompleteEvent;


        void Awake()
        {
            Assert.IsTrue(imgFoodContainer.Length > 0, "ERROR: image food selector is empty on UIManager");
            Assert.IsTrue(imgRechargeBar.Length > 0, "ERROR: rechargeBar is empty on UIManager");
            Assert.IsNotNull(fXSoundsManager, "ERROR: fXSoundsManager is empty on UIManager");
            lstActiveInventoryItems = new List<Image>(4);
            GetLockedIndexes();

        }

        private void GetLockedIndexes()
        {
            for (int i = 0; i < imgFoodContainer.Length; i++)
            {
                if (imgFoodContainer[i].gameObject.CompareTag(Constants.THROWABLE_UI_TAG))
                {
                    boneInd = i;
                }
                if (imgFoodContainer[i].gameObject.CompareTag(Constants.PLANTABLE_UI_TAG))
                {
                    basketInd = i;
                }
            }
        }

        internal void Init()
        {
            CurrentProjectile = 0;
            selectedIndex = -1;

            foreach (var item in imgRechargeBar)
            {
                item.fillAmount = 0;
            }

        }

        internal void ActivateElementsOnMenu(bool isActive)
        {
            if (lstActiveInventoryItems.Count > initialNumberWeapons & !isActive)
            {
                for (int i = lstActiveInventoryItems.Count; i >= 0; i--)
                {
                    imgFoodContainer[i].gameObject.SetActive(isActive);
                    lstActiveInventoryItems.Remove(imgFoodContainer[i]);
                }
            }
            else
            {
                for (int i = 0; i < initialNumberWeapons; i++)
                {
                    imgFoodContainer[i].gameObject.SetActive(isActive);
                    if (lstActiveInventoryItems.Count < initialNumberWeapons)
                    {
                        lstActiveInventoryItems.Add(imgFoodContainer[i]);
                    }
                }
            }



            foreach (var item in imgRechargeBar)
            {
                item.gameObject.SetActive(isActive);
            }


        }

        void Update()
        {

            if (IsValidKey())
            {
                lastKeyPressed = Input.inputString;
                if (int.TryParse(lastKeyPressed, out int result))
                {
                    if (lstActiveInventoryItems.Count >= result)
                    {
                        InventorySelect(result);
                    }
                }
            }
            if (Input.GetKey(KeyCode.P))
            {
                UnlockBasket();
            }

        }

        private bool IsValidKey()
        {
            //Debug.Log(lstActiveInventoryItems.Count);
            return lstActiveInventoryItems.Count switch
            {
                2 => Input.GetKeyDown(KeyCode.Alpha1) |
                                   Input.GetKeyDown(KeyCode.Alpha2),
                3 => Input.GetKeyDown(KeyCode.Alpha1) |
                                Input.GetKeyDown(KeyCode.Alpha2) |
                                Input.GetKeyDown(KeyCode.Alpha3),
                4 => Input.GetKeyDown(KeyCode.Alpha1) |
                                Input.GetKeyDown(KeyCode.Alpha2) |
                                Input.GetKeyDown(KeyCode.Alpha3) |
                                Input.GetKeyDown(KeyCode.Alpha4),
                _ => Input.GetKeyDown(KeyCode.Alpha1) |
                                    Input.GetKeyDown(KeyCode.Alpha2),
            };

        }

        private void InventorySelect(int pressedKey, bool playSound = true)
        {

            int newIndex = pressedKey - 1;
            // Si es el mismo elemento, no hacemos nada
            if (selectedIndex == newIndex)
                return;

            // Desrotar el anterior
            if (selectedIndex != -1)
            {
                RectTransform prev = lstActiveInventoryItems[selectedIndex].GetComponent<RectTransform>();
                prev.DOKill();
                prev.DOLocalRotate(Vector3.zero, timeTweenRotation);
            }

            // Rotar el nuevo
            RectTransform current = lstActiveInventoryItems[newIndex].GetComponent<RectTransform>();
            current.DOKill();
            current.DOLocalRotate(new Vector3(0, 0, inventoryMaxRotation), timeTweenRotation);
            //PlatSound
            if (playSound)
            {
                fXSoundsManager.PlayFX(FXTypes.SelectItem, pitch: 1, volumne: 0.3f);
            }

            selectedIndex = newIndex;
            OnSelectedItemInventoryEvent?.Invoke(newIndex, timeTweenRotation, inventoryMaxRotation);
            CurrentProjectile = newIndex;
        }

        internal void StartGame()
        {
            InventorySelect(1, false);
            foreach (var item in imgRechargeBar)
            {
                item.fillAmount = 0;
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

        internal void UnlockBone()
        {
            Debug.Log(lstActiveInventoryItems.Count);
            foodLayoutUnlock[boneInd].SetSiblingIndex(lstActiveInventoryItems.Count);
            Debug.Log(foodLayoutUnlock[boneInd].GetSiblingIndex());
            imgFoodContainer[boneInd].gameObject.SetActive(true);
            lstActiveInventoryItems.Add(imgFoodContainer[boneInd]);
            foodSelectorManager.UnlockWeapon(boneInd);
        }
        internal void UnlockBasket()
        {
            Debug.Log(lstActiveInventoryItems.Count);
            foodLayoutUnlock[basketInd].SetSiblingIndex(lstActiveInventoryItems.Count);
            Debug.Log(foodLayoutUnlock[boneInd].GetSiblingIndex());
            imgFoodContainer[basketInd].gameObject.SetActive(true);
            lstActiveInventoryItems.Add(imgFoodContainer[basketInd]);
            foodSelectorManager.UnlockWeapon(basketInd);
        }
    }

}