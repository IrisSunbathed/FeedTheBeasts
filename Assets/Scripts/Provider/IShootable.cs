using System.Numerics;
using UnityEngine;

namespace FeedTheBeasts.Scripts
{
    public interface IShootable
    {
        AudioSource AudioSourceShoot { get; set; }

      //  int GetBullets();
        void TryShoot();

    }


}