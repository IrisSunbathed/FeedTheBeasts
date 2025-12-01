using System.Collections;
using UnityEngine;

namespace FeedTheBeasts.Scripts
{
    public class FruitBasket : MonoBehaviour
    {
        [SerializeField] int totalResistance;
        [SerializeField] float totalCooldown;
        float cooldown;
        int resistance;

        void Awake()
        {
            resistance = totalResistance;
            cooldown = 0;
        }

        void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(Constants.ANIMAL_TAG) & cooldown == 0)
            {
                StartCoroutine(ResistanceCoroutine());
            }
        }

        IEnumerator ResistanceCoroutine()
        {
            resistance--;
            cooldown = totalCooldown;
            if (resistance == 0)
            {
                Destroy(gameObject);
            }
            while (cooldown >= 0)
            {
                cooldown -= Time.deltaTime;
                yield return null;
            }
            cooldown = 0;

        }
    }
}

