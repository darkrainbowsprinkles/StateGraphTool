using RainbowAssets.Utils;
using UnityEngine;

namespace RainbowAssets.Demo.Movement
{
    public class Patroller : MonoBehaviour, IActionPerformer, IPredicateEvaluator
    {
        [SerializeField] RainbowAssets.Demo.Control.PatrolPath patrolPath;
        [SerializeField] [Range(0,1)] float patrolSpeedFraction = 0.6f;
        [SerializeField] float waypointDwellTime = 3;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        int currentWaypointIndex = 0;
        RainbowAssets.Demo.Movement.Mover mover;

        void Awake()
        {
            mover = GetComponent<RainbowAssets.Demo.Movement.Mover>();
        }

        void Update()
        {
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        bool AtWaypoint()
        {
            bool arrived = mover.AtDestination(GetCurrentWaypoint());

            if (arrived)
            {
                currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
                timeSinceArrivedAtWaypoint = 0;
            }

            return arrived;
        }

        void MoveToCurrentWaypoint()
        {
            mover.MoveTo(GetCurrentWaypoint(), patrolSpeedFraction);
        }

        Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        void IActionPerformer.PerformAction(EAction action, string[] parameters)
        {
            switch (action)
            {
                case EAction.MoveToWaypoint:
                    MoveToCurrentWaypoint();
                    break;
            }
        }

        bool? IPredicateEvaluator.Evaluate(EPredicate predicate, string[] parameters)
        {
            switch (predicate)
            {
                case EPredicate.AtWaypoint:
                    return AtWaypoint();

                case EPredicate.CanPatrol:
                    return timeSinceArrivedAtWaypoint >= waypointDwellTime;
            }

            return null;
        }
    }
}