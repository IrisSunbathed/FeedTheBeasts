
using System;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;


namespace FeedTheBeasts.Scripts
{

    public class RunState : States
    {
        public float runSpeed;
        float currentsSpeed;
        internal float currentRunSpeed;

        [SerializeField, Range(5, 10)] float addedSpeedPowerUp;

        void Start()
        {
            SetDeafultSpeed();
        }

        internal void SetDeafultSpeed()
        {
            currentsSpeed = runSpeed;
        }

        public override void Enter()
        {
            animator.SetBool(Constants.ANIM_BOOL_DEATH, false);
            currentRunSpeed = currentsSpeed;
            animator.SetFloat(Constants.ANIM_FLOAT_SPEED, Mathf.Max(Mathf.Abs(input.HorizontalInput), Mathf.Abs(input.VerticalInput)));
        }
        public override void Do()

        {
            base.Do();
            animator.SetInteger(Constants.ANIM_INT_IDLE, 0);
            animator.SetFloat(Constants.ANIM_FLOAT_SPEED, Mathf.Max(Mathf.Abs(input.HorizontalInput), Mathf.Abs(input.VerticalInput)));
            if (input.HorizontalInput == 0 & input.VerticalInput == 0)
            {
                animator.SetFloat(Constants.ANIM_FLOAT_SPEED, 0);
                IsStateComplete = true;
            }
        }
        public override void Exit()
        {
            animator.SetFloat(Constants.ANIM_FLOAT_SPEED, 0);
        }

        internal void IncreaseMovementSpeed()
        {
            currentsSpeed += addedSpeedPowerUp;
        }
    }

}