using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Audio;


namespace FeedTheBeasts.Scripts
{

    public class GameCatalog : MonoBehaviour
    {
        static GameCatalog instance;
        static public GameCatalog Instance => instance;
        [SerializeField] FoodItemTransparent[] foodItemTransparent;
        [SerializeField] MusicItem[] musicItems;
        [SerializeField] FXItem[] fxItems;
        [SerializeField] PowerUpsItem[] powerUpsItems;

        Dictionary<FoodTypes, FoodItemTransparent> foodTypeToItem;
        Dictionary<MusicThemes, MusicItem> musicThemeToItem;
        Dictionary<FXTypes, FXItem> fxToItem;
        Dictionary<PowerUps, PowerUpsItem> powerUpToItem;


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
            SetUpMusicDictionary();
            SetUpFXDictionary();
            SetUpPowerUpDictionary();
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
                    Debug.LogWarning("WARNING: duplicate in the array");
                }
            }
        }
        void SetUpMusicDictionary()
        {
            Assert.IsTrue(musicItems.Length > 0, "ERROR:music items not added to array");
            musicThemeToItem = new Dictionary<MusicThemes, MusicItem>();
            foreach (var item in musicItems)
            {
                if (!musicThemeToItem.ContainsKey(item.musicThemes))
                {
                    musicThemeToItem.Add(item.musicThemes, item);
                }
                else
                {
                    Debug.LogWarning("WARNING: dupliate in the array");
                }
            }
        }
        void SetUpFXDictionary()
        {
            Assert.IsTrue(fxItems.Length > 0, "ERROR:fxItems not added to array");
            fxToItem = new Dictionary<FXTypes, FXItem>();
            foreach (var item in fxItems)
            {
                if (!fxToItem.ContainsKey(item.fxType))
                {
                    fxToItem.Add(item.fxType, item);
                }
                else
                {
                    Debug.LogWarning("WARNING: duplicate in the array");
                }
            }
        }
        void SetUpPowerUpDictionary()
        {
            Assert.IsTrue(powerUpsItems.Length > 0, "ERROR:powerUpsItems not added to array");
            powerUpToItem = new Dictionary<PowerUps, PowerUpsItem>();
            foreach (var item in powerUpsItems)
            {
                if (!powerUpToItem.ContainsKey(item.powerUpItem))
                {
                    powerUpToItem.Add(item.powerUpItem, item);
                }
                else
                {
                    Debug.LogWarning("WARNING: duplicate in the array");
                }
            }
        }
        internal GameObject GetFoodGameObject(FoodTypes preferredFood)
        {
            return foodTypeToItem[preferredFood].goFood;
        }


        internal AudioClip GetAudioClip(MusicThemes musicThemes)
        {
            return musicThemeToItem[musicThemes].audioClip;
        }

        internal AudioClip GetFXClip(FXTypes fxType)
        {
            return fxToItem[fxType].audioClip;
        }

        internal (string, Sprite) GetPowerUpProperties(PowerUps randomPowerUp)
        {
            return (powerUpToItem[randomPowerUp].description, powerUpToItem[randomPowerUp].sprite);
        }

        internal bool CanPowerUpBeStacked(PowerUps randomPowerUp)
        {
            return powerUpToItem[randomPowerUp].canBeStacked;
        }
    }

}