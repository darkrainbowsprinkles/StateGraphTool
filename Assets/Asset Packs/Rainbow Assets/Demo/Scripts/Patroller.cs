using RainbowAssets.Utils;
using UnityEngine;

namespace RainbowAssets.Demo
{
    /// <summary>
    /// Handles patrolling behavior along a predefined patrol path.
    /// </summary>
    public class Patroller : MonoBehaviour, IAction, IPredicateEvaluator
    {
        /// <summary>
        /// The patrol path defining waypoints for movement.
        /// </summary>
        [SerializeField] PatrolPath patrolPath;

        /// <summary>
        /// The fraction of maximum speed used while patrolling.
        /// </summary>
        [SerializeField] [Range(0,1)] float patrolSpeedFraction = 0.6f;

        /// <summary>
        /// Time in seconds to wait at each waypoint before moving to the next.
        /// </summary>
        [SerializeField] float waypointDwellTime = 3;

        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        int currentWaypointIndex = 0;
        Mover mover;

        // LIFECYCLE METHODS

        void Awake()
        {
            mover = GetComponent<Mover>();
        }

        void Update()
        {
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        /// <summary>
        /// Checks if the character has arrived at the current waypoint and updates the index.
        /// </summary>
        /// <returns>True if at the waypoint, otherwise false.</returns>
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

        /// <summary>
        /// Moves the character to the current waypoint.
        /// </summary>
        void MoveToCurrentWaypoint()
        {
            mover.MoveTo(GetCurrentWaypoint(), patrolSpeedFraction);
        }

        /// <summary>
        /// Retrieves the position of the current waypoint.
        /// </summary>
        /// <returns>The position of the current waypoint.</returns>
        Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        /// <summary>
        /// Performs an action as defined by the IAction interface.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="parameters">Additional parameters for the action.</param>
        void IAction.DoAction(EAction action, string[] parameters)
        {
            switch (action)
            {
                case EAction.MoveToWaypoint:
                    MoveToCurrentWaypoint();
                    break;
            }
        }

        /// <summary>
        /// Evaluates a predicate as defined by the IPredicateEvaluator interface.
        /// </summary>
        /// <param name="predicate">The predicate to evaluate.</param>
        /// <param name="parameters">Additional parameters for evaluation.</param>
        /// <returns>The result of the predicate evaluation.</returns>
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