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
            Debug.Log($"Animator paused. Speed: {animator.speed}");
            if (GameStage.gameStageEnum == GameStageEnum.Paused)
            {
                animator.speed = 0;
                return;
            }
            if (GameStage.gameStageEnum != GameStageEnum.Paused)
            {
                animator.speed = 1;
                return;
            }

        }
        public virtual void FixedDo()
        {

        }
        public virtual void Exit()
        {

        }

    }

}