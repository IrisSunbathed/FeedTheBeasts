using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;

namespace FeedTheBeasts.Scripts
{
    public class CarrotObjectPool : GenericObjectPool
    {

        [SerializeField] GameObject goCarrot;
        internal ObjectPool<GameObject> opStraightProjectile;

        void Awake()
        {
            opStraightProjectile = new ObjectPool<GameObject>(OnCreateEvent, OnActionGet, OnRelease, OnPODestroy, true, defaultCapacity, maxPoolSize);
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
            projectile.GetComponent<MeshRenderer>().enabled = true;
            projectile.GetComponent<Collider>().enabled = true;
            StraightProjectile straightProjectile = projectile.GetComponent<StraightProjectile>();
            straightProjectile.SetUpSpeed();
            //projectile.transform.SetParent(transform, true);
        }

        private GameObject OnCreateEvent()
        {
            GameObject newProjectile = Instantiate(goCarrot, Vector3.zero, quaternion.identity);

            StraightProjectile newStraightProjectile = newProjectile.GetComponent<StraightProjectile>();

            newStraightProjectile.OnInvisible += ReturnToObjectPool;

            return newProjectile;
        }

        private void ReturnToObjectPool(StraightProjectile instance)
        {
            opStraightProjectile.Release(instance.gameObject);
        }
    }

}

