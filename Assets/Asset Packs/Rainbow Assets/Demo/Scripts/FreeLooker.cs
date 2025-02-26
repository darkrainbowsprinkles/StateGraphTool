using RainbowAssets.Utils;
using UnityEngine;

namespace RainbowAssets.Demo
{
    /// <summary>
    /// Allows free movement of the character based on player input and camera direction.
    /// </summary>
    public class FreeLooker : MonoBehaviour, IAction
    {
        /// <summary>
        /// The movement speed fraction when freely looking.
        /// </summary>
        [SerializeField, Range(0,1)] float freeLookSpeedFraction = 1;

        Mover mover;

        // LIFECYCLE METHODS

        void Awake()
        {
            mover = GetComponent<Mover>();
        }

        /// <summary>
        /// Computes the direction for free movement based on camera orientation and input.
        /// </summary>
        /// <returns>A world-space direction vector.</returns>
        Vector3 GetFreeLookDirection()
        {
            Transform mainCamera = Camera.main.transform;

            Vector3 right = (GetInputValue().x * mainCamera.right).normalized;
            right.y = 0;

            Vector3 forward = (GetInputValue().y * mainCamera.forward).normalized;
            forward.y = 0;

            return right + forward + transform.position;
        }

        /// <summary>
        /// Gets the raw input values for movement.
        /// </summary>
        /// <returns>A Vector2 representing horizontal and vertical movement input.</returns>
        Vector2 GetInputValue()
        {
            return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }

        /// <summary>
        /// Executes an action related to free movement.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="parameters">Additional parameters for the action.</param>
        void IAction.DoAction(EAction action, string[] parameters)
        {
            switch(action)
            {
                case EAction.FreeLook:
                    mover.MoveTo(GetFreeLookDirection(), freeLookSpeedFraction);
                    break;
            }
        }
    }
}