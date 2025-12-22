using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Pool;

namespace FeedTheBeasts.Scripts
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(BeefObjectPool))]
    public class BeefProvider : FoodProvider, IRechargeable, IShootable

    {
        public bool IsRecharging { get; set; }
        public AudioSource AudioSourceShoot { get; set; }

        public event Action<float> OnRechargeEvent;

        GameCatalog gameCatalog;

        BeefObjectPool beefObjectPool;

        [SerializeField] ConsecutiveShootsManager consecutiveShootsManager;

        //ObjectPool<GameObject> objectPool;

        void Start()
        {
            gameCatalog = GameCatalog.Instance;
        }

        void Awake()
        {
            
            Assert.IsNotNull(consecutiveShootsManager, "ERROR: consecutiveShootsManager not added");
            AudioSourceShoot = GetComponent<AudioSource>();
            beefObjectPool = GetComponent<BeefObjectPool>();
            Init();
        }

        public override void Init()
        {
            StopAllCoroutines();
            beefObjectPool.StopAllCoroutines();
            canShoot = true;
            shootCount = 0;
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
                // projectilePool.GetProjectile();
                GameObject newPro = beefObjectPool.opStraightProjectile.Get();
                DetectCollisions detectCollisions = newPro.GetComponent<DetectCollisions>();
                consecutiveShootsManager.SubscribeToEvents(detectCollisions);
                newPro.transform.SetPositionAndRotation(playerPosition.position, playerPosition.rotation);
                AudioClip audioClip = gameCatalog.GetFXClip(FXTypes.Shot);
                AudioSourceShoot.resource = audioClip;
                AudioSourceShoot.pitch = .5f;
                AudioSourceShoot.Play();

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
    }

    
}