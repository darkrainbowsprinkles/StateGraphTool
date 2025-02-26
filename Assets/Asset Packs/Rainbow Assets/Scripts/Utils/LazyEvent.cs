using System.Collections;
using UnityEngine;
using UnityEngine.Events;
namespace RainbowAssets.Utils
{
    /// <summary>
    /// A UnityEvent that tracks whether it was recently invoked, useful for getting event invoke data.
    /// </summary>
    [System.Serializable]
    public class LazyEvent : UnityEvent
    {
        /// <summary>
        /// Time in seconds before the invocation flag resets.
        /// </summary>
        const float flagResetTime = 0.001f;

        /// <summary>
        /// Indicates whether the event was recently invoked.
        /// </summary>
        bool wasInvoked = false;

        /// <summary>
        /// Invokes the event and sets the flag, which resets after a short delay.
        /// </summary>
        /// <returns>Coroutine for flag reset timing.</returns>
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

    /// <summary>
    /// A generic version of LazyEvent that supports a parameterized UnityEvent.
    /// </summary>
    /// <typeparam name="T">The type of parameter passed to the event.</typeparam>
    [System.Serializable]
    public class LazyEvent<T> : UnityEvent<T>
    {
        /// <summary>
        /// Time in seconds before the invocation flag resets.
        /// </summary>
        const float flagResetTime = 0.001f;

        /// <summary>
        /// Indicates whether the event was recently invoked.
        /// </summary>
        bool wasInvoked = false;

        /// <summary>
        /// Invokes the event with a parameter and sets the flag, which resets after a short delay.
        /// </summary>
        /// <param name="value">The parameter passed to the event.</param>
        /// <returns>Coroutine for flag reset timing.</returns>
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