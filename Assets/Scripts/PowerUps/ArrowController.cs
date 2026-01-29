using System;
using DG.Tweening;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

namespace FeedTheBeasts.Scripts
{

    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Image))]
    public class ArrowController : MonoBehaviour
    {

        [SerializeField, Range(75, 150)] float yGoal;
        [SerializeField, Range(0.5f, 1.5f)] float duration;
        RectTransform rectTransform;
        Image image;


        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            image = GetComponent<Image>();

            float goalPosition = rectTransform.anchoredPosition.y + yGoal;

            rectTransform.DOAnchorPosY(goalPosition, duration);
            Color imageEndValue = new Color(image.color.r, image.color.g, image.color.b, 0);

            image.DOColor(imageEndValue, duration).OnComplete(DestroySelf);


        }

        private void DestroySelf()
        {
            Destroy(gameObject);
        }
    }

}