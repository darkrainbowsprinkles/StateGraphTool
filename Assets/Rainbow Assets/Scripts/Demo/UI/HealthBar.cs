using UnityEngine;

namespace RainbowAssets.Demo
{
    /// <summary>
    /// Controls the health bar UI by adjusting its scale based on the current health percentage.
    /// </summary>
    public class HealthBar : MonoBehaviour
    {
        /// <summary>
        /// The Health component representing the entity's health.
        /// </summary>
        [SerializeField] RainbowAssets.Demo.Attributes.Health health;

        /// <summary>
        /// The foreground RectTransform that visually represents the health bar.
        /// </summary>
        [SerializeField] RectTransform foreground;

        // LIFECYCLE METHODS

        void Update()
        {
            foreground.localScale = new Vector3(health.GetHealthFraction(), 1, 1);
        }
    }
}