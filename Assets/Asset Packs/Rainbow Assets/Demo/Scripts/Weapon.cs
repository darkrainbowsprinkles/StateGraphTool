using UnityEngine;

namespace RainbowAssets.Demo
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] Transform hitboxCenter;
        [SerializeField] float hitboxRadius = 0.5f;
        [SerializeField] float damage = 50;

        public void Hit(GameObject user)
        {
            var hits = Physics.SphereCastAll(hitboxCenter.position, hitboxRadius, Vector3.up, 0);

            foreach(var hit in hits)
            {
                if(hit.transform.gameObject == user)
                {
                    continue;
                }

                if(hit.transform.TryGetComponent(out Health health))
                {
                    health.TakeDamage(damage);
                }
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(hitboxCenter.position, hitboxRadius);
        }   
    }
}