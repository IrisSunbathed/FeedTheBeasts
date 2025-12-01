using System;
using UnityEditor.ShaderGraph.Internal;

namespace FeedTheBeasts.Scripts
{
    internal interface IPlantable
    {
        public event Action<float> OnPlantEvent;
        public float PlantingTime { get; set; }
        void TryPlant();
    }
}