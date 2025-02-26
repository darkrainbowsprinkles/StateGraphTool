using UnityEngine;

namespace RainbowAssets.Demo
{
    /// <summary>
    /// Defines a patrol path using child objects as waypoints.
    /// </summary>
    public class PatrolPath : MonoBehaviour
    {
        /// <summary>
        /// The radius of the gizmo sphere drawn at each waypoint.
        /// </summary>
        const float waypointRadius = 0.3f;

        /// <summary>
        /// Retrieves the position of a waypoint at the specified index.
        /// </summary>
        /// <param name="index">The index of the waypoint.</param>
        /// <returns>The world position of the waypoint.</returns>
        public Vector3 GetWaypoint(int index)
        {
            return transform.GetChild(index).position;
        }

        /// <summary>
        /// Gets the index of the next waypoint in the patrol path.
        /// Loops back to the first waypoint if at the last one.
        /// </summary>
        /// <param name="index">The current waypoint index.</param>
        /// <returns>The index of the next waypoint.</returns>
        public int GetNextIndex(int index)
        {
            if (index == transform.childCount - 1)
            {
                return 0;
            }

            return index + 1;
        }

        /// <summary>
        /// Draws gizmos in the editor to visualize the patrol path.
        /// </summary>
        void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Gizmos.DrawSphere(GetWaypoint(i), waypointRadius);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(GetNextIndex(i)));
            }
        }
    }
}