using System;
using UnityEngine;


namespace FeedTheBeasts.Scripts
{

    public abstract class States : MonoBehaviour
    {
        public bool IsStateComplete { get; protected set; }
        protected Animator animator;

        protected PlayerController input;

        protected Rigidbody rb;
        protected float startTime;
        float CurrentTime => Time.time - startTime;
      


        public void Setup(Rigidbody rb, Animator animator, PlayerController input)
        {
            this.rb = rb;
            this.animator = animator;
            this.input = input;
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