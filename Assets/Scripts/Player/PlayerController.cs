using System;
using System.Buffers;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FeedTheBeasts.Scripts
{
    [RequireComponent(typeof(MeshRenderer), typeof(Rigidbody))]
    [RequireComponent(typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {

        [Header("Movement Properties")]

        [SerializeField] bool canMove;
        public bool CanMove
        {
            get => canMove;
            set
            {
                canMove = value;

                if (canMove == false)
                {
                    HorizontalInput = 0;
                    VerticalInput = 0;
                }
            }
        }

        public float HorizontalInput { get; private set; }
        public float VerticalInput { get; private set; }

        Rigidbody rbPlayer;
        [Header("Character States")]

        States states;
        public RunState runState;
        public States idleState;
        public States deathState;

        Animator animator;
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
            #region GET COMPONENTS
            meshRenderer = GetComponent<MeshRenderer>();
            rbPlayer = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            #endregion
            #region SET VARIABLES
            characterXBounds = meshRenderer.bounds.max.x;
            mainCam = Camera.main;
            lengthCam = mainCam.orthographicSize * mainCam.aspect;
            #endregion
            SetpUpStates();

        }

        private void SetpUpStates()
        {
            runState.Setup(rbPlayer, animator, this);
            idleState.Setup(rbPlayer, animator, this);
            deathState.Setup(rbPlayer, animator, this);
            states = idleState;
        }

        internal void Init()
        {
            states = idleState;
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
                rbPlayer.linearVelocity = new Vector3(HorizontalInput, 0, VerticalInput) * runState.runSpeed;
                //add animator
                #endregion
                #region Shoot
                LookAtMousePosition();
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    foodSelectorManager.TryShootCurrentWeapon();
                }
                #endregion
                #region Reload
                if (Input.GetKeyDown(KeyCode.R))
                {

                    foodSelectorManager.ReloadCurrentWeapon();

                }
                #endregion

            }

            #region States
            if (states.IsStateComplete)
            {
                SelectState();
            }

            states.Do();
            #endregion


        }

        private void SelectState()
        {

            if (HorizontalInput == 0 & VerticalInput == 0)
            {
                states = idleState;
            }
            else
            {
                states = runState;
            }
            Debug.Log(states);
            states.Enter();
        }

        internal void SetDeathState()
        {
            CanMove = false;
            states = deathState;
            states.Enter();
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
            HorizontalInput = Input.GetAxis(Constants.HORIZONTAL_AXIS);
            VerticalInput = Input.GetAxis(Constants.VERTICAL_AXIS);
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