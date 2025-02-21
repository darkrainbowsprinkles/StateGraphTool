using System;
using RainbowAssets.Utils;
using UnityEngine;

namespace RainbowAssets.Demo
{
    public class InputReader : MonoBehaviour, IPredicateEvaluator
    {
        bool KeyCodePressed(string keyName)
        {
            if(!Enum.TryParse(keyName, out KeyCode keyCode))
            {
                Debug.LogError($"KeyCode with name {keyName} not found");
                return false;
            }

            return Input.GetKeyDown(keyCode);
        }

        bool? IPredicateEvaluator.Evaluate(EPredicate predicate, string[] parameters)
        {
            switch(predicate)
            {
                case EPredicate.KeyCodePressed:
                    return KeyCodePressed(parameters[0]);
            }

            return null;
        }
    }
}