using System.Collections;
using FeedTheBeasts.Scripts;
using Unity.Mathematics;
using UnityEngine;

namespace FeedTheBeasts.Scripts
{

    public abstract class FoodProvider : MonoBehaviour
    {
        [Header("Shoot properties", order = 1)]
        [Header("Recharge", order = 2)]
        [SerializeField] protected float rechargingTime;
        [SerializeField] protected float cooldown;
        [SerializeField] protected int projectilesPerRecharge;
        [Header("Bullets", order = 2)]
        [SerializeField] protected float shootCooldown;
        [SerializeField, Range(20, 50)] protected int increaseTotalBulletCount;
        internal int shootCount;
        [SerializeField, Range(0.25f, 3f)] float decreaseCooldownPowerUp;
        [Header("Position", order = 2)]
        // [SerializeField] protected ProjectilePool projectilePool;
        [SerializeField] protected Transform playerPosition;
        [Header("Other", order = 2)]
        [SerializeField] FoodTypes foodTypes;


        public virtual void Init() { }

        public virtual void AddBulletsPowerUp()
        {
            projectilesPerRecharge += increaseTotalBulletCount;
            shootCount = 0;
        }
        public virtual void DecreaseCooldownPowerUp()
        {
            shootCooldown -= decreaseCooldownPowerUp;
        }
        protected bool canShoot;


    }


}