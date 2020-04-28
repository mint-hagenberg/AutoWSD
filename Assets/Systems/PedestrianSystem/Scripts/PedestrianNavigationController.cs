using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANR
{
    public class PedestrianNavigationController : MonoBehaviour
    {
        #region Properties

        [SerializeField] private Vector3 destination = Vector3.zero;
        [SerializeField] private float stopDistance = 2.5f;
        [SerializeField] private float rotationSpeed = 120f;
        [SerializeField] private float movementSpeed = 1f;
        public bool reachedDestination = false;

        private Vector3 lastPosition;
        private Vector3 velocity = Vector3.zero;
        private Animator animator;

        #endregion

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (transform.position != destination)
            {
                Vector3 destinationDirection = destination - transform.position;
                destinationDirection.y = 0;

                float destinationDistance = destinationDirection.magnitude;

                if (destinationDistance >= stopDistance)
                {
                    reachedDestination = false;
                    Quaternion targetRotation = Quaternion.LookRotation(destinationDirection);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation,
                        rotationSpeed * Time.deltaTime);
                    transform.Translate(Vector3.forward * (movementSpeed * Time.deltaTime));
                }
                else
                {
                    reachedDestination = true;
                }

                velocity = (transform.position - lastPosition) / Time.deltaTime;
                velocity.y = 0;
                float velocityMagnitude = velocity.magnitude;
                velocity = velocity.normalized;
                float forwardDotProduct = Vector3.Dot(transform.forward, velocity);
                float rightDotProduct = Vector3.Dot(transform.right, velocity);

                // animator
                if (animator)
                {
                    animator.SetFloat("Horizontal", rightDotProduct);
                    animator.SetFloat("Forward", forwardDotProduct);
                }
            }
            else
            {
                reachedDestination = true;
            }
        }

        public void SetDestination(Vector3 destination)
        {
            this.destination = destination;
            reachedDestination = false;
        }
    }
}