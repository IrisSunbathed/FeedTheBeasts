using System;
using FeedTheBeasts.Scripts;
using NUnit.Framework;
using UnityEngine;
using RangeAttribute = UnityEngine.RangeAttribute;

namespace FeedTheBeasts.Scripts
{
    [RequireComponent(typeof(MeshRenderer))]
    public class DestroyOutOfBounds : MonoBehaviour
    {

        [SerializeField, Range(0.5f, 2f)] float offset;
        CamerasManager camerasManager;

        float bounds;
        public event Action<bool> OnLoseLifeEvent;
        bool flag;

        void Start()
        {
            camerasManager = CamerasManager.Instance;
        }
        void Awake()
        {
            flag = false;
          
        }
        void Update()
        {
            if (transform.position.z + offset< -camerasManager.OrthographicSize  & !flag)
            {
                flag = true;
                OnLoseLifeEvent?.Invoke(true);
                Destroy(gameObject);
            }
        }
    }

}