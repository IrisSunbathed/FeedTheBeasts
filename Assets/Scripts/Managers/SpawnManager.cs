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
        [SerializeField] UnityEngine.GameObject[] goPrefabs;
        [SerializeField] UnityEngine.GameObject goAggresiveAnimal;
        [Header("References")]
        [SerializeField] DifficultyManager difficultyManager;
        [SerializeField] UIManager uIManager;
        LevelManager levelManager;

        [SerializeField] bool spawnAnimals;
        [SerializeField] float stampedeTime;

        int numberSpawnAnimals;

        #region Cameras

        CamerasManager camerasManager;

        float[] extremePosition;
        float lengthCam;

        #endregion
        public event Action<UnityEngine.GameObject> OnAnimalSpawnEvent;

        int index;



        readonly float offset = 4f;

        UnityEngine.GameObject lastAnimalSpawned;
        UnityEngine.GameObject lastAggresiveAnimalSpawned;
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
            Assert.IsNotNull(uIManager, "ERROR: uIManager was not added to SpawnManager");
        }


        internal void Init()
        {
            numberSpawnAnimals = 0;
            ConfigureCameraExtremes();
            if (spawnAnimals)
            {
                StartCoroutine(StartCouroutines());
            }

            DestroyAnimals();


        }

        private void SpawnRandomAnimal()
        {
            numberSpawnAnimals++;
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
            numberSpawnAnimals++;
            float randomZValue = Random.Range(0, camerasManager.OrthographicSize);
            int randomXValue = Random.Range(0, 2);



            Vector3 spawnPosition = new Vector3(extremePosition[randomXValue],
                                                goPrefabs[index].transform.position.y,
                                                randomZValue);

            lastAggresiveAnimalSpawned = Instantiate(goAggresiveAnimal, spawnPosition, goPrefabs[index].transform.rotation);

            OnAnimalSpawnEvent?.Invoke(lastAggresiveAnimalSpawned);
        }

        internal void StopSpawning(bool destroy = true)
        {
            numberSpawnAnimals = 0;
            StopAllCoroutines();
            coroutineAnimals = null;
            coroutineAggressiveAnimals = null;
            if (destroy)
            {
                DestroyAnimals();
            }

        }

        internal void DestroyAnimals()
        {
            foreach (var animal in UnityEngine.GameObject.FindGameObjectsWithTag(Constants.ANIMAL_TAG))
            {
                Vector3 bounds = animal.GetComponent<MeshRenderer>().bounds.max;
                var result = camerasManager.IsOutOfBounds(animal.transform.position, bounds);
                if (result.Item2)
                {
                    Destroy(animal);
                }
            }
        }

        internal void Stampede(int numberOfAnimals)
        {
            StopAllCoroutines();
            StartCoroutine(StampedeWarningCoroutine(numberOfAnimals));

        }

        IEnumerator StampedeWarningCoroutine(int numberOfAnimals)
        {
            uIManager.InGameWarning(stampedeTime, Constants.STAMPEDE_TEXT);
            yield return new WaitForSeconds(stampedeTime);
            coroutineAnimals = null;
            coroutineAggressiveAnimals = null;

            if (numberOfAnimals > levelManager.AnimalsLeft)
            {
                numberOfAnimals = levelManager.AnimalsLeft;
            }

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

            levelManager.AnimalsLeft = levelManager.feedAnimalsGoal - numberSpawnAnimals;
            Debug.Log($"spawn animals: {numberSpawnAnimals} spawn animals per level: {levelManager.LevelAnimalGoal}");
            if (numberSpawnAnimals < levelManager.LevelAnimalGoal)
            {
                if (levelManager.LevelAnimalGoal - numberSpawnAnimals == 5)
                {
                    Stampede(5);
                }
                else
                {
                    StartCoroutine(SpawnRandomAnimalCoroutine(0, intervalMin, intervalMax, SpawnRandomAnimal, true));
                }
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