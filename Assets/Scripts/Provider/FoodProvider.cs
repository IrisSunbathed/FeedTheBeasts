using System.Collections;
using FeedTheBeasts.Scripts;
using Unity.Mathematics;
using UnityEngine;

namespace FeedTheBeasts.Scripts
{

    public abstract class FoodProvider : MonoBehaviour
    {
        [SerializeField] protected float rechargingTime;
        [SerializeField] protected float cooldown;
        [SerializeField] protected float shootCooldown;
        [SerializeField] protected int projectilesPerRecharge;
        protected int currentProjectiles;
        [SerializeField] protected ProjectilePool projectilePool;

        [SerializeField] FoodTypes foodTypes;

        public virtual void Init() {}

        protected int shootCount;

        protected bool canShoot;

      
    }
    

}