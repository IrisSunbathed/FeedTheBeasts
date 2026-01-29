using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace FeedTheBeasts.Scripts

{

    public class NearAnimalDetection : MonoBehaviour
    {
        // StraightProjectile straightProjectile;
        // List<GameObject> pendingAnimals;

        // void Awake()
        // {
        //     straightProjectile = GetComponentInParent<StraightProjectile>();
        //     pendingAnimals = new List<GameObject>();
        // }

        // void OnCollisionEnter(Collision other)
        // {
        //     if (other.gameObject.CompareTag(Constants.ANIMAL_TAG))
        //     {
        //         AnimalHunger animalHunger = other.gameObject.GetComponent<AnimalHunger>();
        //         if (!straightProjectile.gameObject.CompareTag(animalHunger.preferredFood.ToString()))
        //             return;

        //         if (straightProjectile.followedAnimal != null)
        //         {
        //             if (pendingAnimals.Count == 0)
        //             {
        //                 straightProjectile.SetDirection(other.gameObject);
        //             }
        //             else
        //             {
        //                 straightProjectile.SetDirection(pendingAnimals[0]);
        //                 pendingAnimals.Remove(pendingAnimals[0]);
        //                 for (int i = 1; i < pendingAnimals.Count; i++)
        //                 {
        //                     if (pendingAnimals[i++] != null)
        //                     {
        //                         pendingAnimals[i] = pendingAnimals[i++];
        //                     }
        //                 }
        //             }
        //         }
        //         else
        //         {
        //             pendingAnimals.Add(other.gameObject);
        //         }

        //     }
        // }
        // // void OnTriggerEnter(Collider other)
        // // {
        // //     if (other.CompareTag(Constants.ANIMAL_TAG))
        // //     {
        // //         AnimalHunger animalHunger = other.GetComponent<AnimalHunger>();
        // //         if (!straightProjectile.gameObject.CompareTag(animalHunger.preferredFood.ToString()))
        // //             return;

        // //         if (straightProjectile.followedAnimal != null)
        // //         {
        // //             if (pendingAnimals.Count == 0)
        // //             {
        // //                 straightProjectile.SetDirection(other.gameObject);
        // //             }
        // //             else
        // //             {
        // //                 straightProjectile.SetDirection(pendingAnimals[0]);
        // //                 pendingAnimals.Remove(pendingAnimals[0]);
        // //                 for (int i = 1; i < pendingAnimals.Count; i++)
        // //                 {
        // //                     if (pendingAnimals[i++] != null)
        // //                     {
        // //                         pendingAnimals[i] = pendingAnimals[i++];
        // //                     }
        // //                 }
        // //             }
        // //         }
        // //         else
        // //         {
        // //             pendingAnimals.Add(other.gameObject);
        // //         }

        // //     }
        // // }

        // void OnCollisionExit(Collision other)
        // {
        //       if (other.gameObject.CompareTag(Constants.ANIMAL_TAG))
        //     {

        //         AnimalHunger animalHunger = other.gameObject.GetComponent<AnimalHunger>();
        //         if (!straightProjectile.gameObject.CompareTag(animalHunger.preferredFood.ToString()))
        //             return;
        //         foreach (var item in pendingAnimals)
        //         {
        //             if (item == straightProjectile.followedAnimal)
        //             {
        //                 pendingAnimals.Remove(item);
        //             }
        //         }
        //     }
        // }

        // // void OnTriggerExit(Collider other)
        // // {
        // //     if (other.CompareTag(Constants.ANIMAL_TAG))
        // //     {

        // //         AnimalHunger animalHunger = other.GetComponent<AnimalHunger>();
        // //         if (!straightProjectile.gameObject.CompareTag(animalHunger.preferredFood.ToString()))
        // //             return;
        // //         foreach (var item in pendingAnimals)
        // //         {
        // //             if (item == straightProjectile.followedAnimal)
        // //             {
        // //                 pendingAnimals.Remove(item);
        // //             }
        // //         }
        // //     }
        // // }


    }

}