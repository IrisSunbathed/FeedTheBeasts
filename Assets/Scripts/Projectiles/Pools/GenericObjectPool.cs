using System;
using UnityEngine;

namespace FeedTheBeasts.Scripts
{
    public class GenericObjectPool : MonoBehaviour
    {
        [SerializeField, Range(10, 100)] protected int defaultCapacity;
        [SerializeField, Range(100, 200)] protected int maxPoolSize;
        [SerializeField, Range(0.01f, 0.1f)] protected float timeTrailReactivate;

        internal virtual void EnableComponents(GameObject projectile, bool areActive)
        { }
    }



}

