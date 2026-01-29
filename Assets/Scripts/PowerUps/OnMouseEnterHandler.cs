using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FeedTheBeasts.Scripts
{
    [RequireComponent(typeof(PowerUpProperties))]
    public class OnMouseEnterHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {

        public event Action<string, PowerUpProperties> OnHoverEvent;
        public event Action<PowerUps> OnClickEvent;

        PowerUpProperties powerUpProperties;

        void Awake()
        {
            powerUpProperties = GetComponent<PowerUpProperties>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnHoverEvent?.Invoke(powerUpProperties.description, powerUpProperties);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnHoverEvent?.Invoke(Constants.POWER_UP_HOVER_EXIT_TEXT, null);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //Get Type

            //Send event to PowerUpsManager
            OnClickEvent?.Invoke(powerUpProperties.powerUps);
            /////switch
            ////"Test X applied"
        }
    }

}
