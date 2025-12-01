using System;
using System.Collections;

namespace FeedTheBeasts.Scripts
{
    public interface IRechargeable
    {
        public event Action<float> OnRechargeEvent;
        bool IsRecharging { get; set; }

        void IncreaseShootCount();
        public void TryReload();
        IEnumerator ReloadCoroutine();

        int GetBullets();


    }


}