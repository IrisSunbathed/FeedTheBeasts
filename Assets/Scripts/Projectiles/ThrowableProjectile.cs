using System.Collections;
using UnityEngine;

namespace FeedTheBeasts.Scripts
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(MeshRenderer))]
    public class ThrowableProjectile : Projectile
    {

        public float firingAngle = 45.0f;
        public float gravity = 9.8f;
        protected override void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            meshRenderer = GetComponent<MeshRenderer>();
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.ANIMAL_TAG))
            {
                Debug.Log("Animal interaction with it");

            }
        }

        internal void SimulateProjectile(Vector3 target, Transform origin)
        {
            Vector3 newTarget = new Vector3(target.x, origin.position.y, target.z);
            StartCoroutine(SimulateProjectileCoroutine(newTarget, origin));
        }

        IEnumerator SimulateProjectileCoroutine(Vector3 target, Transform origin)
        {

            // Calculate distance to target
            float target_Distance = Vector3.Distance(origin.position, target);

            // Calculate the velocity needed to throw the object to the target at specified angle.
            float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

            // Extract the X  Y componenent of the velocity
            float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
            float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

            // Calculate flight time.
            float flightDuration = target_Distance / Vx;

            // Rotate projectile to face the target.
            origin.rotation = Quaternion.LookRotation(target - origin.position);

            float elapse_time = 0;

            while (elapse_time < flightDuration)
            {
                origin.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);

                elapse_time += Time.deltaTime;

                yield return null;
            }
        }
    }
}

