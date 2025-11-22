using UnityEngine;




namespace FeedTheBeasts.Scripts
{
    [RequireComponent(typeof(MeshFilter))]
    public class DetectCollisions : MonoBehaviour
    {

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.ANIMAL_TAG))
            {
                AnimalHunger feedPoints = other.GetComponent<AnimalHunger>();
                feedPoints.FeedAnimal(tag);
                gameObject.SetActive(false);
            }
        }
    }

}