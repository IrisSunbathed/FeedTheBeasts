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

        public event Action<GameObject> OnAnimalSpawnEvent;

        float[] extremePosition;
        int index;

        Camera mainCam;

        float lengthCam;

        float upperLimitCamera;
        readonly float offset = 4f;

        GameObject lastAnimalSpawned;
        GameObject lastAggresiveAnimalSpawned;

        void Awake()
        {
            Assert.IsTrue(goPrefabs.Length > 0, "ERROR: no game objects were added to the array in SpawnManager");

            mainCam = Camera.main;
            upperLimitCamera = mainCam.orthographicSize * 2;
            lengthCam = mainCam.orthographicSize * mainCam.aspect;

            extremePosition = new float[]
            {
            -lengthCam - offset,
            lengthCam + offset
            };



          
        }


        private void SpawnRandomAnimal()
        {
            index = Random.Range(0, goPrefabs.Length);
            float randomXValue = Random.Range(-lengthCam, lengthCam);
            Vector3 spawnPosition = new Vector3(randomXValue,
                                                 goPrefabs[index].transform.position.y,
                                                upperLimitCamera + offset * -Mathf.Sign(upperLimitCamera));

            lastAnimalSpawned = Instantiate(goPrefabs[index], spawnPosition, goPrefabs[index].transform.rotation);
            

            OnAnimalSpawnEvent?.Invoke(lastAnimalSpawned);

            


        }

        private void SpawnAggresiveAnimal()
        {
            float randomZValue = Random.Range(0, mainCam.orthographicSize);
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
            InvokeRepeating(nameof(SpawnRandomAnimal), startDelay, Random.Range(intervalSpawnMin, intervalSpawnMin));
            InvokeRepeating(nameof(SpawnAggresiveAnimal), startDelay, Random.Range(intervalSpawnAggressiveMin, intervalSpawnAggresiveMax));
            foreach (var animals in GameObject.FindGameObjectsWithTag(Constants.ANIMAL_TAG))
            {
                Destroy(animals);
            } 
           
        }
    }

}