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
<<<<<<< Updated upstream
        protected int shootCount;
        [SerializeField] protected ProjectilePool projectilePool;
=======
        internal int shootCount;
        // [SerializeField] protected ProjectilePool projectilePool;
        [SerializeField] protected Transform playerPosition;

>>>>>>> Stashed changes

        [SerializeField] FoodTypes foodTypes;

        public virtual void Init() { }


        protected bool canShoot;


    }


}