using UnityEngine;

namespace RainbowAssets.Demo
{
    public class PatrolPath : MonoBehaviour
    {
        const float waypointRadius = 0.3f;

        public Vector3 GetWaypoint(int index)
        {
            return transform.GetChild(index).position;
        }

        public int GetNextIndex(int index)
        {
            if(index == transform.childCount - 1)
            {
                return 0;
            }

            return index + 1;
        }

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