using RainbowAssets.Utils;
using UnityEngine;

namespace RainbowAssets.Demo
{
    public class Patroller : MonoBehaviour, IAction, IPredicateEvaluator
    {
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] [Range(0,1)] float patrolSpeedFraction = 0.6f;
        [SerializeField] float waypointDwellTime = 3;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        int currentWaypointIndex = 0;
        Mover mover;

        void Awake()
        {
            mover = GetComponent<Mover>();
        }

        void Update()
        {
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        bool AtWaypoint()
        {
            bool arrived = mover.AtDestination(GetCurentWaypoint());

            if(arrived)
            {
                currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
                timeSinceArrivedAtWaypoint = 0;
            }

            return arrived;
        }

        void MoveToCurrentWaypoint()
        {
            mover.MoveTo(GetCurentWaypoint(), patrolSpeedFraction);
        }

        Vector3 GetCurentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        void IAction.DoAction(EAction action, string[] parameters)
        {
            switch(action)
            {
                case EAction.MoveToWaypoint:
                    MoveToCurrentWaypoint();
                    break;
            }
        }

        bool? IPredicateEvaluator.Evaluate(EPredicate predicate, string[] parameters)
        {
            switch(predicate)
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