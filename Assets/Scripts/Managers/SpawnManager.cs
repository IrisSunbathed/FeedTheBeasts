using System;
using FeedTheBeasts;
using NUnit.Framework;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FeedTheBeasts.Scripts
{

    public class SpawnManager : MonoBehaviour
    {
        [Header("Prefabs to spawn")]
        [SerializeField] GameObject[] goPrefabs;
        [SerializeField] GameObject goAggresiveAnimal;
        [Header("Manage time to spawn")]
        [SerializeField] float startDelay = 2f;

        [SerializeField] float intervalSpawnMin = 2f;

        [SerializeField] float intervalSpawnMax = 10f;
        [SerializeField] float intervalSpawnAggressiveMin = 10f;
        [SerializeField] float intervalSpawnAggresiveMax = 20f;

        #region Cameras

        CamerasManager camerasManager;

        float[] extremePosition;
        float lengthCam;

        #endregion
        public event Action<GameObject> OnAnimalSpawnEvent;

        int index;



        readonly float offset = 4f;

        GameObject lastAnimalSpawned;
        GameObject lastAggresiveAnimalSpawned;

        void Start()
        {
            camerasManager = CamerasManager.Instance;
        }
        void Awake()
        {
            Assert.IsTrue(goPrefabs.Length > 0, "ERROR: no game objects were added to the array in SpawnManager");
        }


        private void SpawnRandomAnimal()
        {
            index = Random.Range(0, goPrefabs.Length);
            float randomXValue = Random.Range(-lengthCam, lengthCam);
            Vector3 spawnPosition = new Vector3(randomXValue,
                                                 goPrefabs[index].transform.position.y,
                                                camerasManager.UpperLimitCamera + offset * -Mathf.Sign(camerasManager.UpperLimitCamera));

            lastAnimalSpawned = Instantiate(goPrefabs[index], spawnPosition, goPrefabs[index].transform.rotation);
            

            OnAnimalSpawnEvent?.Invoke(lastAnimalSpawned);

            


        }

        private void SpawnAggresiveAnimal()
        {
            float randomZValue = Random.Range(0, camerasManager.OrthographicSize);
            int randomXValue = Random.Range(0, 2);



            Vector3 spawnPosition = new Vector3(extremePosition[randomXValue],
                                                goPrefabs[index].transform.position.y,
                                                randomZValue);

            lastAggresiveAnimalSpawned = Instantiate(goAggresiveAnimal, spawnPosition, goPrefabs[index].transform.rotation);
            
            OnAnimalSpawnEvent?.Invoke(lastAggresiveAnimalSpawned);
        }

        internal void StopSpawning()
        {
            CancelInvoke(nameof(SpawnRandomAnimal));
            CancelInvoke(nameof(SpawnAggresiveAnimal));
            MoveForward moveForward = lastAnimalSpawned.GetComponent<MoveForward>();
            moveForward.SetSpeed(0f);


        }

        internal void Init()
        {
            ConfigureCameraExtremes();
            InvokeRepeating(nameof(SpawnRandomAnimal), startDelay, Random.Range(intervalSpawnMin, intervalSpawnMin));
            InvokeRepeating(nameof(SpawnAggresiveAnimal), startDelay, Random.Range(intervalSpawnAggressiveMin, intervalSpawnAggresiveMax));
            foreach (var animals in GameObject.FindGameObjectsWithTag(Constants.ANIMAL_TAG))
            {
                Destroy(animals);
            }


        }

        private void ConfigureCameraExtremes()
        {

            lengthCam = camerasManager.GetCameraLength();

            extremePosition = new float[]
            {
            -lengthCam - offset,
            lengthCam + offset
            };
        }
    }

}