using System;
using System.Collections;
using System.Collections.Generic;
using FeedTheBeasts.Scripts;
using NUnit.Framework;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;
using RangeAttribute = UnityEngine.RangeAttribute;

public class IdleStateBoss : BossStates
{
    [SerializeField] GameObject goChilds;

    // [SerializeField] int summonRounds;
    [SerializeField, Range(1f, 15f)] float summonIntervalMin;
    [SerializeField, Range(5f, 25f)] float summonIntervalMax;
    [SerializeField] Transform[] spawnPositions;
    [SerializeField] WorldManager worldManager;
    public event Action<DestroyOutOfBounds, bool, AnimalHunger> OnSpawnEvent;
    int randomPositionInt = -1;



    void OnValidate()
    {
        summonIntervalMin = Mathf.Clamp(summonIntervalMin, 5f, summonIntervalMax);
        summonIntervalMax = Mathf.Clamp(summonIntervalMax, summonIntervalMin, 25f);
    }
    public override void Enter()
    {
        navMeshAgent.isStopped = true;
        animator.SetBool(Constants.ANIM_BOOL_EAT, true);
        animator.SetFloat(Constants.ANIM_FLOAT_SPEED, 0);
        TrySpawn();
    }

    public override void Exit()
    {

        IsStateComplete = true;
        StopAllCoroutines();
        GameObject[] goDoes = GameObject.FindGameObjectsWithTag(Constants.ANIMAL_TAG);
        if (goDoes.Length > 0)
        {

            for (int i = 0; i < goDoes.Length; i++)
            {
                if (goDoes[i].TryGetComponent(out Animal agent))
                {
                    agent.navMeshAgent.isStopped = true;
                    int destIndex = Mathf.Clamp(i, 0, spawnPositions.Length - 1);
                    agent.animalStatus = AnimalStatus.Returning; 
                    agent.SetDestination(spawnPositions[destIndex].position.x,
                                         spawnPositions[destIndex].position.y,
                                         spawnPositions[destIndex].position.z);
                    agent.navMeshAgent.isStopped = false;
                }
            }

        }

    }

    IEnumerator SummonCouroutine()
    {
        //List<int> = usedPositions.Add(randomPositionInt);

        int randomNumber = Random.Range(2, spawnPositions.Length);
        List<int> randomPositions = new List<int>();

        for (int i = 0; i < randomNumber; i++)
        {
            StartCoroutine(ConfigRandomPositionCoroutine(randomNumber, randomPositions));
        }
        randomPositions.Clear();
        float summonInterval = Random.Range(summonIntervalMin, summonIntervalMax);
        yield return new WaitForSeconds(summonInterval);
        TrySpawn();
    }

    private void TrySpawn()
    {
        GameObject[] goDoes = GameObject.FindGameObjectsWithTag(Constants.ANIMAL_TAG);
        if (goDoes.Length < 4)
        {
            StartCoroutine(SummonCouroutine());
        }
        else
        {
            StartCoroutine(WaitingCoroutine());
        }
    }

    IEnumerator WaitingCoroutine()
    {
        yield return new WaitForSeconds(.5f);
        TrySpawn();
    }

    IEnumerator ConfigRandomPositionCoroutine(int randomNumber, List<int> randomPositions)
    {
        randomPositionInt = Random.Range(0, randomNumber);
        if (!randomPositions.Contains(randomPositionInt))
        {
            randomPositions.Add(randomPositionInt);
            GameObject newDoe = Instantiate(goChilds, spawnPositions[randomPositionInt].position, quaternion.identity);
            DestroyOutOfBounds destroyOutOfBounds = newDoe.GetComponent<DestroyOutOfBounds>();
            OnSpawnEvent?.Invoke(destroyOutOfBounds, true, newDoe.GetComponent<AnimalHunger>());
        }
        else
        {
            StartCoroutine(ConfigRandomPositionCoroutine(randomNumber, randomPositions));
        }
        yield return null;
    }

    private void GetRandomPosition(int randomNumber, List<int> randomPositions)
    {
        randomPositionInt = Random.Range(0, randomNumber);
        if (!randomPositions.Contains(randomPositionInt)) return;
        else
        {
            GetRandomPosition(randomNumber, randomPositions);
        }


    }
}
