using NUnit.Framework;
using UnityEngine;
namespace FeedTheBeasts.Scripts
{

    public class UpdatePlayerRigPosition : MonoBehaviour
    {
        [SerializeField] Transform playerTransform;

        Vector3 newPosition;


        void Awake()
        {
            Assert.IsNotNull(playerTransform, "ERROR: playerTransform is not added");
            
        }

        // Update is called once per frame
        void Update()
        {
            newPosition = new Vector3(playerTransform.position.x + 6f, 5, playerTransform.position.z);
            transform.position = newPosition;
        }

      
    }

}