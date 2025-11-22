using System;
using System.Collections;
using UnityEngine;

namespace FeedTheBeasts.Scripts
{
    public class CarrotProvider : FoodProvider, IRechargeable, IShootable

    {
        public bool IsRecharging { get; set; }

        public event Action<float> OnRechargeEvent;

        void Awake()
        {
            canShoot = true;
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


        public void TryShoot()
        {
            if (canShoot & !IsRecharging)
            {
                projectilePool.GetProjectile();
                if (shootCount == projectilesPerRecharge)
                {
                    shootCooldown = rechargingTime;

                    shootCount = 0;
                }
                else
                {
                    cooldown = shootCooldown;
                    IncreaseShootCount();
                }
                StartCoroutine(ShootDelay());
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
            if (IsRecharging)
            {
                return 0;
            }
            else
            {
                return projectilesPerRecharge - shootCount;
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


    }


}