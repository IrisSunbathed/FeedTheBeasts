using System;
using System.Collections;
using UnityEngine;

namespace FeedTheBeasts.Scripts
{
    [RequireComponent(typeof(AudioSource))]
    public class BoneProvider : FoodProvider, IRechargeable, IThrowable

    {
        public bool IsRecharging { get; set; }
        public AudioSource AudioSourceShoot { get; set; }

        public event Action<float> OnRechargeEvent;

        GameCatalog gameCatalog;

        void Start()
        {
            gameCatalog = GameCatalog.Instance;
        }

        void Awake()
        {
            Init();
            AudioSourceShoot = GetComponent<AudioSource>();
        }

        public override void Init()
        {
            StopAllCoroutines();
            canShoot = true;
            shootCount = 0;
        }

        public IEnumerator ReloadCoroutine()
        {
            IsRecharging = true;
            yield return new WaitForSeconds(rechargingTime);
            IsRecharging = false;
        }

        private void ConfigureAudio()
        {
            AudioClip audioClip = gameCatalog.GetFXClip(FXTypes.Shot);
            AudioSourceShoot.resource = audioClip;
            AudioSourceShoot.pitch = .5f;
            AudioSourceShoot.Play();
        }

        public void IncreaseShootCount()
        {
            shootCount++;
            if (shootCount == projectilesPerRecharge)
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
            return projectilesPerRecharge - shootCount;
        }

        public void TryThrow(Vector3 position)
        {
            if (canShoot & !IsRecharging)
            {
                projectilePool.ThrowProjectile(position);
                ConfigureAudio();

                if (shootCount == projectilesPerRecharge)
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
    }


}