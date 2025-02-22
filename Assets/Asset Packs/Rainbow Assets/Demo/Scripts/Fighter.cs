using RainbowAssets.Utils;
using UnityEngine;

namespace RainbowAssets.Demo
{
    public class Fighter : MonoBehaviour, IAction, IPredicateEvaluator
    {
        [SerializeField] Weapon weapon;
        [SerializeField] float chaseRange = 10;
        [SerializeField, Range(0,1)] float chaseSpeedFraction = 1;
        [SerializeField] float attackRange = 1;
        [SerializeField] float suspicionDuration = 3;
        Mover mover;
        Health player;
        float timeSinceLastSawPlayer = Mathf.Infinity;

        // Animation Event
        public void Hit()
        {
            weapon.Hit(gameObject);
        }

        void Awake()
        {
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        void Update()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
        }

        bool PlayerInRange(float range)
        {
            if(player.IsDead())
            {
                return false;
            }
            
            bool inRange = Vector3.Distance(transform.position, player.transform.position) <= range;
            
            if(inRange)
            {
                timeSinceLastSawPlayer = 0;
                return true;
            }

            return false;
        }

        void IAction.DoAction(EAction action, string[] parameters)
        {
            switch(action)
            {
                case EAction.ChasePlayer:
                    mover.MoveTo(player.transform.position, chaseSpeedFraction);
                    break;
            }
        }

        bool? IPredicateEvaluator.Evaluate(EPredicate predicate, string[] parameters)
        {
            switch(predicate)
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

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, chaseRange);
        }
    }
}