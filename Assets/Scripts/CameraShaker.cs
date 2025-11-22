using System.Collections;
using UnityEngine;

namespace FeedTheBeasts.Scripts
{

    public class CameraShaker : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        [Header("Shake properties")]
        [SerializeField, Range(0.01f, 2f)] float duration = 1f;

        [SerializeField, Range(0.01f, 0.1f)] float offset = .05f;
        [SerializeField, Range(0.5f, 5f)] float cameraDefaultIntensity = .5f;
        Camera cam;



        void Awake()
        {
            cam = Camera.main;
        }

        internal void ShakeCamera(float intensity = 1)
        {
            StartCoroutine(ShakeCoroutine(intensity));
        }


        IEnumerator ShakeCoroutine(float intensity)
        {

            float time = 0;
            Vector2 randomPosition;
            Vector3 initialPosition = cam.transform.position;

            while (time <= duration)
            {
                time += Time.deltaTime;
                randomPosition = Random.insideUnitCircle * intensity * cameraDefaultIntensity;

                Vector3 newPosition = new(initialPosition.x + randomPosition.x,
                                        initialPosition.y + randomPosition.y,
                                         cam.transform.position.z);
                cam.transform.position = Vector3.Lerp(initialPosition, newPosition, offset);
                yield return null;
            }
            cam.transform.position = initialPosition;


        }
    }

}