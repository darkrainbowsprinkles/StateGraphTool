using RainbowAssets.Utils;
using UnityEngine;

namespace RainbowAssets.Demo
{
    public class Health : MonoBehaviour, IPredicateEvaluator
    {
        [SerializeField] float maxHealth = 100;
        [SerializeField] float currentHealth;
        LazyEvent OnDamageTaken = new();
        LazyEvent OnDie = new();

        public void TakeDamage(float damage)
        {
            if(!IsDead())
            {
                currentHealth = Mathf.Max(0, currentHealth - damage);

                if(IsDead())
                {
                    StartCoroutine(OnDie?.Invoke());
                }
                else
                {
                    StartCoroutine(OnDamageTaken?.Invoke());
                }
            }
        }

        void Start()
        {
            currentHealth = maxHealth;
        }

        bool IsDead()
        {
            return currentHealth == 0;
        }

        bool? IPredicateEvaluator.Evaluate(EPredicate predicate, string[] parameters)
        {
            switch(predicate)
            {
                case EPredicate.DamageTakenEvent:
                    return OnDamageTaken.WasInvoked();

                case EPredicate.DieEvent:
                    return OnDie.WasInvoked();
            }

            return null;
        }
    }
}