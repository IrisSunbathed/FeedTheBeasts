using System;
using System.Collections;
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



        GameObject selectedGameObject;

        float pointObjective;

        float originalPositionY;
        int currentIndex;

        public event Action<int, GameObject> OnChangeEquippedItemEvent;
        [SerializeField] TMP_Text[] txtBulletsLeft;

        RectTransform rectTransform;


        void Awake()
        {
            #region ASSERTIONS
            Assert.IsTrue(itemsInventory.Length > 0, "ERROR: items in inventory is empty");
            Assert.IsTrue(itemsProjectile.Length > 0, "ERROR: items projectiles is empty");
            Assert.IsTrue(itemsPosition.Length > 0, "ERROR: items position is empty");
            Assert.IsNotNull(uIManager, "ERROR: UIManager not added to FoodSelectorManager");
            #endregion
            uIManager.OnRechargeCompleteEvent += OnRechargeCompleCallBack;
            rectTransform = itemsInventory[0].GetComponent<RectTransform>();
            originalPositionY = rectTransform.localPosition.y;
            Init();

        }

        private void OnRechargeCompleCallBack(int currentProjectile)
        {
            IShootable shootable = itemsInventory[currentProjectile].GetComponent<IShootable>();
            IRechargeable rechargeable = itemsInventory[currentProjectile].GetComponent<IRechargeable>();
            rechargeable.IsRecharging = false;
            txtBulletsLeft[currentProjectile].text = shootable.GetBullets().ToString();
        }

        internal void Init()
        {
            OnSelectedItemInventoryCallBack(0);


            for (int i = 0; i < itemsInventory.Length; i++)
            {
                itemsInventory[i].SetActive(false);

                FoodProvider foodProvider = itemsInventory[i].GetComponent<FoodProvider>();
                foodProvider.Init();

                IShootable shootable = itemsInventory[i].GetComponent<IShootable>();
                
                txtBulletsLeft[i].text = shootable.GetBullets().ToString();
            }

            uIManager.OnSelectedItemInventoryEvent += OnSelectedItemInventoryCallBack;
        }

        internal void StartGame()
        {

            foreach (var item in itemsInventory)
            {
                item.SetActive(true);
            }
             OnSelectedItemInventoryCallBack(0);
           
        }

        private void OnSelectedItemInventoryCallBack(int index)
        {
            currentIndex = index;
            OnChangeEquippedItemEvent?.Invoke(currentIndex, itemsProjectile[currentIndex]);
            selectedGameObject = itemsInventory[currentIndex];
            StartCoroutine(SelectionEffectCoroutine());
        }

        IEnumerator SelectionEffectCoroutine()
        {
            RectTransform rectTransform = selectedGameObject.GetComponent<RectTransform>();

            pointObjective = originalPositionY + offset;
            float newYPosition = 0;


            while (rectTransform.anchoredPosition.y < pointObjective)
            {
                newYPosition += 0.01f;
                rectTransform.anchoredPosition = new Vector3(
                   rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + newYPosition,
                     rectTransform.localPosition.z);
                yield return null;

            }
            rectTransform.anchoredPosition = new Vector3(
                   rectTransform.anchoredPosition.x, pointObjective,
                  rectTransform.localPosition.z);
            while (rectTransform.anchoredPosition.y > originalPositionY)
            {
                newYPosition -= 0.01f;
                rectTransform.anchoredPosition = new Vector3(
                     rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + newYPosition,
                    rectTransform.localPosition.z);
                yield return null;

            }

            rectTransform.anchoredPosition = new Vector3(
                  rectTransform.anchoredPosition.x, originalPositionY,
                   rectTransform.localPosition.z);
        }

        void Update()
        {
            if (selectedGameObject != null)
            {

                selectedGameObject.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
            }
        }

        internal void TryShootCurrentWeapon()
        {
            IShootable shootable = selectedGameObject.GetComponent<IShootable>();
            shootable.TryShoot();
            int bulletsLeft = shootable.GetBullets();
            txtBulletsLeft[currentIndex].text = bulletsLeft.ToString();
        }

        internal void ReloadCurrentWeapon()
        {
            IRechargeable rechargeable = selectedGameObject.GetComponent<IRechargeable>();
            rechargeable.TryReload();
        }
    }

}