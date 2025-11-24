using UnityEngine;


namespace FeedTheBeasts.Scripts
{
    public class IdleState : States
    {

        public override void Enter()
        {
            animator.SetBool(Constants.ANIM_BOOL_DEATH, false);
            animator.SetInteger(Constants.ANIM_INT_IDLE, 1);
        }
        public override void Do()
        {
            if (input.HorizontalInput != 0 | input.VerticalInput != 0)
            {
                animator.SetInteger(Constants.ANIM_INT_IDLE, 0);
                IsStateComplete = true;
            }

        }

        public override void Exit()
        {
        }

    }

}