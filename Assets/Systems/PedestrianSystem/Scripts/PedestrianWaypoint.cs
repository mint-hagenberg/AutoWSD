using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace ANR
{
    public class PedestrianWaypoint : MonoBehaviour
    {
        #region Properties

        public PedestrianWaypoint previousWaypoint;
        public PedestrianWaypoint nextWaypoint;
        [Range(0f, 5f)] public float width = 1f;

        // branching waypoints
        public List<PedestrianWaypoint> branches;
        [Range(0f, 1f)] public float branchRatio = 0.5f;

        #endregion

        public Vector3 GetPosition()
        {
            Vector3 minBound = transform.position + transform.right * width / 2f;
            Vector3 maxBound = transform.position - transform.right * width / 2f;

            return Vector3.Lerp(minBound, maxBound, Random.Range(0f, 1f));
        }
    }
}