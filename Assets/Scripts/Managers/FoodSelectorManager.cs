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
        [SerializeField] UnityEngine.GameObject[] itemsInventory;
        [SerializeField] UnityEngine.GameObject[] itemsProjectile;
        [SerializeField] RectTransform[] itemsPosition;
        [Header("References")]
        [SerializeField] UIManager uIManager;
        [Header("Configuration")]
        [SerializeField, Range(10f, 40f), Tooltip("Rotation of the selected item when it is selected")] float rotationSpeed = 20f;
        [SerializeField, Range(1f, 10f), Tooltip("The offset associated to the max vertical movement when a item is selected")] float offset;



        UnityEngine.GameObject selectedGameObject;

        float pointObjective;

        float originalPositionY;
        int currentIndex;

        public event Action<int, UnityEngine.GameObject> OnChangeEquippedItemEvent;
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

        internal void Init()
        {

            OnSelectedItemInventoryCallBack(0);

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

            uIManager.OnSelectedItemInventoryEvent += OnSelectedItemInventoryCallBack;
        }

        private static void DestroyObjectsInScene()
        {
            foreach (var item in UnityEngine.GameObject.FindGameObjectsWithTag(Constants.THROWABLE_TAG))
            {

                Destroy(item);
            }
            foreach (var item in UnityEngine.GameObject.FindGameObjectsWithTag(Constants.PLANTABLE_TAG))
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



        private int GetBullets(UnityEngine.GameObject goProvider)
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
        }

    }

}