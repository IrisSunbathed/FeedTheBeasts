using System;
using System.Collections;
using NUnit.Framework;
using Unity.Mathematics;
using UnityEngine;

namespace FeedTheBeasts.Scripts
{
    public class FruitProvider : FoodProvider, IRechargeable, IPlantable
    {
        [SerializeField] float plantingTime;
        [SerializeField] GameObject fruitBasket;
        

        public float PlantingTime { get => plantingTime; set { plantingTime = value; } }
        public bool IsRecharging { get; set; }

        public event Action<float> OnRechargeEvent;
        public event Action<float> OnPlantEvent;

        void Awake()
        {
            Assert.IsNotNull(fruitBasket, "ERROR: fruitBasket not added");
            Init();
        }

        public override void Init()
        {
            StopAllCoroutines();
            canShoot = true;
            shootCount = 0;
            IsRecharging = false;
        }

        public int GetBullets()
        {
            if (IsRecharging)
            {
                return 0;
            }
            else
            {
                return projectilesPerRecharge - shootCount;
            }

        }

        public void IncreaseShootCount()
        {
            shootCount++;
            if (shootCount == projectilesPerRecharge)
            {
                StartCoroutine(ReloadCoroutine());
                OnRechargeEvent?.Invoke(rechargingTime);
                shootCount = 0;
            }
        }

        public IEnumerator ReloadCoroutine()
        {
            IsRecharging = true;
            yield return new WaitForSeconds(rechargingTime);
            IsRecharging = false;
        }

        public void TryPlant()
        {
            Debug.Log($"!IsRecharging {!IsRecharging}");
            if (!IsRecharging)
            {
                
                OnPlantEvent?.Invoke(PlantingTime);
            }
        }

        public void TryReload()
        {
            if (!IsRecharging)
            {
                OnRechargeEvent?.Invoke(rechargingTime);
                StartCoroutine(ReloadCoroutine());
                shootCount = 0;
            }
        }

        internal void Plant(Vector3 position)
        {
            Instantiate(fruitBasket, position, quaternion.identity);
            IncreaseShootCount();
        }
    }
}