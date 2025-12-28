using System;
using System.Collections;
using FeedTheBeasts.Scripts;
using Unity.VisualScripting;
using UnityEngine;

namespace FeedTheBeasts.Scripts
{


    public class PlantingState : States
    {
        [SerializeField] FoodSelectorManager foodSelectorManager;

        public event Action OnCancelledAction;
        public override void Enter()
        {
            if (input.runState != null)
            {
                input.runState.Exit();
           }
            IsStateComplete = false;
            input.CanMove = false;
            StartCoroutine(LookingDownCoroutine(animator.GetFloat(Constants.ANIM_BODY_VERTICAL)));
            animator.SetInteger(Constants.ANIM_INT_IDLE, 5);
        }

        IEnumerator LookingDownCoroutine(float currentPosition)
        {
            while (currentPosition > -1)
            {
                currentPosition -= 0.01f;
                animator.SetFloat(Constants.ANIM_BODY_VERTICAL, currentPosition);
                yield return null;
            }
            animator.SetFloat(Constants.ANIM_BODY_VERTICAL, -1);
        }


        IEnumerator LookingUpCoroutine(float currentPosition)
        {
            while (currentPosition < 0)
            {
                currentPosition += 0.01f;
                animator.SetFloat(Constants.ANIM_BODY_VERTICAL, currentPosition);
                yield return null;
            }
            animator.SetFloat(Constants.ANIM_BODY_VERTICAL, 0);
       
        }

        public override void Exit()
        {
            Debug.Log("Exiting Plating state");
            //  animator.SetFloat(Constants.ANIM_BODY_VERTICAL, 0);
            StartCoroutine(LookingUpCoroutine(animator.GetFloat(Constants.ANIM_BODY_VERTICAL)));
            animator.SetInteger(Constants.ANIM_INT_IDLE, 0);
            input.CanMove = true;
            OnCancelledAction.Invoke();
            IsStateComplete = true;
        }
    }

}