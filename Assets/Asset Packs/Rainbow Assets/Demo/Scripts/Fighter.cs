using UnityEngine;

namespace RainbowAssets.Demo
{
    public class Fighter : MonoBehaviour
    {
        [SerializeField] Weapon weapon;

        // Animation Event
        public void Hit()
        {
            weapon.Hit(gameObject);
        }
    }
}