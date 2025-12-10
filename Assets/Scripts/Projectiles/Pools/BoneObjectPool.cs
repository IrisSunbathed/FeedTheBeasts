using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;

namespace FeedTheBeasts.Scripts
{
    public class BoneObjectPool : GenericObjectPool
    {

        [SerializeField] GameObject goBone;
        internal ObjectPool<GameObject> opThrowableObject;

        void Awake()
        {
            opThrowableObject = new ObjectPool<GameObject>(OnCreateEvent, OnActionGet, OnRelease, OnPODestroy, false, defaultCapacity, maxPoolSize);
        }

        private void OnPODestroy(GameObject projectile)
        {
            Destroy(projectile);
        }

        private void OnRelease(GameObject projectile)
        {
            projectile.SetActive(false);
        }

        private void OnActionGet(GameObject projectile)
        {
            projectile.SetActive(true);
            //projectile.transform.SetParent(transform, true);
        }

        private GameObject OnCreateEvent()
        {
            GameObject newProjectile = Instantiate(goBone, Vector3.zero, quaternion.identity);

            DistractCollision distractCollision = newProjectile.GetComponent<DistractCollision>();

            distractCollision.OnWastedItem += ReturnToObjectPool;


            return newProjectile;
        }

        private void ReturnToObjectPool(GameObject instance)
        {
            opThrowableObject.Release(gameObject);
        }
    }





}

