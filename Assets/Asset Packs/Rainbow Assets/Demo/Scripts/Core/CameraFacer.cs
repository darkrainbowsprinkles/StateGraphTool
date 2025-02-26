using UnityEngine;

namespace RainbowAssets.Demo.Core
{
    /// <summary>
    /// Ensures the object always faces the camera.
    /// </summary>
    public class CameraFacer : MonoBehaviour
    {
        /// <summary>
        /// Reference to the main camera's transform.
        /// </summary>
        Transform mainCameraTransform;

        // LIFECYCLE METHODS

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