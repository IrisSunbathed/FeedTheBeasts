using System.Collections;
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
            // Destroy(GameObject);
        }

        private void OnRelease(GameObject projectile)
        {
            EnableComponents(projectile, false);
        }

        private void OnActionGet(GameObject projectile)
        {
            EnableComponents(projectile, true);
        }

        internal override void EnableComponents(GameObject projectile, bool areActive)
        {
            projectile.SetActive(areActive);
            projectile.GetComponent<MeshRenderer>().enabled = areActive;
            projectile.GetComponent<Collider>().enabled = areActive;
            StartCoroutine(EnableTrailRendererCoroutine(projectile, areActive));
        }
        IEnumerator EnableTrailRendererCoroutine(GameObject projectile, bool isActive)
        {
            yield return new WaitForSeconds(timeTrailReactivate);
            projectile.GetComponentInChildren<TrailRenderer>().enabled = isActive;
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
            opThrowableObject.Release(instance);
        }
    }
}

