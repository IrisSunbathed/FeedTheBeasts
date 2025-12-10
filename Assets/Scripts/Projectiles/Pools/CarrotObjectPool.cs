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
            projectile.GetComponent<MeshRenderer>().enabled = false;
            projectile.GetComponent<Collider>().enabled = false;
            projectile.GetComponentInChildren<TrailRenderer>().enabled = false;
            projectile.GetComponent<StraightProjectile>().currentSpeed = 0;
        }

        private void OnActionGet(GameObject projectile)
        {
            EnableComponents(projectile);
            projectile.GetComponent<StraightProjectile>().SetUpSpeed();
            //projectile.transform.SetParent(transform, true);
        }

        private static void EnableComponents(GameObject projectile)
        {
            projectile.SetActive(true);
            projectile.GetComponent<MeshRenderer>().enabled = true;
            projectile.GetComponent<Collider>().enabled = true;
            projectile.GetComponentInChildren<TrailRenderer>().enabled = true;
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

