using System;
using FeedTheBeasts.Scripts;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class BossController : Animal
{

    internal BossStates bossStates;
    public RunStateBoss runStateBoss;
    public IdleStateBoss idleStateBoss;
    //  public CallStateBoss callStateBoss;
    Animator animBoss;

    bool isFed;



    void Awake()
    {
        animBoss = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        SetUpStates();
    }

    private void SetUpStates()
    {
        runStateBoss.Setup( animBoss, navMeshAgent);
        idleStateBoss.Setup( animBoss, navMeshAgent);
        bossStates = runStateBoss;
        bossStates.Enter();
    }


    // Update is called once per frame
    public override void Update()
    {

        if (bossStates.IsStateComplete)
        {
            SelectState();
        }
        bossStates.Do();

        Debug.Log(bossStates.name);

    }

    private void SelectState()
    {

        if (!isFed)
        {
            bossStates = idleStateBoss;

        }
        else
        {
            bossStates = runStateBoss;

        }
        bossStates.Enter();
    }

    internal void IsFed()
    {
        isFed = true;
        bossStates.Exit();
    }
}
