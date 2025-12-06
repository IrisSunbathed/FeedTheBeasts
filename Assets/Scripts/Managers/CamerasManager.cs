using System;
using NUnit.Framework;
using UnityEngine;
namespace FeedTheBeasts.Scripts
{

    public class CamerasManager : MonoBehaviour
    {

        [Header("Cameras references")]
        [SerializeField] Camera mainCamera;
        [SerializeField] Camera menuCamera;

         [SerializeField] ShakeController shakeController;

        static CamerasManager instance;
        public static CamerasManager Instance => instance;

        public float OrthographicSize => mainCamera.orthographicSize;
        public float UpperLimitCamera => OrthographicSize * 2;

        void Awake()
        {
            Assert.IsNotNull(mainCamera, "ERROR: Main camera is not included");
            Assert.IsNotNull(menuCamera, "ERROR: Menu camera is not included");
            Assert.IsNotNull(shakeController, "ERROR: Shake Controller is not included");

            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        internal void SwitchCameras(bool isGameplayCamera)
        {
            Debug.Log("Test");
            mainCamera.gameObject.SetActive(isGameplayCamera);
            menuCamera.gameObject.SetActive(!isGameplayCamera);
        }
        internal float GetCameraLength()
        {
            return mainCamera.orthographicSize * mainCamera.aspect;
        }

        internal Vector3 GetScreenToWorldPoint(Vector3 mousePosition)
        {
            return mainCamera.ScreenToWorldPoint(mousePosition);
        }

        internal void ShakeCurrentCamera(float intensity)
        {
            Camera currentCamera = Camera.allCameras[0];

            shakeController.Shake(currentCamera.transform, 0.5f, .5f, intensity);
        }
    }

}