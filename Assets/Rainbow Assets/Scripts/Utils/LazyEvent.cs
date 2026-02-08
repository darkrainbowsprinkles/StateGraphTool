using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace RainbowAssets.Utils
{
    [System.Serializable]
    public class LazyEvent : UnityEvent
    {
        const float flagResetTime = 0.001f;
        bool wasInvoked = false;

        public new IEnumerator Invoke()
        {
            base.Invoke();
            wasInvoked = true;
            yield return new WaitForSeconds(flagResetTime);
            wasInvoked = false;
        }

        public bool WasInvoked()
        {
            return wasInvoked;
        }
    }

    [System.Serializable]
    public class LazyEvent<T> : UnityEvent<T>
    {
        const float flagResetTime = 0.001f;
        bool wasInvoked = false;

        public new IEnumerator Invoke(T value)
        {
            base.Invoke(value);
            wasInvoked = true;
            yield return new WaitForSeconds(flagResetTime);
            wasInvoked = false;
        }

        public bool WasInvoked()
        {
            return wasInvoked;
        }
    }
}