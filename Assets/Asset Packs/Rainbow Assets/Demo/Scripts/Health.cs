using UnityEngine;
using UnityEngine.Events;

namespace RainbowAssets.Demo
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float maxHealth = 100;
        float currentHealth;
        UnityEvent OnDamageTaken;
        UnityEvent OnDie;

        public void TakeDamage(float damage)
        {
            if(!IsDead())
            {
                currentHealth = Mathf.Max(0, currentHealth - damage);

                if(IsDead())
                {
                    OnDie?.Invoke();
                }
                else
                {
                    OnDamageTaken?.Invoke();
                }
            }
        }

        void Start()
        {
            currentHealth = maxHealth;
        }

        bool IsDead()
        {
            return currentHealth <= 0;
        }
    }
}