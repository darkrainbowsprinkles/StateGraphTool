using UnityEngine;

namespace RainbowAssets.Demo
{
    public class Patroller : MonoBehaviour
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
                timeSinceArrivedAtWaypoint = 0;
            }

            return arrived;
        }

        void MoveToCurrentWaypoint()
        {
            mover.MoveTo(GetCurentWaypoint(), patrolSpeedFraction);
        }

        void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        Vector3 GetCurentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }
    }
}