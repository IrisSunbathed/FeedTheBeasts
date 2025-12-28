using System;
using System.Buffers;
using DG.Tweening;
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
                    runState.currentRunSpeed = 0;
                }
                if (canMove == false)
                {
                     runState.currentRunSpeed = runState.runSpeed;
                }
            }
        }

        public float HorizontalInput { get; private set; }
        public float VerticalInput { get; private set; }

        Vector3 originalPosition;

        Rigidbody rbPlayer;
        [Header("Character States")]

        States states;
        public RunState runState;
        public IdleState idleState;
        public DeathState deathState;
        public WinState winState;
        public PlantingState plantingState;
        internal float plantingTime;


        Animator animator;
        [Header("Shoot Properties")]
        [SerializeField] FoodSelectorManager foodSelectorManager;
        float pressedKeyTime;
        float lookAngle;
        bool hasShoot;

        [Header("Get bounds")]

        MeshRenderer meshRenderer;

        float characterXBounds;

        float characterXBoundsSign;
        #region Camera information

        CamerasManager camerasManager;

        float orthographicSize;

        float lengthCam;
        #endregion



        void Start()
        {
            camerasManager = CamerasManager.Instance;
            orthographicSize = camerasManager.OrthographicSize;
            lengthCam = camerasManager.GetCameraLength();
            hasShoot = false;

        }

        void Awake()
        {
            Assert.IsNotNull(foodSelectorManager, "ERROR: foodSelectorManager not added to Player Controller");
            #region GET COMPONENTS
            meshRenderer = GetComponent<MeshRenderer>();
            rbPlayer = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            #endregion
            #region SET VARIABLES
            characterXBounds = meshRenderer.bounds.max.x;
            originalPosition = transform.position;
            #endregion
            SetpUpStates();

        }

        private void SetpUpStates()
        {
            runState.Setup(rbPlayer, animator, this);
            idleState.Setup(rbPlayer, animator, this);
            deathState.Setup(rbPlayer, animator, this);
            winState.Setup(rbPlayer, animator, this);
            plantingState.Setup(rbPlayer, animator, this);
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
                rbPlayer.linearVelocity = new Vector3(HorizontalInput, 0, VerticalInput) * runState.currentRunSpeed;
                
                #endregion
                #region Shoot
                LookAtMousePosition();
                if (Input.GetKey(KeyCode.Mouse0))
                {

                    pressedKeyTime = 0;
                    pressedKeyTime += Time.deltaTime;

                    if (!hasShoot)
                    {
                        Vector3 screenPoint = camerasManager.GetScreenToWorldPoint(Input.mousePosition);
                        foodSelectorManager.TryShootCurrentWeapon(screenPoint);
                        hasShoot = true;
                    }

                }
                
                #endregion
                #region Reload
                if (Input.GetKeyDown(KeyCode.R))
                {

                    foodSelectorManager.ReloadCurrentWeapon();

                }
                #endregion
            }

            if (Input.GetKeyUp(KeyCode.Mouse0)/* & states == plantingState*/)
            {
                pressedKeyTime = 0;
                if (states == plantingState)
                {
                    plantingState.Exit();
                }
                hasShoot = false;
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

            if (states != plantingState)
            {

                if (HorizontalInput == 0 & VerticalInput == 0)
                {
                    states.Exit();
                    states = idleState;
                }
                else
                {
                    states.Exit();
                    states = runState;
                }
                states.Enter();
            }
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
            Vector3 mouseToWorld = camerasManager.GetScreenToWorldPoint(mousePosition);

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

            if (transform.position.z < -orthographicSize
                | transform.position.z > orthographicSize)
            {
                transform.position = new Vector3(transform.position.x
                                               , transform.position.y, orthographicSize * Mathf.Sign(transform.position.z));
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

        internal void SetOriginalPosition()
        {
            transform.position = originalPosition;
        }

        internal States GetCurrentState()
        {
            return states;
        }

        internal void WinState()
        {
            CanMove = false;
            states = winState;
            states.Enter();
        }

        internal void SetPlantingState()
        {
            states = plantingState;
            states.Enter();
        }

        internal Vector3 GetFontPosition()
        {
            return new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + 1f);
        }
    }

}