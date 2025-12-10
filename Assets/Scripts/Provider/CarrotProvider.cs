using System;
using System.Collections;
using UnityEngine;

namespace FeedTheBeasts.Scripts
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(CarrotObjectPool))]
    public class CarrotProvider : FoodProvider, IRechargeable, IShootable

    {
        public bool IsRecharging { get; set; }
        public AudioSource AudioSourceShoot { get; set; }

        public event Action<float> OnRechargeEvent;

        GameCatalog gameCatalog;

        CarrotObjectPool carrotObjectPool;


        void Start()
        {
            gameCatalog = GameCatalog.Instance;
        }

        void Awake()
        {
            Init();
            AudioSourceShoot = GetComponent<AudioSource>();
            carrotObjectPool = GetComponent<CarrotObjectPool>();
        }

        public override void Init()
        {
            StopAllCoroutines();
            canShoot = true;
            shootCount = 0;
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
                GameObject newPro = carrotObjectPool.opStraightProjectile.Get();
                newPro.transform.SetPositionAndRotation(playerPosition.position, playerPosition.rotation);
                // projectilePool.GetProjectile();
                AudioClip audioClip = gameCatalog.GetFXClip(FXTypes.Shot);

                AudioSourceShoot.resource = audioClip;
                // AudioSourceShoot.pitch = -3f;
                AudioSourceShoot.Play();
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