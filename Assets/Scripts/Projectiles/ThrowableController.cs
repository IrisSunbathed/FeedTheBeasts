using System;
using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine;


namespace FeedTheBeasts.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(MeshRenderer))]
    public class ThrowableController : MonoBehaviour
    {


        // int resistance;
        // float timeBetweenBites;
        public float firingAngle = 45.0f;
        public float gravity = 9.8f;

        MeshRenderer meshRenderer;
        CamerasManager camerasManager;

        float boneXBounds;

        float boneXBoundsSign;
        float lengthCam;
        float orthographicSize;

        Rigidbody rbThrowable;

        void Start()
        {
            camerasManager = CamerasManager.Instance;
            lengthCam = camerasManager.GetCameraLength();
            orthographicSize = camerasManager.OrthographicSize;
        }


        void Awake()
        {
            rbThrowable = GetComponent<Rigidbody>();
            meshRenderer = GetComponent<MeshRenderer>();
            boneXBounds = meshRenderer.bounds.max.x;
            SetUp();

        }

        internal void SetUp()
        {

            enabled = true;

            rbThrowable.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            // if (!TryGetComponent(out DistractCollision _))
            // {
            //     DistractCollision distractCollision = gameObject.AddComponent<DistractCollision>();
            //     distractCollision.SetUp(resistance, timeBetweenBites);

            // }

            // if (TryGetComponent(out StraightController straightController))
            // {
            //     straightController.Deactivate();
            // }
        }

        void Update()
        {
            GetXBounds();
            GetZBounds();
        }

        private void GetZBounds()
        {
            if (transform.position.z < -orthographicSize
              | transform.position.z > orthographicSize)
            {
                transform.position = new Vector3(transform.position.x
                                               , transform.position.y, orthographicSize * Mathf.Sign(transform.position.z));
            }
        }

        private void GetXBounds()
        {
            boneXBoundsSign = boneXBounds * Mathf.Sign(transform.position.x);
            if (transform.position.x < -lengthCam + boneXBoundsSign
                | transform.position.x > lengthCam + boneXBoundsSign)
            {
                transform.position = new Vector3(lengthCam
                                                 * Mathf.Sign(transform.position.x) + boneXBoundsSign
                                                , transform.position.y, transform.position.z);
            }
        }

        internal void SimulateProjectile(Vector3 target)
        {
            Vector3 newTarget = new Vector3(target.x, transform.position.y, target.z);
            StartCoroutine(SimulateProjectileCoroutine(newTarget));
        }

        IEnumerator SimulateProjectileCoroutine(Vector3 target)
        {

            // Calculate distance to target
            float target_Distance = Vector3.Distance(transform.position, target);

            // Calculate the velocity needed to throw the object to the target at specified angle.
            float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

            // Extract the X  Y componenent of the velocity
            float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
            float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

            // Calculate flight time.
            float flightDuration = target_Distance / Vx;

            // Rotate projectile to face the target.
            transform.rotation = Quaternion.LookRotation(target - transform.position);

            float elapse_time = 0;

            while (elapse_time < flightDuration)
            {
                transform.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);

                elapse_time += Time.deltaTime;

                yield return null;
            }
            if (transform.position.y < 0)
            {
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            }
            rbThrowable.collisionDetectionMode = CollisionDetectionMode.Discrete;
        }

        // internal void Deactivate()
        // {
        //     if (TryGetComponent(out DistractCollision distractCollision))
        //     {
        //         distractCollision.enabled = false;
        //         Destroy(this);
        //     }
        // }
    }

}