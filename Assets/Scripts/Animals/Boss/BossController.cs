using System;
using FeedTheBeasts.Scripts;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class BossController : MonoBehaviour
{

    BossStates bossStates;
    public RunStateBoss runStateBoss;
    public IdleStateBoss idleStateBoss;
    //  public CallStateBoss callStateBoss;
    Rigidbody rbBoss;
    Animator animBoss;

    NavMeshAgent navMeshAgent;
    bool isFed;



    void Awake()
    {
        rbBoss = GetComponent<Rigidbody>();
        animBoss = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        SetUpStates();
    }

    private void SetUpStates()
    {
        runStateBoss.Setup(rbBoss, animBoss, navMeshAgent);
        idleStateBoss.Setup(rbBoss, animBoss, navMeshAgent);
        bossStates = runStateBoss;
        bossStates.Enter();
    }


    // Update is called once per frame
    void Update()
    {

        if (bossStates.IsStateComplete)
        {
            SelectState();
        }
        bossStates.Do();

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
