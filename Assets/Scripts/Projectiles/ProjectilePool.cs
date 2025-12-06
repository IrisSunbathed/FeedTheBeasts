using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Pool;

namespace FeedTheBeasts.Scripts
{
   
    public class ProjectilePool : MonoBehaviour
    {
        [SerializeField] GameObject goEquippedProjectile;
        [SerializeField, Min(10)] int projectilePoolLength = 10;

        MeshFilter meshNewProjectile; //The new controller would have its own 
        GameObject[] listProjectiles;
        string projectileTag;

        

        int indexProjectile;

        void Start()
        {
            indexProjectile = 0;
            meshNewProjectile = goEquippedProjectile.GetComponent<MeshFilter>();
            projectileTag = goEquippedProjectile.tag;
            CreateProjectiles();

        }

        void Awake()
        {
            Assert.IsNotNull(goEquippedProjectile, "ERROR: goEquippedProjectile is not added");
        }

        internal GameObject GetProjectile()
        {
            var projectile = listProjectiles[indexProjectile];

            ConfigureProjectile(projectile);
            indexProjectile++;
            if (indexProjectile == listProjectiles.Length)
            {
                indexProjectile = 0;
            }
            return projectile;
        }

        private void ConfigureProjectile(GameObject projectile)
        {
            projectile.SetActive(true);
            SetTransform(projectile);
            SetUpController(projectile);
            SetMesh(projectile);
            projectile.tag = projectileTag;

        }
        private ThrowableController ConfigureThrowable(GameObject projectile)
        {
            projectile.SetActive(true);

            SetTransform(projectile);
            SetMesh(projectile);
            projectile.tag = projectileTag;
            if (projectile.TryGetComponent(out ThrowableController throwableController))
            {
                throwableController.SetUp();
                return throwableController;
            }
            else
            {
                return projectile.AddComponent<ThrowableController>();
            }
        }


        private void SetMesh(GameObject projectile)
        {
            MeshFilter meshProjectile = projectile.GetComponent<MeshFilter>();
            meshProjectile.sharedMesh = meshNewProjectile.sharedMesh;
            // MeshRenderer meshFilter = projectile.GetComponent<MeshRenderer>();
            // meshFilter.materials[0] =
        }

        private void SetUpController(GameObject projectile)
        {
            if (projectile.TryGetComponent(out StraightController straightController))
            {
                straightController.SetUpController();
            }
            else
            {
                projectile.AddComponent<StraightController>();
            }
        }

        private void SetTransform(GameObject projectile)
        {
            projectile.transform.SetPositionAndRotation(transform.position, transform.rotation);
            projectile.transform.localScale = goEquippedProjectile.transform.localScale;
        }

        private void CreateProjectiles()
        {
            listProjectiles = new GameObject[projectilePoolLength];

            for (int i = 0; i < listProjectiles.Length; i++)
            {
                listProjectiles[i] = Instantiate(goEquippedProjectile);

                listProjectiles[i].SetActive(false);
            }
        }

        internal void SetProjectile(GameObject go)
        {
            goEquippedProjectile = go;

            meshNewProjectile = go.GetComponent<MeshFilter>();
            projectileTag = go.tag;
        }

        internal void ThrowProjectile(Vector3 target)
        {
            var projectile = listProjectiles[indexProjectile];

            ThrowableController throwableController = ConfigureThrowable(projectile);
            throwableController.SimulateProjectile(target);
            indexProjectile++;

            if (indexProjectile == listProjectiles.Length)
            {
                indexProjectile = 0;
            }

        }


    }

}