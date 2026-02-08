using RainbowAssets.Utils;
using UnityEngine;

namespace RainbowAssets.Demo.Movement
{
    public class FreeLooker : MonoBehaviour, IActionPerformer
    {
        [SerializeField, Range(0,1)] float freeLookSpeedFraction = 1;

        RainbowAssets.Demo.Movement.Mover mover;

        // LIFECYCLE METHODS

        void Awake()
        {
            mover = GetComponent<RainbowAssets.Demo.Movement.Mover>();
        }

        Vector3 GetFreeLookDirection()
        {
            Transform mainCamera = Camera.main.transform;

            Vector3 right = (GetInputValue().x * mainCamera.right).normalized;
            right.y = 0;

            Vector3 forward = (GetInputValue().y * mainCamera.forward).normalized;
            forward.y = 0;

            return right + forward + transform.position;
        }

        Vector2 GetInputValue()
        {
            return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
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