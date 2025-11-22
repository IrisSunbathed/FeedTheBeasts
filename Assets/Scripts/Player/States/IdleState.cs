using UnityEngine;


namespace FeedTheBeasts.Scripts
{
    public class IdleState : States
    {

        public override void Enter()
        {
            animator.SetInteger("Animation_int", 1);
        }
        public override void Do()
        {
            if (input.HorizontalInput != 0 | input.VerticalInput != 0)
            {
                animator.SetInteger("Animation_int", 0);
                IsStateComplete = true;
            }

        }

        public override void Exit()
        {
        }

    }

}