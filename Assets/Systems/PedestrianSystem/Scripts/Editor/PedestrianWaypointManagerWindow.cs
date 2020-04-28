using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ANR
{
    public class PedestrianWaypointManagerWindow : EditorWindow
    {
        #region Properties

        public Transform waypointRoot;

        #endregion

        [MenuItem("Tools/Pedestrian Waypoint Editor")]
        public static void Open()
        {
            GetWindow<PedestrianWaypointManagerWindow>();
        }

        private void OnGUI()
        {
            SerializedObject obj = new SerializedObject(this);

            EditorGUILayout.PropertyField(obj.FindProperty("waypointRoot"));

            if (waypointRoot == null)
            {
                EditorGUILayout.HelpBox("Root transform must be selected. Please assign a root transform.",
                    MessageType.Warning);
            }
            else
            {
                EditorGUILayout.BeginVertical("box");
                DrawButtons();
                EditorGUILayout.EndVertical();
            }

            obj.ApplyModifiedProperties();
        }

        private void DrawButtons()
        {
            // create new waypoint at root
            if (GUILayout.Button("Create Pedestrian Waypoint"))
            {
                CreateWaypoint();
            }

            // if a waypoint is selected, you can add waypoints before/after it or remove it
            if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<PedestrianWaypoint>())
            {
                if (GUILayout.Button("Add Branch Waypoint"))
                {
                    CreateBranch();
                }

                if (GUILayout.Button("Create Pedestrian Waypoint Before"))
                {
                    CreateWaypointBefore();
                }

                if (GUILayout.Button("Create Pedestrian Waypoint After"))
                {
                    CreateWaypointAfter();
                }

                if (GUILayout.Button("Remove Pedestrian Waypoint"))
                {
                    RemoveWaypoint();
                }
            }
        }

        private void CreateWaypoint()
        {
            GameObject waypointObject = new GameObject("PedestrianWaypoint " + waypointRoot.childCount,
                typeof(PedestrianWaypoint));
            waypointObject.transform.SetParent(waypointRoot, false);

            PedestrianWaypoint waypoint = waypointObject.GetComponent<PedestrianWaypoint>();
            if (waypointRoot.childCount > 1)
            {
                waypoint.previousWaypoint =
                    waypointRoot.GetChild(waypointRoot.childCount - 2).GetComponent<PedestrianWaypoint>();
                waypoint.previousWaypoint.nextWaypoint = waypoint;
                // place waypoint at last position
                waypoint.transform.position = waypoint.previousWaypoint.transform.position;
                waypoint.transform.forward = waypoint.previousWaypoint.transform.forward;
            }

            Selection.activeGameObject = waypoint.gameObject;
        }

        private void CreateWaypointBefore()
        {
            GameObject waypointObject = new GameObject("PedestrianWaypoint " + waypointRoot.childCount,
                typeof(PedestrianWaypoint));
            waypointObject.transform.SetParent(waypointRoot, false);

            PedestrianWaypoint newWaypoint = waypointObject.GetComponent<PedestrianWaypoint>();
            PedestrianWaypoint selectedWaypoint = Selection.activeGameObject.GetComponent<PedestrianWaypoint>();

            waypointObject.transform.position = selectedWaypoint.transform.position;
            waypointObject.transform.forward = selectedWaypoint.transform.forward;

            if (selectedWaypoint.previousWaypoint != null)
            {
                newWaypoint.previousWaypoint = selectedWaypoint.previousWaypoint;
                selectedWaypoint.previousWaypoint.nextWaypoint = newWaypoint;
            }

            newWaypoint.nextWaypoint = selectedWaypoint;
            selectedWaypoint.previousWaypoint = newWaypoint;

            newWaypoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex());
            Selection.activeGameObject = newWaypoint.gameObject;
        }

        private void CreateWaypointAfter()
        {
            GameObject waypointObject = new GameObject("PedestrianWaypoint " + waypointRoot.childCount,
                typeof(PedestrianWaypoint));
            waypointObject.transform.SetParent(waypointRoot, false);

            PedestrianWaypoint newWaypoint = waypointObject.GetComponent<PedestrianWaypoint>();
            PedestrianWaypoint selectedWaypoint = Selection.activeGameObject.GetComponent<PedestrianWaypoint>();

            waypointObject.transform.position = selectedWaypoint.transform.position;
            waypointObject.transform.forward = selectedWaypoint.transform.forward;

            newWaypoint.previousWaypoint = selectedWaypoint;

            if (selectedWaypoint.nextWaypoint != null)
            {
                selectedWaypoint.nextWaypoint.previousWaypoint = newWaypoint;
                newWaypoint.nextWaypoint = selectedWaypoint.nextWaypoint;
            }

            selectedWaypoint.nextWaypoint = newWaypoint;

            newWaypoint.transform.SetSiblingIndex(selectedWaypoint.transform.GetSiblingIndex());
            Selection.activeGameObject = newWaypoint.gameObject;
        }

        private void RemoveWaypoint()
        {
            PedestrianWaypoint selectedWaypoint = Selection.activeGameObject.GetComponent<PedestrianWaypoint>();

            if (selectedWaypoint.nextWaypoint != null)
            {
                selectedWaypoint.nextWaypoint.previousWaypoint = selectedWaypoint.previousWaypoint;
            }

            if (selectedWaypoint.previousWaypoint != null)
            {
                selectedWaypoint.previousWaypoint.nextWaypoint = selectedWaypoint.nextWaypoint;
                Selection.activeGameObject = selectedWaypoint.previousWaypoint.gameObject;
            }

            DestroyImmediate(selectedWaypoint.gameObject);
        }

        private void CreateBranch()
        {
            GameObject waypointObject = new GameObject("PedestrianWaypointBranch " + waypointRoot.childCount,
                typeof(PedestrianWaypoint));
            waypointObject.transform.SetParent(waypointRoot, false);

            PedestrianWaypoint waypoint = waypointObject.GetComponent<PedestrianWaypoint>();

            PedestrianWaypoint branchedFromWaypoint = Selection.activeGameObject.GetComponent<PedestrianWaypoint>();
            branchedFromWaypoint.branches.Add(waypoint);

            waypoint.transform.position = branchedFromWaypoint.transform.position;
            waypoint.transform.forward = branchedFromWaypoint.transform.forward;

            Selection.activeGameObject = waypoint.gameObject;
        }
    }
}