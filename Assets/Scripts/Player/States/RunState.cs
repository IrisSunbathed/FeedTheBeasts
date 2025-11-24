
using UnityEngine;


namespace FeedTheBeasts.Scripts
{

    public class RunState : States
    {
        public float runSpeed;
        public override void Enter()
        {
            animator.SetFloat(Constants.ANIM_FLOAT_SPEED, Mathf.Max(Mathf.Abs(input.HorizontalInput), Mathf.Abs(input.VerticalInput)));
        }
        public override void Do()
        
        {
            // Debug.Log(Mathf.Max(Mathf.Abs(input.HorizontalInput), Mathf.Abs(input.VerticalInput)));
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
        }

    }

}