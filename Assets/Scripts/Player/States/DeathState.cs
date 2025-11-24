using UnityEngine;


namespace FeedTheBeasts.Scripts
{

    public class DeathState : States
    {
      

        public override void Enter()
        {
            animator.SetBool(Constants.ANIM_BOOL_DEATH, true);
            int animation = Random.Range(1, 3);
            animator.SetInteger(Constants.ANIM_INT_DEATHTYPE, animation);
        }
        public override void Do()
        {
            input.SetOriginalPosition();
            if (input.CanMove)
            {
                animator.SetBool(Constants.ANIM_BOOL_DEATH, false);
            }
        }
        public override void Exit()
        {
        }

    }

}