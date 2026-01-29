using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace FeedTheBeasts.Scripts
{
    [RequireComponent(typeof(Image))]
    public class PowerUpProperties : MonoBehaviour
    {
        internal string description;
        Image image;

        internal PowerUps powerUps;

        void Awake()
        {
            image = GetComponent<Image>();
        }
        internal void SetUp(string description, Sprite sprite, PowerUps powerUps)
        {
            this.description = description;
            image.sprite = sprite;
            this.powerUps = powerUps;
        }
    }
}
