using RainbowAssets.Utils;
using UnityEngine;

namespace RainbowAssets.Demo.Movement
{
    public class FreeLooker : MonoBehaviour, IActionPerformer
    {
        [SerializeField, Range(0,1)] float freeLookSpeedFraction = 1;
        RainbowAssets.Demo.Control.InputReader inputReader;
        RainbowAssets.Demo.Movement.Mover mover;

        void Awake()
        {
            mover = GetComponent<RainbowAssets.Demo.Movement.Mover>();
            inputReader = GetComponent<RainbowAssets.Demo.Control.InputReader>();
        }

        Vector3 GetFreeLookDirection()
        {
            Vector2 movementInput = inputReader.GetMovementInput();
            Transform mainCamera = Camera.main.transform;

            Vector3 right = (movementInput.x * mainCamera.right).normalized;
            right.y = 0;

            Vector3 forward = (movementInput.y * mainCamera.forward).normalized;
            forward.y = 0;

            return right + forward + transform.position;
        }

        void IActionPerformer.PerformAction(EAction action, string[] parameters)
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