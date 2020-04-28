using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ANR
{
    public class PedestrianWaypointNavigator : MonoBehaviour
    {
        #region Properties

        public PedestrianWaypoint currentWaypoint;

        private PedestrianNavigationController controller;
        private int direction; // 0: move forward, 1: move backward

        #endregion


        private void Awake()
        {
            controller = GetComponent<PedestrianNavigationController>();
        }

        // Start is called before the first frame update
        void Start()
        {
            direction = Mathf.RoundToInt(Random.Range(0f, 1f));
            controller.SetDestination(currentWaypoint.GetPosition());
        }

        // Update is called once per frame
        void Update()
        {
            if (controller.reachedDestination)
            {
                bool shouldBranch = false;
                if (currentWaypoint.branches != null && currentWaypoint.branches.Count > 0)
                {
                    // determine if pedestrian should branch or continue the forward walk
                    shouldBranch = Random.Range(0f, 1f) <= currentWaypoint.branchRatio;
                }

                if (shouldBranch)
                {
                    // randomly pick branch as next waypoint position
                    currentWaypoint = currentWaypoint.branches[Random.Range(0, currentWaypoint.branches.Count - 1)];
                }
                else
                {
                    // no branches available, just continue
                    if (direction == 0)
                    {
                        if (currentWaypoint.nextWaypoint != null)
                        {
                            // continue walking forward
                            currentWaypoint = currentWaypoint.nextWaypoint;
                        }
                        else
                        {
                            // go back if there is no next waypoint
                            currentWaypoint = currentWaypoint.previousWaypoint;
                            direction = 1;
                        }
                    }
                    else if (direction == 1)
                    {
                        if (currentWaypoint.previousWaypoint != null)
                        {
                            // continue walking in other direction
                            currentWaypoint = currentWaypoint.previousWaypoint;
                        }
                        else
                        {
                            // go back if there is no next waypoint (in other direction)
                            currentWaypoint = currentWaypoint.nextWaypoint;
                            direction = 0;
                        }
                    }
                }

                controller.SetDestination(currentWaypoint.GetPosition());
            }
        }
    }
}