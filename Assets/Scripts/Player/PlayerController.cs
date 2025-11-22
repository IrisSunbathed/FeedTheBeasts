using System;
using System.Buffers;
using NUnit.Framework;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FeedTheBeasts.Scripts
{
    [RequireComponent(typeof(MeshRenderer), typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {

        [Header("Movement Properties")]

        [SerializeField] bool canMove;
        public bool CanMove { get => canMove; set { canMove = value; } }
        [SerializeField] float speed = 10;

        Rigidbody rbPlayer;
        float horizontalInput;
        float verticalInput;
        [Header("Shoot Properties")]
        [SerializeField] Shooter shooter;
        [SerializeField] FoodSelectorManager foodSelectorManager;
        float lookAngle;

        [Header("Get bounds")]

        MeshRenderer meshRenderer;

        Camera mainCam;

        float lengthCam;

        float characterXBounds;

        float characterXBoundsSign;

        void Awake()
        {
            Assert.IsNotNull(shooter, "ERROR: Shooter not added to Player Controller");
            Assert.IsNotNull(shooter, "ERROR: foodSelectorManager not added to Player Controller");
            meshRenderer = GetComponent<MeshRenderer>();
            rbPlayer = GetComponent<Rigidbody>();
            characterXBounds = meshRenderer.bounds.max.x;
            mainCam = Camera.main;
            lengthCam = mainCam.orthographicSize * mainCam.aspect;
            // GetMesh(goProjectile);

        }
        internal void Init()
        {
            CanMove = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (CanMove)
            {
                #region Movement
                GetAxisValues();
                CheckOutOfBoundsX();
                CheckOutOfBoundsZ();
                rbPlayer.linearVelocity = new Vector3(horizontalInput, 0, verticalInput) * speed;
                //add animator
                #endregion
                #region Shoot
                LookAtMousePosition();
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    //shooter.TryShoot();
                    foodSelectorManager.TryShootCurrentWeapon();

                }
                #endregion
                #region Recharge
                if (Input.GetKeyDown(KeyCode.R))
                {
                  
                    foodSelectorManager.ReloadCurrentWeapon();

                }
                #endregion

            }
            else
            {
                horizontalInput = 0;
                verticalInput = 0;
            }

        }

        private void LookAtMousePosition()
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 mouseToWorld = mainCam.ScreenToWorldPoint(mousePosition);
            lookAngle = AngleBetweenTwoPoints(transform.position, mouseToWorld) + 180;
            transform.eulerAngles = new Vector3(0, lookAngle, 0);
        }

        private float AngleBetweenTwoPoints(Vector3 position, Vector3 mouseToWorld)
        {
            return Mathf.Atan2(position.x - mouseToWorld.x, position.z - mouseToWorld.z) * Mathf.Rad2Deg;
        }

        private void GetAxisValues()
        {
            horizontalInput = Input.GetAxis(Constants.HORIZONTAL_AXIS);
            verticalInput = Input.GetAxis(Constants.VERTICAL_AXIS);
        }

        private void CheckOutOfBoundsZ()
        {
            if (transform.position.z < -mainCam.orthographicSize
                | transform.position.z > mainCam.orthographicSize)
            {
                transform.position = new Vector3(transform.position.x
                                               , transform.position.y, mainCam.orthographicSize * Mathf.Sign(transform.position.z));
            }
        }

        private void CheckOutOfBoundsX()
        {
            characterXBoundsSign = characterXBounds * Mathf.Sign(transform.position.x);
            if (transform.position.x < -lengthCam + characterXBoundsSign
                | transform.position.x > lengthCam + characterXBoundsSign)
            {
                transform.position = new Vector3(lengthCam
                                                 * Mathf.Sign(transform.position.x) + characterXBoundsSign
                                                , transform.position.y, transform.position.z);
            }
        }

    }

}