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

        [SerializeField] GameObject smokeWhenFed;
        [SerializeField] GameObject smokeHit;

        [SerializeField] Transform objectPool;


        int canvasIndex;
        [SerializeField] int smokeWhenFedPoolLimit;
        [SerializeField] int smokeWhenHitPoolLimit;

        List<GameObject> listSmokeWhenFed;
        List<UnityEngine.GameObject> listHitWhenFed;


        void Awake()
        {
            Assert.IsNotNull(smokeWhenFed, "ERROR: particle system not added");

            listSmokeWhenFed = new List<GameObject>();
            canvasIndex = 0;
            listSmokeWhenFed = new List<UnityEngine.GameObject>();
            listHitWhenFed = new List<GameObject>();

            for (int i = 1; i <= smokeWhenFedPoolLimit; i++)
            {
                AddSmokeParticleToPool();
            }
            for (int i = 1; i <= smokeWhenHitPoolLimit; i++)
            {
                AddHitParticleToPool();
            }

        }

        private void AddSmokeParticleToPool()
        {
            GameObject newParticle = Instantiate(smokeWhenFed, objectPool); ;
            newParticle.SetActive(false);
            listSmokeWhenFed.Add(newParticle);
        }
        private void AddHitParticleToPool()
        {
            UnityEngine.GameObject newParticle = Instantiate(smokeHit, objectPool); ;
            newParticle.SetActive(false);
            listHitWhenFed.Add(newParticle);
        }

        internal void SpawnFedParticles(Transform animalTransform)
        {
            int activeParticles = -1;
            foreach (var smoke in listSmokeWhenFed)
            {
                if (!smoke.activeSelf)
                {
                    smoke.SetActive(true);
                    Vector3 newPosition = new Vector3(animalTransform.position.x,
                                                      smokeWhenFed.transform.position.y,
                                                      animalTransform.position.z);
                    smoke.transform.position =newPosition;
                    StartCoroutine(SetInactiveCorroutime(smoke));
                    
                    break;
                }
                else
                {
                    activeParticles++;
                }
            }
            if (activeParticles == listSmokeWhenFed.Count)
            {
                AddSmokeParticleToPool();
                SpawnFedParticles(animalTransform);

            }
        }
          internal void SpawnHitParticles(Transform bulletTransform)
        {
            int activeParticles = -1;
            foreach (var smoke in listHitWhenFed)
            {
                if (!smoke.activeSelf)
                {
                    smoke.SetActive(true);
                    Vector3 newPosition = new Vector3(bulletTransform.position.x,
                                                      smokeWhenFed.transform.position.y,
                                                      bulletTransform.position.z);
                    smoke.transform.position = newPosition;
                    smoke.transform.Rotate(0, bulletTransform.rotation.y - 180, 0);
                    StartCoroutine(SetInactiveCorroutime(smoke));
                    break;
                }
                else
                {
                    activeParticles++;
                }
            }
            if (activeParticles == listSmokeWhenFed.Count)
            {
                AddSmokeParticleToPool();
                SpawnFedParticles(bulletTransform);
            }
        }





        IEnumerator SetInactiveCorroutime(GameObject smoke)
        {
            ParticleSystem particleSystem = smoke.GetComponent<ParticleSystem>();
            yield return new WaitForSeconds(particleSystem.main.startLifetimeMultiplier);
            smoke.SetActive(false);
        }

        
    }

}