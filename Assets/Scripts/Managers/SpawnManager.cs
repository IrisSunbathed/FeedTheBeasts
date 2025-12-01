using System;
using System.Collections;
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
        [Header("References")]
        [SerializeField] DifficultyManager difficultyManager;
        LevelManager levelManager;

        [SerializeField] bool spawnAnimals;

        int numberSpawnAnimals;

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
        delegate void MyMethodDelegate();

        Coroutine coroutineAnimals;
        Coroutine coroutineAggressiveAnimals;




        void Start()
        {
            camerasManager = CamerasManager.Instance;
            levelManager = LevelManager.Instance;
        }
        void Awake()
        {
            Assert.IsTrue(goPrefabs.Length > 0, "ERROR: no game objects were added to the array in SpawnManager");
            Assert.IsNotNull(difficultyManager, "ERROR: difficultyManager was not added to SpawnManager");
        }


        internal void Init()
        {
            ConfigureCameraExtremes();
            if (spawnAnimals)
            {
                StartCoroutine(StartCouroutines());
            }

            foreach (var animals in GameObject.FindGameObjectsWithTag(Constants.ANIMAL_TAG))
            {
                Destroy(animals);
            }


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
            // CancelInvoke(nameof(SpawnRandomAnimal));
            // CancelInvoke(nameof(SpawnAggresiveAnimal));
            StopAllCoroutines();
            coroutineAnimals = null;
            coroutineAggressiveAnimals = null;


            DestroyAnimals();
        }

        private static void DestroyAnimals()
        {
            foreach (var animals in GameObject.FindGameObjectsWithTag(Constants.ANIMAL_TAG))
            {
                Destroy(animals);
            }
        }

        internal void Stampede(int numberOfAnimals)
        {
            Debug.Log("Stampedeeee!");
            StopAllCoroutines();
            coroutineAnimals = null;
            coroutineAggressiveAnimals = null;

            for (int i = 0; i <= numberOfAnimals; i++)
            {
                SpawnRandomAnimal();
            }

            StartCoroutine(StartCouroutines(5f));
        }



        IEnumerator StartCouroutines(float timeToWait = 0f)
        {
            yield return timeToWait;
            coroutineAnimals ??= StartCoroutine(SpawnRandomAnimalCoroutine(difficultyManager.StartDelay,
                                                                      difficultyManager.IntervalSpawnMin,
                                                                      difficultyManager.IntervalSpawnMax,
                                                                      SpawnRandomAnimal));
            coroutineAggressiveAnimals ??= StartCoroutine(SpawnRandomAnimalCoroutine(difficultyManager.StartDelayAggressiveAnimals,
                                                      difficultyManager.IntervalSpawnAggressiveMin,
                                                      difficultyManager.IntervalSpawnAggressiveMin,
                                                      SpawnAggresiveAnimal));
        }

        IEnumerator SpawnRandomAnimalCoroutine(float startDelay, float intervalMin, float intervalMax, MyMethodDelegate myMethodDelegate, bool startInizilized = false)
        {
            float interval = Random.Range(intervalMin, intervalMax);

            if (!startInizilized)
            {
                yield return new WaitForSeconds(startDelay);
                myMethodDelegate();

            }
            else
            {
                yield return new WaitForSeconds(interval);
                myMethodDelegate();
            }
            numberSpawnAnimals++;
            if (numberSpawnAnimals <= levelManager.feedAnimalsGoal)
            {
                StartCoroutine(SpawnRandomAnimalCoroutine(0, intervalMin, intervalMax, SpawnRandomAnimal, true));
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