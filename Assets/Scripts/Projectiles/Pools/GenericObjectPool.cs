using System;
using UnityEngine;

namespace FeedTheBeasts.Scripts
{
    public class GenericObjectPool : MonoBehaviour
    {
        [SerializeField, Range(10, 100)] protected int defaultCapacity;
        [SerializeField, Range(100, 200)] protected int maxPoolSize;
    }



}

