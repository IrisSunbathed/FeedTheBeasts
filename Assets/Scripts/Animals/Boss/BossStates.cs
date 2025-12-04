using System;
using UnityEngine;
using UnityEngine.AI;


namespace FeedTheBeasts.Scripts
{

    public abstract class BossStates : MonoBehaviour
    {
        public bool IsStateComplete { get; protected set; }
        protected Animator animator;
        protected Rigidbody rb;
        protected NavMeshAgent navMeshAgent;
        protected float startTime;
        float CurrentTime => Time.time - startTime;



        public void Setup(Rigidbody rb, Animator animator, NavMeshAgent navMeshAgent)
        {
            this.rb = rb;
            this.animator = animator;
            this.navMeshAgent = navMeshAgent;
        }
        public virtual void Enter()
        {

        }
        public virtual void Do()
        {
            

        }
        public virtual void FixedDo()
        {

        }
        public virtual void Exit()
        {
          
        }

    }

}