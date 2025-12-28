using System;
using System.Collections;
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
        [Header("Game Object lists")]
        [SerializeField] GameObject[] itemsInventory;
        [SerializeField] GameObject[] itemsProjectile;
        [SerializeField] RectTransform[] itemsPosition;
        [Header("References")]
        [SerializeField] UIManager uIManager;
        [Header("Configuration")]
        [SerializeField, Range(10f, 40f), Tooltip("Rotation of the selected item when it is selected")] float rotationSpeed = 20f;
        [SerializeField, Range(1f, 10f), Tooltip("The offset associated to the max vertical movement when a item is selected")] float offset;


        [SerializeField] TMP_Text[] txtBulletsLeft;
        [SerializeField, Range(-45f, -180f)] float bulletMaxRotation;
        [SerializeField, Range(0.001f, 0.25f)] float timeTweenRotation;


        int selectedIndex;

        GameObject selectedGameObject;

        int currentIndex;


        public event Action<int, GameObject> OnChangeEquippedItemEvent;


      



        void Awake()
        {
            #region ASSERTIONS
            Assert.IsTrue(itemsInventory.Length > 0, "ERROR: items in inventory is empty");
            Assert.IsTrue(itemsProjectile.Length > 0, "ERROR: items projectiles is empty");
            Assert.IsTrue(itemsPosition.Length > 0, "ERROR: items position is empty");
            Assert.IsNotNull(uIManager, "ERROR: UIManager not added to FoodSelectorManager");
            #endregion
            itemsInventory[0].GetComponent<RectTransform>();
            Init();

        }

        internal void Init()
        {

            //OnSelectedItemInventoryCallBack(0);

            DestroyObjectsInScene();

            for (int i = 0; i < itemsInventory.Length; i++)
            {
                itemsInventory[i].SetActive(false);

                if (itemsInventory[i].TryGetComponent(out FoodProvider foodProvider))
                {
                    foodProvider = itemsInventory[i].GetComponent<FoodProvider>();
                    foodProvider.Init();
                }

                int bulletsLeft = GetBullets(itemsInventory[i]);

                txtBulletsLeft[i].text = bulletsLeft.ToString();
            }

            uIManager.OnRechargeCompleteEvent += OnRechargeCompleCallBack;
            uIManager.OnSelectedItemInventoryEvent += OnSelectedItemInventoryCallBack;
        }

        private static void DestroyObjectsInScene()
        {
            foreach (var item in GameObject.FindGameObjectsWithTag(Constants.THROWABLE_TAG))
            {
                Destroy(item);
            }
            foreach (var item in GameObject.FindGameObjectsWithTag(Constants.PLANTABLE_TAG))
            {
                Destroy(item);
            }
        }

        private void OnRechargeCompleCallBack(int currentProjectile)
        {
            IRechargeable rechargeable = itemsInventory[currentProjectile].GetComponent<IRechargeable>();
            int bulletsLeft = GetBullets(itemsInventory[currentProjectile]);
            rechargeable.IsRecharging = false;
            txtBulletsLeft[currentProjectile].text = bulletsLeft.ToString();
        }


        internal void StartGame()
        {

            foreach (var item in itemsInventory)
            {
                item.SetActive(true);
            }
            //OnSelectedItemInventoryCallBack(0);

        }

        private void OnSelectedItemInventoryCallBack(int index)
        {
            currentIndex = index;
            int newIndex = index;
            selectedGameObject = itemsInventory[currentIndex];


            // Si es el mismo elemento, no hacemos nada
            if (selectedIndex == newIndex)
                return;

            // Desrotar el anterior
            if (selectedIndex != -1)
            {
                RectTransform prev = txtBulletsLeft[selectedIndex].GetComponent<RectTransform>();
                prev.DOKill();
                prev.DOLocalRotate(Vector3.zero, timeTweenRotation);
            }

            // Rotar el nuevo
            RectTransform current = txtBulletsLeft[newIndex].GetComponent<RectTransform>();
            current.DOKill();
            current.DOLocalRotate(new Vector3(0, 0, bulletMaxRotation), timeTweenRotation);

            OnChangeEquippedItemEvent?.Invoke(currentIndex, itemsProjectile[currentIndex]);
            selectedIndex = newIndex;

            //Efecto
        }



        private int GetBullets(GameObject goProvider)
        {
            IRechargeable rechargeable = goProvider.GetComponent<IRechargeable>();
            int bulletsLeft = rechargeable.GetBullets();
            return bulletsLeft;
        }

        internal void TryShootCurrentWeapon(Vector3 position)
        {
            if (selectedGameObject.TryGetComponent(out IThrowable throwable))
            {
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
            txtBulletsLeft[currentIndex].text = 0.ToString();
        }


        internal void EndGame()
        {
            DestroyObjectsInScene();

            for (int i = 0; i < itemsInventory.Length; i++)
            {
                itemsInventory[i].SetActive(false);

                if (itemsInventory[i].TryGetComponent(out FoodProvider foodProvider))
                {
                    foodProvider = itemsInventory[i].GetComponent<FoodProvider>();
                    foodProvider.Init();
                }
            }

        }
    }

}