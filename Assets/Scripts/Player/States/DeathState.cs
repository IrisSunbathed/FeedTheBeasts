using UnityEngine;


namespace FeedTheBeasts.Scripts
{

    public class DeathState : States
    {
        public override void Enter()
        {
            animator.SetBool("death_b", true);
            int animation = Random.Range(1, 3);
            animator.SetInteger("deathType_int", animation);
        }
        public override void Do()
        {
            if (input.CanMove)
            {
                  animator.SetBool("death_b", false);
            }
        }
        public override void Exit()
        {
        }

    }

}