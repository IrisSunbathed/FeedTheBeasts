using System;
using System.Collections.Generic;
using DG.Tweening;
using NUnit.Framework;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using RangeAttribute = UnityEngine.RangeAttribute;

namespace FeedTheBeasts.Scripts
{



    public class UpdateMenuUIManager : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField, Range(3, 5)] int powerUpsShown;
        [SerializeField, Range(15, 30)] float powerUpMaxRotation;
        [SerializeField, Range(.01f, .05f)] float timeTweenRotation;

        [SerializeField, RangeAttribute(3, 10)] int minArrows;
        [SerializeField, RangeAttribute(10, 20)] int maxArrows;

        [SerializeField] RectTransform canvas;
        [SerializeField] GameObject upArrow;

        RectTransform selectedTransform;


        [Header("Required UI components")]
        [SerializeField] TMP_Text tmp_description;
        [SerializeField] RectTransform powerUpLayout;
        [SerializeField] GameObject background;
        [SerializeField] GameObject powerUpUITemplate;
        [SerializeField] GameObject VerticalLayout;

        // [SerializeField] Button bttTestActivate;

        [Header("References")]

        [SerializeField] PowerUpsManager powerUpsManager;
        [SerializeField] WorldManager worldManager;

        [SerializeField] FXSoundsManager fXSoundsManager;

        GameCatalog gameCatalog;

        List<int> randomPowerUps;
        List<GameObject> instantiatedPowerUps;

        int powerUpCount;


        void Start()
        {
            gameCatalog = GameCatalog.Instance;
            randomPowerUps = new List<int>();
            powerUpCount = Enum.GetNames(typeof(PowerUps)).Length;


        }
        void Awake()
        {
            Assert.IsNotNull(tmp_description, "ERROR: tmp_description is empty");
            Assert.IsNotNull(powerUpLayout, "ERROR: powerUpLayout is empty");
            Assert.IsNotNull(powerUpUITemplate, "ERROR: powerUpUITemplate is empty");
            Assert.IsNotNull(VerticalLayout, "ERROR: VerticalLayout is empty");
            Assert.IsNotNull(powerUpsManager, "ERROR: powerUpsManager is empty");
            //Assert.IsNotNull(bttTestActivate, "ERROR: bttTestActivate is empty");
            Assert.IsNotNull(worldManager, "ERROR: worldManager is empty");
            // bttTestActivate.onClick.AddListener(SetActivePowerUpMenu);

        }

        // void Update()
        // {
        //     // if (Input.GetKeyDown(KeyCode.U))
        //     // {
        //     //     SetActivePowerUpMenu();
        //     // }
        // }

        internal void SetActivePowerUpMenu()
        {
            tmp_description.transform.parent.gameObject.SetActive(true);
            VerticalLayout.SetActive(true);
            powerUpLayout.gameObject.SetActive(true);
            background.SetActive(true);
            instantiatedPowerUps = new List<GameObject>();
            SpawnArrows();
            tmp_description.text = Constants.POWER_UP_HOVER_EXIT_TEXT;

            for (int i = 0; i < powerUpsShown; i++)
            {
                InstantiateRandomPowerUp();
            }
        }

        private void SpawnArrows()
        {
            float numberArrows = Random.Range(minArrows, maxArrows);
            for (int i = 1; i < numberArrows; i++)
            {
                SpawnArrow();
            }
        }

        private void SpawnArrow()
        {
            float randomX = Random.Range(-300, 300);
            float randomY = Random.Range(-150, 150);

            GameObject newArrow = Instantiate(upArrow, canvas);

            RectTransform rectTransform = newArrow.GetComponent<RectTransform>();

            rectTransform.anchoredPosition = new Vector2(randomX, randomY);
        }

        private void InstantiateRandomPowerUp()
        {
            int random;
            int safety = 0;
            do
            {
                if (safety == 9999)
                {
                    Debug.LogWarning("Safety warning in PowerUps Instatioation");
                    return;
                }
                random = Random.Range(0, powerUpCount);
                safety++;


                //Is The PowerUp Activated And then can it be stacked
                //OR
                //IsAlreadyInTheListOfPowerUps?

            } while (powerUpsManager.CanPowerUpBeStacked((PowerUps)random) || randomPowerUps.Contains(random));

            randomPowerUps.Add(random);
            PowerUps randomPowerUp = (PowerUps)random;
            var powerUpProp = gameCatalog.GetPowerUpProperties((PowerUps)random);
            //Configure PowerUp
            GameObject newPowerUpUI = Instantiate(powerUpUITemplate, powerUpLayout);
            instantiatedPowerUps.Add(newPowerUpUI);
            OnMouseEnterHandler onMouseEnterHandler = newPowerUpUI.GetComponent<OnMouseEnterHandler>();
            onMouseEnterHandler.OnHoverEvent += OnHoverCallBack;
            onMouseEnterHandler.OnClickEvent += OnClickCallBack;
            //powerUpsManager.SubscribeToEvent(onMouseEnterHandler);
            PowerUpProperties powerUpProperties = newPowerUpUI.GetComponent<PowerUpProperties>();
            powerUpProperties.SetUp(powerUpProp.Item1, powerUpProp.Item2, randomPowerUp);
            safety = 0;
            fXSoundsManager.PlayFX(FXTypes.PowerUpMenu);
        }

        private void OnClickCallBack(PowerUps powerUp)
        {
            randomPowerUps.Clear();
            // tmp_description.transform.parent.gameObject.SetActive(false);
            VerticalLayout.SetActive(false);
            powerUpLayout.gameObject.SetActive(false);
            background.SetActive(false);
            powerUpsManager.OnPowerUpClick(powerUp);

            foreach (var item in instantiatedPowerUps)
            {
                Destroy(item);
            }
            instantiatedPowerUps.Clear();
            worldManager.NextRound();
        }

        private void OnHoverCallBack(string description, PowerUpProperties powerUpProperties)
        {
            tmp_description.text = description;
            RectTransform newRect = null;
            if (powerUpProperties != null)
            {
                newRect = powerUpProperties.GetComponent<RectTransform>();

            }

            if (selectedTransform == newRect)
                return;

            // Desrotar el anterior
            if (selectedTransform != newRect | newRect == null)
            {
                // RectTransform prev = lstActiveInventoryItems[selectedIndex].GetComponent<RectTransform>();
                selectedTransform.DOKill();
                selectedTransform.DOLocalRotate(Vector3.zero, timeTweenRotation);
            }

            // Rotar el nuevo
            //RectTransform dcurrent = lstActiveInventoryItems[newIndex].GetComponent<RectTransform>();

            if (newRect != null)
            {
                newRect.DOKill();
                newRect.DOLocalRotate(new Vector3(0, 0, powerUpMaxRotation), timeTweenRotation);
                fXSoundsManager.PlayFX(FXTypes.SelectItem, pitch: 1.5f, volumne: 0.3f);
            }

            if (newRect != null)
            {
                selectedTransform = newRect;
            }
            else
            {
                selectedTransform = null;
            }
            // return true;

        }


    }

}