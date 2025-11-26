using FeedTheBeasts.Scripts;
using UnityEngine;

public class WinState : States
{
    public override void Enter()
    {
        animator.SetInteger(Constants.ANIM_INT_IDLE, 4);
    }
    public override void Do()
    {
        input.SetOriginalPosition();
        if (input.CanMove)
        {
            animator.SetInteger(Constants.ANIM_INT_IDLE, 1);
        }
    }

    public override void Exit()
    {

    }
}
