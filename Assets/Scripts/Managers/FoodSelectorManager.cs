using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NUnit.Framework;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using RangeAttribute = UnityEngine.RangeAttribute;
using Vector3 = UnityEngine.Vector3;


namespace FeedTheBeasts.Scripts
{

    public class FoodSelectorManager : MonoBehaviour
    {
        [Header("Selectors elements")]
        [SerializeField] UnityEngine.GameObject[] itemsInventory;
        [SerializeField] UnityEngine.GameObject[] itemsProjectile;
        List<GameObject> activeInventoryItems;
        [SerializeField] TMP_Text[] txtBulletsLeft;
        [Header("References")]
        [SerializeField] InventoryUIManager inventoryUIManager;
        [Header("Configuration")]
        [SerializeField, Range(1f, 10f), Tooltip("The offset associated to the max vertical movement when a item is selected")] float offset;



        UnityEngine.GameObject selectedGameObject;


        int currentIndex;


        public event Action<int, UnityEngine.GameObject> OnChangeEquippedItemEvent;


        //RectTransform rectTransform;


        void Awake()
        {
            #region ASSERTIONS
            Assert.IsTrue(itemsInventory.Length > 0, "ERROR: items in inventory is empty");
            Assert.IsTrue(itemsProjectile.Length > 0, "ERROR: items projectiles is empty");
            Assert.IsNotNull(inventoryUIManager, "ERROR: UIManager not added to FoodSelectorManager");
            #endregion
            inventoryUIManager.OnRechargeCompleteEvent += OnRechargeCompleCallBack;
            activeInventoryItems = new List<GameObject>();
            //rectTransform = itemsInventory[0].GetComponent<RectTransform>();


            //Instanciar
            //
            //SetHerarchyIndex
            Init();

        }

        internal void Init()
        {


            for (int i = 0; i < itemsInventory.Length; i++)
            {
                itemsInventory[i].SetActive(false);

                int bulletsLeft = GetBullets(itemsInventory[i]);

                txtBulletsLeft[i].text = bulletsLeft.ToString();
            }


            inventoryUIManager.OnSelectedItemInventoryEvent += OnSelectedItemInventoryCallBack;
        }



        private void OnRechargeCompleCallBack(int currentProjectile)
        {
            if (activeInventoryItems[currentProjectile].TryGetComponent(out IRechargeable rechargeable))
            {
                int bulletsLeft = GetBullets(activeInventoryItems[currentProjectile]);
                rechargeable.IsRecharging = false;
                txtBulletsLeft[currentProjectile].text = bulletsLeft.ToString();

            }
        }


        internal void StartGame()
        {

            // foreach (var item in itemsInventory)
            // {
            //     item.SetActive(true);
            // }

            for (int i = 0; i < inventoryUIManager.initialNumberWeapons; i++)
            {
                itemsInventory[i].SetActive(true);
                // activeInventoryItems.Add(itemsInventory[i]);
                if (activeInventoryItems.Count <= inventoryUIManager.initialNumberWeapons)
                {
                    activeInventoryItems.Add(itemsInventory[i]);
                }

            }

            selectedGameObject = activeInventoryItems[0];
            //OnSelectedItemInventoryCallBack(0);

        }

        private void OnSelectedItemInventoryCallBack(int index, float timeTweenRotation, float inventoryMaxRotation)
        {
            int newIndex = index;
            // Si es el mismo elemento, no hacemos nada
            if (currentIndex == newIndex)
                return;

            // Desrotar el anterior
            if (currentIndex != -1)
            {
                RectTransform prev = txtBulletsLeft[currentIndex].GetComponent<RectTransform>();
                // RectTransform prev2 = itemsProjectile[currentIndex].GetComponent<RectTransform>();

                prev.DOKill();
                prev.DOLocalRotate(Vector3.zero, timeTweenRotation);

                // prev2.DOKill();
                // prev2.DOLocalRotate(Vector3.zero, timeTweenRotation);
            }

            // Rotar el nuevo
            RectTransform current = txtBulletsLeft[newIndex].GetComponent<RectTransform>();
            // RectTransform current2 = itemsProjectile[newIndex].GetComponent<RectTransform>();
            current.DOKill();
            current.DOLocalRotate(new Vector3(0, 0, -inventoryMaxRotation), timeTweenRotation);
            // current2.DOKill();
            // current2.DOLocalRotate(new Vector3(0, 0, -inventoryMaxRotation), timeTweenRotation);

            selectedGameObject = activeInventoryItems[newIndex];
            currentIndex = index;

            //OnChangeEquippedItemEvent?.Invoke(currentIndex, itemsProjectile[currentIndex]);
            // StartCoroutine(SelectionEffectCoroutine());
        }

        private int GetBullets(UnityEngine.GameObject goProvider)
        {
            if (goProvider.TryGetComponent(out IRechargeable rechargeable))
            {

                int bulletsLeft = rechargeable.GetBullets();
                return bulletsLeft;
            }
            return 1;
        }

        internal void TryShootCurrentWeapon(Vector3 position)
        {
            if (selectedGameObject.TryGetComponent(out IThrowable throwable))
            {
                Debug.Log("Throwing");
                throwable.TryThrow(position);
                SetBulletsToText();
            }
            if (selectedGameObject.TryGetComponent(out IShootable shootable))
            {
                shootable.TryShoot();
                SetBulletsToText();
            }
            if (selectedGameObject.TryGetComponent(out IPlantable plantable))
            {
                Debug.Log("Planting");
                plantable.TryPlant();
                SetBulletsToText();

            }

        }

        private void SetBulletsToText()
        {
            int bulletsLeft = GetBullets(selectedGameObject);
            txtBulletsLeft[currentIndex].text = bulletsLeft.ToString();
        }

        internal void ReloadCurrentWeapon()
        {
            IRechargeable rechargeable = selectedGameObject.GetComponent<IRechargeable>();
            rechargeable.TryReload();
        }

        internal void EndGame()
        {


            for (int i = 0; i < itemsInventory.Length; i++)
            {
                itemsInventory[i].SetActive(false);

                if (itemsInventory[i].TryGetComponent(out FoodProvider foodProvider))
                {
                    foodProvider = itemsInventory[i].GetComponent<FoodProvider>();
                    foodProvider.Init();
                }

            }
            activeInventoryItems.Clear();

        }

        internal void UnlockWeapon(int provIndex)
        {
            itemsInventory[provIndex].SetActive(true);
            activeInventoryItems.Add(itemsInventory[provIndex]);
        }

        internal void RefreshBullets()
        {
            for (int i = 0; i < activeInventoryItems.Count; i++)
            {
                OnRechargeCompleCallBack(i);
            }
        }
    }

}