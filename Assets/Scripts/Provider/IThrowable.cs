using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace FeedTheBeasts.Scripts
{
    public interface IThrowable
    {
        AudioSource AudioSourceShoot { get; set; }

      //  int GetBullets();
        void TryThrow(Vector3 position);
    }


}