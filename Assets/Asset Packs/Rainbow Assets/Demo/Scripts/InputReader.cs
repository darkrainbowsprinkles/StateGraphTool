using System;
using RainbowAssets.Utils;
using UnityEngine;

namespace RainbowAssets.Demo
{
    /// <summary>
    /// Handles input reading.
    /// </summary>
    public class InputReader : MonoBehaviour, IPredicateEvaluator
    {
        /// <summary>
        /// Checks if the specified key was pressed.
        /// </summary>
        /// <param name="keyName">The name of the key to check.</param>
        /// <returns>True if the key was pressed, otherwise false.</returns>
        bool KeyCodePressed(string keyName)
        {
            if (!Enum.TryParse(keyName, out KeyCode keyCode))
            {
                Debug.LogError($"KeyCode with name {keyName} not found");
                return false;
            }

            return Input.GetKeyDown(keyCode);
        }

        /// <summary>
        /// Evaluates input-related predicates.
        /// </summary>
        /// <param name="predicate">The predicate to evaluate.</param>
        /// <param name="parameters">Additional parameters for evaluation.</param>
        /// <returns>
        /// True if the predicate condition is met, false otherwise, or null if unsupported.
        /// </returns>
        bool? IPredicateEvaluator.Evaluate(EPredicate predicate, string[] parameters)
        {
            switch (predicate)
            {
                case EPredicate.KeyCodePressed:
                    return KeyCodePressed(parameters[0]);
            }

            return null;
        }
    }
}