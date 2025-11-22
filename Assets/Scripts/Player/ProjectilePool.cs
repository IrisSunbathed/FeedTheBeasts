using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace FeedTheBeasts.Scripts
{
    public class ProjectilePool : MonoBehaviour
    {
        [SerializeField] GameObject goEquippedProjectile;
        [SerializeField, Min(10)] int projectilePoolLength = 10;

        MeshFilter meshNewProjectile;
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
            projectile.transform.SetPositionAndRotation(transform.position, transform.rotation);
            projectile.tag = projectileTag;
            MeshFilter meshProjectile = projectile.GetComponent<MeshFilter>();
            meshProjectile.sharedMesh = meshNewProjectile.sharedMesh;

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

            meshNewProjectile = goEquippedProjectile.GetComponent<MeshFilter>();
            projectileTag = go.tag;
        }

    }

}