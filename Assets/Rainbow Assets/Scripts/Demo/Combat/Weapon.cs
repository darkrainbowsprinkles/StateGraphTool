using UnityEngine;

namespace RainbowAssets.Demo.Combat
{
    /// <summary>
    /// Represents a weapon for combat.
    /// </summary>
    public class Weapon : MonoBehaviour
    {
        /// <summary>
        /// Center of the hitbox sphere cast.
        /// </summary>
        [SerializeField] Transform hitboxCenter;
        
        /// <summary>
        /// The radius of the hitbox sphere cast.
        /// </summary>
        [SerializeField] float hitboxRadius = 0.5f;

        /// <summary>
        /// Damage points associated with this weapon.
        /// </summary>
        [SerializeField] float damage = 50;

        /// <summary>
        /// Does a sphere cast check to all health components for dealing damage.
        /// </summary>
        /// <param name="user">This weapon's user.</param>
        public void Hit(GameObject user)
        {
            var hits = Physics.SphereCastAll(hitboxCenter.position, hitboxRadius, Vector3.up, 0);

            foreach(var hit in hits)
            {
                if(hit.transform.gameObject == user)
                {
                    continue;
                }

                if(hit.transform.TryGetComponent(out RainbowAssets.Demo.Attributes.Health health))
                {
                    health.TakeDamage(damage);
                }
            }
        }

        /// <summary>
        /// Draws gizmos in the editor to visualize the weapon's hitbox.
        /// </summary>
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(hitboxCenter.position, hitboxRadius);
        }   
    }
}