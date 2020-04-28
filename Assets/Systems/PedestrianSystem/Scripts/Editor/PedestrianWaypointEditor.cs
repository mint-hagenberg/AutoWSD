using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ANR
{
    [InitializeOnLoad()]
    public class PedestrianWaypointEditor
    {
        [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
        public static void OnDrawSceneGizmo(PedestrianWaypoint waypoint, GizmoType gizmoType)
        {
            if ((gizmoType & GizmoType.Selected) != 0)
            {
                Gizmos.color = Color.magenta;
            }
            else
            {
                Gizmos.color = Color.magenta * 0.5f;
            }

            Gizmos.DrawSphere(waypoint.transform.position, 0.1f);

            Gizmos.color = Color.white;
            Gizmos.DrawLine(waypoint.transform.position + (waypoint.transform.right * waypoint.width / 2f),
                waypoint.transform.position - (waypoint.transform.right * waypoint.width / 2f));

            if (waypoint.previousWaypoint != null)
            {
                Gizmos.color = Color.red;
                Vector3 offsetFrom = waypoint.transform.right * waypoint.width / 2f;
                Vector3 offsetTo = waypoint.previousWaypoint.transform.right * waypoint.previousWaypoint.width / 2f;

                Gizmos.DrawLine(waypoint.transform.position + offsetFrom,
                    waypoint.previousWaypoint.transform.position + offsetTo);
            }

            if (waypoint.nextWaypoint != null)
            {
                Gizmos.color = Color.green;
                Vector3 offsetFrom = waypoint.transform.right * -waypoint.width / 2f;
                Vector3 offsetTo = waypoint.nextWaypoint.transform.right * -waypoint.nextWaypoint.width / 2f;

                Gizmos.DrawLine(waypoint.transform.position + offsetFrom,
                    waypoint.nextWaypoint.transform.position + offsetTo);
            }

            if (waypoint.branches != null)
            {
                foreach (PedestrianWaypoint branch in waypoint.branches)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(waypoint.transform.position, branch.transform.position);
                }
            }
        }
    }
}