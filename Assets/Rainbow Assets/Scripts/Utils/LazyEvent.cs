using System.Collections;
using UnityEngine;
using UnityEngine.Events;
namespace RainbowAssets.Utils
{
    [System.Serializable]
    public class LazyEvent : UnityEvent
    {
        const float flagResetTime = 0.001f;

        /// <summary>
        /// Indicates whether the event was recently invoked.
        /// </summary>
        bool wasInvoked = false;

        public new IEnumerator Invoke()
        {
            base.Invoke();
            wasInvoked = true;
            yield return new WaitForSeconds(flagResetTime);
            wasInvoked = false;
        }

        /// <summary>
        /// Checks whether the event was invoked recently.
        /// </summary>
        /// <returns>True if invoked within the reset time, otherwise false.</returns>
        public bool WasInvoked()
        {
            return wasInvoked;
        }
    }

    [System.Serializable]
    public class LazyEvent<T> : UnityEvent<T>
    {
        const float flagResetTime = 0.001f;

        /// <summary>
        /// Indicates whether the event was recently invoked.
        /// </summary>
        bool wasInvoked = false;

        public new IEnumerator Invoke(T value)
        {
            base.Invoke(value);
            wasInvoked = true;
            yield return new WaitForSeconds(flagResetTime);
            wasInvoked = false;
        }

        /// <summary>
        /// Checks whether the event was invoked recently.
        /// </summary>
        /// <returns>True if invoked within the reset time, otherwise false.</returns>
        public bool WasInvoked()
        {
            return wasInvoked;
        }
    }
}