
using UnityEngine;


namespace FeedTheBeasts.Scripts
{

    public class RunState : States
    {

        public float runSpeed;
        public override void Enter()
        {
            animator.SetFloat("Speed_f", Mathf.Max(Mathf.Abs(input.HorizontalInput), Mathf.Abs(input.VerticalInput)));
        }
        public override void Do()
        {
            animator.SetFloat("Speed_f", Mathf.Max(Mathf.Abs(input.HorizontalInput), Mathf.Abs(input.VerticalInput)));
            if (input.HorizontalInput == 0 | input.VerticalInput == 0)
            {
                animator.SetFloat("Speed_f", 0);
                IsStateComplete = true;
            }
        }
        public override void Exit()
        {
        }

    }

}