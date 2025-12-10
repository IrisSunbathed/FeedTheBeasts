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
        StartCoroutine(SummonCouroutine());
    }

    public override void Exit()
    {

        IsStateComplete = true;
        StopAllCoroutines();
        GameObject[] goDoes = GameObject.FindGameObjectsWithTag(Constants.ANIMAL_TAG);
        if (goDoes.Length > 0)
        {
            foreach (var item in goDoes)
            {
                if (item.TryGetComponent(out Animal agent))
                {
                    agent.SetEatingAnimation();

                }
            }

        }

    }

    IEnumerator SummonCouroutine()
    {
        //randonNumber = take a random number between 1 and 4 -> number of spawn positions active
        //for change i < randonNumber
        //spawnPosition.position also random

        //randomPositionInt = Random.Range(0, randomNumber)
        //if usedPositions.Contains(randomPositionInt) -> randomPositionInt = Random.Range(0, randomNumber)
        //List<int> = usedPositions.Add(randomPositionInt);

        int randomNumber = Random.Range(2, spawnPositions.Length);
        List<int> randomPositions = new List<int>();

        for (int i = 0; i < randomNumber; i++)
        {
            StartCoroutine(CofigRandomPositionCoroutine(randomNumber, randomPositions));
        }
        randomPositions.Clear();
        float summonInterval = Random.Range(summonIntervalMin, summonIntervalMax);
        yield return new WaitForSeconds(summonInterval);

        StartCoroutine(SummonCouroutine());
    }

    IEnumerator CofigRandomPositionCoroutine(int randomNumber, List<int> randomPositions)
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
            StartCoroutine(CofigRandomPositionCoroutine(randomNumber, randomPositions));
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
