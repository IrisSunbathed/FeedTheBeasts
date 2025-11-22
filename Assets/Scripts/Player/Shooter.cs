using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Experimental.AI;

namespace FeedTheBeasts.Scripts
{

    public class Shooter : MonoBehaviour
    {
        [Header("Projectile Pool References and Properties")]
        [SerializeField] ProjectilePool projectilePool;
        [SerializeField, Range(0.05f, 2f)] float shootCooldown;
        [SerializeField, Range(0.05f, 2f)] float rechageCooldown;

        [SerializeField] GameObject[] foodProviders;


        float coolDown;
        int shootCount;
        [SerializeField] int maxShootsRecharge;
        bool canShoot;




        void Awake()
        {
            Assert.IsNotNull(projectilePool, "ERROR: projectilePool not added to Shooter");
            canShoot = true;

         
        }
        internal void TryShoot()
        {
            if (canShoot)
            {
                IRechargeable projectile = projectilePool.GetProjectile().GetComponent<IRechargeable>();
           
                if (shootCount == maxShootsRecharge)
                {
                    coolDown = rechageCooldown;
      
                    shootCount = 0;
                }
                else
                {
                    coolDown = shootCooldown;
                }
                StartCoroutine(ShootDelay());
            }
        }
        IEnumerator ShootDelay()
        {
            canShoot = false;
            yield return new WaitForSeconds(coolDown);
            canShoot = true;
        }
    }

}