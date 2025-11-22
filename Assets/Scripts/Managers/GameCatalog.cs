using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


namespace FeedTheBeasts.Scripts
{

    public class GameCatalog : MonoBehaviour
    {
        [SerializeField] FoodItemTransparent[] foodItemTransparent;
        static GameCatalog instance;
        static public GameCatalog Instance => instance;

        Dictionary<FoodTypes, FoodItemTransparent> foodTypeToItem;


        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            SetUpFoodDictionary();
        }

        void SetUpFoodDictionary()
        {
            Assert.IsTrue(foodItemTransparent.Length > 0, "ERROR:Food Item not added to array");
            foodTypeToItem = new Dictionary<FoodTypes, FoodItemTransparent>();
            foreach (var item in foodItemTransparent)
            {
                if (!foodTypeToItem.ContainsKey(item.foodTypes))
                {
                    foodTypeToItem.Add(item.foodTypes, item);
                }
                else
                {
                    Debug.LogWarning("WARNING: dupliate in the array");
                }
            }
        }

        internal GameObject GetFoodGameObject(FoodTypes preferredFood)
        {
            return foodTypeToItem[preferredFood].goFood;
        }
    }

}