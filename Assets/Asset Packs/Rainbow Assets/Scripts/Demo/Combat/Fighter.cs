using RainbowAssets.Demo.Attributes;
using RainbowAssets.Demo.Movement;
using RainbowAssets.Utils;
using UnityEngine;

namespace RainbowAssets.Demo.Combat
{
    /// <summary>
    /// Handles combat behavior.
    /// </summary>
    public class Fighter : MonoBehaviour, IAction, IPredicateEvaluator
    {
        /// <summary>
        /// The weapon used by the fighter.
        /// </summary>
        [SerializeField] Weapon weapon;

        [Header("AI Parameters")]

        /// <summary>
        /// The maximum range at which the fighter starts chasing the player.
        /// </summary>
        [SerializeField] float chaseRange = 10;

        /// <summary>
        /// The speed fraction used while chasing the player.
        /// </summary>
        [SerializeField, Range(0,1)] float chaseSpeedFraction = 1;

        /// <summary>
        /// The range at which the fighter can attack the player.
        /// </summary>
        [SerializeField] float attackRange = 1;

        /// <summary>
        /// The duration after losing sight of the player before stopping the chase.
        /// </summary>
        [SerializeField] float suspicionDuration = 3;

        Mover mover;
        Health player;
        float timeSinceLastSawPlayer = Mathf.Infinity;

        /// <summary>
        /// Called via animation event to execute a hit with the equipped weapon.
        /// </summary>
        public void Hit()
        {
            weapon.Hit(gameObject);
        }

        // LIFECYCLE METHODS

        void Awake()
        {
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        void Update()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
        }

        /// <summary>
        /// Checks if the player is within a specified range.
        /// </summary>
        /// <param name="range">The range to check.</param>
        /// <returns>True if the player is in range, otherwise false.</returns>
        bool PlayerInRange(float range)
        {
            if (player.IsDead())
            {
                return false;
            }

            bool inRange = Vector3.Distance(transform.position, player.transform.position) <= range;

            if (inRange)
            {
                timeSinceLastSawPlayer = 0;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Executes an action related to combat behavior.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="parameters">Additional parameters for the action.</param>
        void IAction.DoAction(EAction action, string[] parameters)
        {
            switch (action)
            {
                case EAction.ChasePlayer:
                    mover.MoveTo(player.transform.position, chaseSpeedFraction);
                    break;
            }
        }

        /// <summary>
        /// Evaluates a combat-related predicate.
        /// </summary>
        /// <param name="predicate">The predicate to evaluate.</param>
        /// <param name="parameters">Additional parameters for evaluation.</param>
        /// <returns>True if the predicate condition is met, otherwise false. Returns null if the predicate is unsupported.</returns>
        bool? IPredicateEvaluator.Evaluate(EPredicate predicate, string[] parameters)
        {
            switch (predicate)
            {
                case EPredicate.PlayerInChaseRange:
                    return PlayerInRange(chaseRange);

                case EPredicate.PlayerInAttackRange:
                    return PlayerInRange(attackRange);

                case EPredicate.SuspicionFinished:
                    return timeSinceLastSawPlayer >= suspicionDuration;
            }

            return null;
        }

        /// <summary>
        /// Draws debug gizmos in the editor to visualize the chase range.
        /// </summary>
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, chaseRange);
        }
    }
}