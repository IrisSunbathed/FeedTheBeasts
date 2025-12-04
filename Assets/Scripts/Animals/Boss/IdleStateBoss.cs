using System;
using System.Collections;
using FeedTheBeasts.Scripts;
using Unity.Mathematics;
using UnityEngine;

public class IdleStateBoss : BossStates
{
    [SerializeField] GameObject goChilds;

    // [SerializeField] int summonRounds;
    [SerializeField] float summonInterval;
    [SerializeField] Transform[] spawnPositions;
    public event Action<DestroyOutOfBounds, bool> OnSpawnEvent;
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

        yield return new WaitForSeconds(summonInterval);

        StartCoroutine(SummonCouroutine());
    }
}
