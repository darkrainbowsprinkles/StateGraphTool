using RainbowAssets.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RainbowAssets.Demo.Control
{
    public class InputReader : MonoBehaviour, IPredicateEvaluator
    {
        PlayerInput playerInput;

        public Vector2 GetMovementInput()
        {
            return playerInput.actions["Locomotion"].ReadValue<Vector2>();
        }

        void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
        }

        bool? IPredicateEvaluator.Evaluate(EPredicate predicate, string[] parameters)
        {
            switch (predicate)
            {
                case EPredicate.InputActionPressed:
                    string inputAction = parameters[0];
                    return playerInput.actions[inputAction].WasPressedThisFrame();
            }

            return null;
        }
    }
}