using FeedTheBeasts.Scripts;
using NUnit.Framework;
using UnityEngine;

namespace FeedTheBeasts.Scripts
{

    [RequireComponent(typeof(AnimalHunger))]
    public class AnimalBubble : MonoBehaviour
    {

        [SerializeField] Transform foodContainer;
        AnimalHunger animalHunger;
        void Start()
        {
            animalHunger = GetComponent<AnimalHunger>();
            FoodTypes preferredFood = animalHunger.GetPreferredFood();

            GameObject goPreferredFood = GameCatalog.Instance.GetFoodGameObject(preferredFood);

            Instantiate(goPreferredFood, foodContainer);

        }

        void Awake()
        {
            Assert.IsNotNull(foodContainer, "ERROR: FoodContainer not added");
        }

     
    }

}