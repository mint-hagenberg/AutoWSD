using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ANR
{
    [RequireComponent(typeof(Camera))]
    public class VRGazeController : MonoBehaviour
    {
        #region Properties
        
        [SerializeField] private RadialReticle reticle; // The reticle, if applicable.
        [SerializeField] private bool showDebugRay; // Optionally show the debug ray.
        [SerializeField] private float debugRayLength = 5f; // Debug ray length.
        [SerializeField] private float debugRayDuration = 1f; // How long the Debug ray will remain visible.
        [SerializeField] private float rayLength = 500f; // How far into the scene the ray is cast.
        [SerializeField] private EventSystem eventSystem; // variables for EventSystem.RaycastAll
        private PointerEventData _pointerEventData; // variables for EventSystem.RaycastAll
        private RaycastResult _currentTarget;
        private VRTargetItem _target;
        private VRTargetItem _previousTarget;
        
        #endregion

        void Update()
        {
            GazeRaycast();
        }

        private void GazeRaycast()
        {
            // Show the debug ray
            if (showDebugRay)
            {
                Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * debugRayLength,
                    Color.blue, debugRayDuration);
            }

            // Set up PointerEventData
            _pointerEventData = new PointerEventData(eventSystem);
            //Set PointerEventData position to the center of the screen
            //m_pointerEventData.position = new Vector2(Screen.width / 2, Screen.height / 2);

            //change to center of eyeTextures for Oculus devices
            _pointerEventData.position = new Vector2(UnityEngine.XR.XRSettings.eyeTextureWidth / 2,
                UnityEngine.XR.XRSettings.eyeTextureHeight / 2);

            // Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();
            eventSystem.RaycastAll(_pointerEventData, results);

            // loop through all items hit
            for (int i = 0; i < results.Count; i++)
            {
                if (results[i].gameObject.GetComponent<VRTargetItem>())
                {
                    _target = results[i].gameObject.GetComponent<VRTargetItem>();
                    _currentTarget = results[i];

                    // Something was hit, set position to hit position.
                    if (reticle)
                        reticle.SetPosition(results[i]);
                    break;
                }

                // no targets found
                _target = null;
            }

            // If current interactive item is not the same as the last interactive item, then call GazeEnter and start fill
            if (_target && _target != _previousTarget)
            {
                reticle.ShowRadialImage(true);
                _target.GazeEnter(_pointerEventData);
                if (_previousTarget)
                    _previousTarget.GazeExit(_pointerEventData);
                reticle.StartProgress();
                _previousTarget = _target;
            }
            else if (_target && _target == _previousTarget) //hovering over same item, advance fill progress
            {
                if (reticle.ProgressRadialImage()) //returns true if selection is completed
                    CompleteSelection();
            }
            else
            {
                // no target hit
                if (_previousTarget)
                    _previousTarget.GazeExit(_pointerEventData);

                _target = null;
                _previousTarget = null;
                reticle.ShowRadialImage(false);
                reticle.ResetProgress();
                reticle.SetPosition();
            }
        }

        private void CompleteSelection()
        {
            // hide radial image
            reticle.ShowRadialImage(false);

            // radial progress completed, call completion events on target
            _target.GazeComplete(_pointerEventData);
        }
    }
}