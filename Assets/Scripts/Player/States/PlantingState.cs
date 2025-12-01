using System;
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
         //   foodSelectorManager.TryShootCurrentWeapon(transform.position);
            input.CanMove = false;
        }

        public override void Exit()
        {
            input.CanMove = true;
            startTime = 0;
            OnCancelledAction.Invoke();
            IsStateComplete = true;
        }
    }

}