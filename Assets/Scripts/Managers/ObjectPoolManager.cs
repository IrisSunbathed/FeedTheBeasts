using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace FeedTheBeasts.Scripts
{
    
public class ObjectPoolManager : MonoBehaviour
{

        Dictionary<FoodTypes, ObjectPool<UnityEngine.GameObject>> foodTypeToObjectPool;
        List<ObjectPool<UnityEngine.GameObject>> objectPools;

        internal void InizialiceDictionary(FoodTypes foodType, UnityEngine.GameObject food)
        {
            foodTypeToObjectPool = new Dictionary<FoodTypes, ObjectPool<UnityEngine.GameObject>>();
        }

        void Awake()
        {
            
        }

        

        // Update is called once per frame
        void Update()
    {

    }
}

}