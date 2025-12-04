using System;
using System.Collections;
using FeedTheBeasts.Scripts;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class IdleStateBoss : BossStates
{
    [SerializeField] GameObject goChilds;

    // [SerializeField] int summonRounds;
    [SerializeField, Range(5f, 15f)] float summonIntervalMin;
    [SerializeField, Range(10f, 25f)] float summonIntervalMax;
    [SerializeField] Transform[] spawnPositions;
    public event Action<DestroyOutOfBounds, bool> OnSpawnEvent;

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

    IEnumerator SummonCouroutine()
    {

        for (int i = 0; i < spawnPositions.Length; i++)
        {
            GameObject newDoe = Instantiate(goChilds, spawnPositions[i].position, quaternion.identity);
            DestroyOutOfBounds destroyOutOfBounds = newDoe.GetComponent<DestroyOutOfBounds>();
            OnSpawnEvent?.Invoke(destroyOutOfBounds, true);

        }
        float summonInterval = Random.Range(summonIntervalMin, summonIntervalMax);
        yield return new WaitForSeconds(summonInterval);

        StartCoroutine(SummonCouroutine());
    }
}
