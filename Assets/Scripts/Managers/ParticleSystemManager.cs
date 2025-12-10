using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;

namespace FeedTheBeasts.Scripts
{
    public class ParticleSystemManager : MonoBehaviour
    {

        [SerializeField] UnityEngine.GameObject smokeWhenFed;
        [SerializeField] Transform objectPool;


        [SerializeField] int smokeWhenFedPoolLimit;

        List<UnityEngine.GameObject> listSmokeWhenFed;

        void Awake()
        {
            Assert.IsNotNull(smokeWhenFed, "ERROR: particle system not added");

            listSmokeWhenFed = new List<UnityEngine.GameObject>();
            for (int i = 1; i <= smokeWhenFedPoolLimit; i++)
            {
                AddParticleToPool();
            }

        }

        private void AddParticleToPool()
        {
            UnityEngine.GameObject newParticle = Instantiate(smokeWhenFed, objectPool); ;
            newParticle.SetActive(false);
            listSmokeWhenFed.Add(newParticle);
        }

        internal void SpawnParticles(Transform animalTransform)
        {
            Debug.Log("Is in spawn particles");
            int activeParticles = -1;
            foreach (var smoke in listSmokeWhenFed)
            {
                if (!smoke.activeSelf)
                {
                    smoke.SetActive(true);
                    Vector3 newPosition = new Vector3(animalTransform.position.x,
                                                      smokeWhenFed.transform.position.y,
                                                      animalTransform.position.z);
                    Debug.Log($"newPositionParticles: {newPosition}");
                    smoke.transform.position = newPosition;
                    StartCoroutine(SetInactiveCorroutime(smoke));

                    break;
                }
                else
                {
                    activeParticles++;
                    Debug.Log($"Active Particles : {activeParticles}");
                }
            }
            if (activeParticles == listSmokeWhenFed.Count)
            {
                AddParticleToPool();
                SpawnParticles(animalTransform);
            }
        }


        IEnumerator SetInactiveCorroutime(UnityEngine.GameObject smoke)
        {
            ParticleSystem particleSystem = smoke.GetComponent<ParticleSystem>();
            yield return new WaitForSeconds(particleSystem.main.startLifetimeMultiplier);
            smoke.SetActive(false);
        }

        
    }

}