using System;
using System.Collections;
using UnityEngine;

namespace FeedTheBeasts.Scripts
{
    public class BeefProvider : FoodProvider, IRechargeable, IShootable

    {
        public bool IsRecharging { get; set; }

        public event Action<float> OnRechargeEvent;

        void Awake()
        {
            Init();
        }

        public override void Init()
        {
            canShoot = true;
            currentProjectiles = projectilesPerRecharge;
        }

        public IEnumerator ReloadCoroutine()
        {
            IsRecharging = true;
            yield return new WaitForSeconds(rechargingTime);
            IsRecharging = false;
        }

        public void TryShoot()
        {
            if (canShoot & !IsRecharging)
            {
                projectilePool.GetProjectile();

                if (shootCount == currentProjectiles)
                {
                    shootCooldown = rechargingTime;
                }
                else
                {
                    cooldown = shootCooldown;
                    IncreaseShootCount();
                }

                StartCoroutine(ShootDelay());
            }

        }


        public void IncreaseShootCount()
        {
            shootCount++;
            if (shootCount == currentProjectiles)
            {
                TryReload();
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

        IEnumerator ShootDelay()
        {
            canShoot = false;
            yield return new WaitForSeconds(cooldown);
            canShoot = true;
        }

        public int GetBullets()
        {
            return currentProjectiles - shootCount;
        }
    }


}