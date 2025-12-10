using System.Collections;
using FeedTheBeasts.Scripts;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;

namespace FeedTheBeasts.Scripts
{

    public abstract class FoodProvider : MonoBehaviour
    {
        [SerializeField] protected float rechargingTime;
        [SerializeField] protected float cooldown;
        [SerializeField] protected float shootCooldown;
        [SerializeField] protected int projectilesPerRecharge;
        protected int shootCount;
        [SerializeField] protected ProjectilePool projectilePool;
            [SerializeField] protected Transform playerPosition;


        [SerializeField] FoodTypes foodTypes;

        public virtual void Init() {}


        protected bool canShoot;

      
    }
    

}