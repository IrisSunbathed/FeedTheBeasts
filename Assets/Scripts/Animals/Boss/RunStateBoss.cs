using FeedTheBeasts.Scripts;
using UnityEngine;
using UnityEngine.AI;

public class RunStateBoss : BossStates
{
    [SerializeField] Transform[] stopPoints;

    int indexPositions = -1;
    public override void Enter()
    {
        indexPositions++;
        Debug.Log(indexPositions);
        foreach (var item in stopPoints)
        {
            item.parent = null;
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
