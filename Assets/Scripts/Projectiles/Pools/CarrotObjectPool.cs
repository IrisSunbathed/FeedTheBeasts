using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Pool;

namespace FeedTheBeasts.Scripts
{
    public class CarrotObjectPool : GenericObjectPool
    {

        [SerializeField] GameObject goCarrot;
        [SerializeField, Range(10, 20)] float projectileSpeed;
        [SerializeField, Range(5, 10)] float addedPowerUpSpeed;

        internal bool doesTrackAnimals;

        internal ObjectPool<GameObject> opStraightProjectile;

        void Awake()
        {
            Assert.IsNotNull(playerTransform, "ERROR: playerTransform is not added");
            opStraightProjectile = new ObjectPool<GameObject>(OnCreateEvent, OnActionGet, OnRelease, OnPODestroy, true, defaultCapacity, maxPoolSize);
        }

        private void OnPODestroy(GameObject projectile)
        {
            Destroy(projectile);
        }

        private void OnRelease(GameObject projectile)
        {
            EnableComponents(projectile, false);
            StraightProjectile strProj = projectile.GetComponent<StraightProjectile>();
            strProj.followedAnimal = null;
            strProj.currentSpeed = 0;
        }

        private void OnActionGet(GameObject projectile)
        {
            EnableComponents(projectile, true);
            projectile.GetComponent<StraightProjectile>().SetUp(projectileSpeed, playerTransform, doesTrackAnimals);
        }

        internal override void EnableComponents(GameObject projectile, bool areActive)
        {
            projectile.SetActive(areActive);
            projectile.GetComponent<MeshRenderer>().enabled = areActive;
            projectile.GetComponent<Collider>().enabled = areActive;
            if (!areActive)
            {
                projectile.GetComponentInChildren<TrailRenderer>().enabled = areActive;
            }
            else
            {
                StartCoroutine(EnableTrailRendererCoroutine(projectile, areActive));
            }
        }

        IEnumerator EnableTrailRendererCoroutine(GameObject projectile, bool areActive)
        {
            yield return new WaitForSeconds(timeTrailReactivate);
            projectile.GetComponentInChildren<TrailRenderer>().enabled = areActive;
        }

        private GameObject OnCreateEvent()
        {
            GameObject newProjectile = Instantiate(goCarrot, Vector3.zero, quaternion.identity);

            DetectCollisions newStraightProjectile = newProjectile.GetComponent<DetectCollisions>();

            newStraightProjectile.OnInvisible += ReturnToObjectPool;

            return newProjectile;
        }

        private void ReturnToObjectPool(GameObject instance)
        {
            opStraightProjectile.Release(instance);
        }

        internal void SetSpeedPowerUp()
        {
            projectileSpeed += addedPowerUpSpeed;
        }
    }

}

