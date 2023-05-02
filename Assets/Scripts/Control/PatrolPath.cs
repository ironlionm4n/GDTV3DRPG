using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        [SerializeField] private float patrolPointGizmoRadius;

        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var j = GetNextIndex(i);
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(GetWaypoint(i), patrolPointGizmoRadius);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
            }
        }

        public int GetNextIndex(int i)
        {
            var nextWaypoint = i + 1;
            if (nextWaypoint == transform.childCount)
            {
                return 0;
            }

            return nextWaypoint;
        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}