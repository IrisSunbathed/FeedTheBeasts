using System.Collections;
using UnityEngine;

namespace FeedTheBeasts.Scripts
{

    public class ShakeController : MonoBehaviour
    {
        internal void Shake(Transform transform, float shakeOffset, float duration, float intensity = 1)
        {
            StartCoroutine(ShakeCoroutine(transform, shakeOffset, duration, intensity));
        }


        IEnumerator ShakeCoroutine(Transform transform, float shakeOffset, float duration, float intensity)
        {

            float time = 0;
            Vector2 randomPosition;
            Vector3 initialPosition = transform.position;

            while (time <= duration)
            {
                time += Time.deltaTime;
                randomPosition = Random.insideUnitCircle * intensity; /** cameraDefaultIntensity;*/

                Vector3 newPosition = new(initialPosition.x + randomPosition.x,
                                        initialPosition.y + randomPosition.y,
                                         transform.position.z);
                transform.position = Vector3.Lerp(initialPosition, newPosition, shakeOffset);
                yield return null;
            }
            transform.position = initialPosition;


        }
    }
}
