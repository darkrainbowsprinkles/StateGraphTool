using RainbowAssets.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace RainbowAssets.Demo.Attributes
{
    /// <summary>
    /// Manages the health system, handling damage, death events, and evaluations.
    /// </summary>
    public class Health : MonoBehaviour, IPredicateEvaluator
    {
        /// <summary>
        /// The maximum amount of health the entity can have.
        /// </summary>
        [SerializeField] float maxHealth = 100;

        /// <summary>
        /// The current health of the entity.
        /// </summary>
        [SerializeField] float currentHealth;

        /// <summary>
        /// Event triggered when the entity dies.
        /// </summary>
        [SerializeField] LazyEvent OnDie = new();

        /// <summary>
        /// Event triggered when damage is taken.
        /// </summary>
        LazyEvent OnDamageTaken = new();

        /// <summary> 
        /// </summary>
        /// <returns>Health amount represented as a fraction</returns>
        public float GetHealthFraction()
        {
            return currentHealth / maxHealth;
        }

        /// <summary>
        /// Checks if the entity is dead.
        /// </summary>
        /// <returns>True if health is zero, otherwise false.</returns>
        public bool IsDead()
        {
            return currentHealth == 0;
        }

        /// <summary>
        /// Reduces the entity's health by the specified amount.
        /// </summary>
        /// <param name="damage">The amount of damage to take.</param>
        public void TakeDamage(float damage)
        {
            if (!IsDead())
            {
                currentHealth = Mathf.Max(0, currentHealth - damage);

                if (IsDead())
                {
                    GetComponent<NavMeshAgent>().enabled = false;
                    StartCoroutine(OnDie?.Invoke());
                }
                else
                {
                    StartCoroutine(OnDamageTaken?.Invoke());
                }
            }
        }

        // LIFECYCLE METHODS

        void Start()
        {
            currentHealth = maxHealth;
        }

        /// <summary>
        /// Evaluates predicates related to health events.
        /// </summary>
        /// <param name="predicate">The predicate to evaluate.</param>
        /// <param name="parameters">Additional parameters for evaluation.</param>
        /// <returns>
        /// True if the predicate condition is met, false otherwise, or null if unsupported.
        /// </returns>
        bool? IPredicateEvaluator.Evaluate(EPredicate predicate, string[] parameters)
        {
            switch (predicate)
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