using UnityEngine;

namespace RainbowAssets.Demo.Core
{
    public class CameraFacer : MonoBehaviour
    {
        Transform mainCameraTransform;

        void Awake()
        {
            mainCameraTransform = Camera.main.transform;
        }

        void LateUpdate()
        {
            transform.forward = mainCameraTransform.forward;
        }
    }
}