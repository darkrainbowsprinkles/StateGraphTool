using UnityEngine;

namespace RainbowAssets.Demo
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] RainbowAssets.Demo.Attributes.Health health;
        [SerializeField] RectTransform foreground;

        void Update()
        {
            foreground.localScale = new Vector3(health.GetHealthFraction(), 1, 1);
        }
    }
}