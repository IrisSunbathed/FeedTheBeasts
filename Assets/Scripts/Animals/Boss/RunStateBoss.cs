using System;
using FeedTheBeasts.Scripts;
using UnityEngine;
using UnityEngine.AI;

public class RunStateBoss : BossStates
{
    [SerializeField] Transform[] stopPoints;

    int indexPositions = -1;
    public override void Enter()
    {
        IsStateComplete = false;
        navMeshAgent.isStopped = false;
        indexPositions++;
        animator.SetBool(Constants.ANIM_BOOL_EAT, false);
        animator.SetFloat(Constants.ANIM_FLOAT_SPEED, 1);
        if (indexPositions > stopPoints.Length - 1)
        {
            indexPositions = stopPoints.Length - 1;
        }
        foreach (var item in stopPoints)
        {
            item.transform.parent = null;
        }
        navMeshAgent.SetDestination(stopPoints[indexPositions].position);
    }
    public override void Do()
    {
        if (transform.position.z <= stopPoints[indexPositions].position.z)
        {
            IsStateComplete = true;

        }
    }

    public override void Exit()
    {

    }
}
