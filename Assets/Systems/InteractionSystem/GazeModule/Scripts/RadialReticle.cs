using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//originally written by Unity Technologies as part of VRStandardAssets
//modified to support interacting with UI and other game objects only through gaze and without button input
//reticle consists of a small dot, a simple sprite of a semi-transparent ring and an opaque ring with 360-fill sprite
namespace ANR
{
    public class RadialReticle : MonoBehaviour
    {
        #region Properties

        [SerializeField] private float _defaultDistance = 2f; // default distance between reticle and camera
        [SerializeField] private bool _isNormalUsed; // is reticle parallel to a surface or always facing camera.
        [SerializeField] private Transform _reticleTransform; // tranbslate reticle to target object
        [SerializeField] private Transform _camera; // camera position is reference for placing reticle

        [SerializeField] private Image
            _radialImage; //image that is filled as an indication of how long the gazepointer is hovered over item

        [SerializeField]
        public float _radialDuration = 2f; // time it takes to complete the reticle's fill (in seconds).

        private bool _isRadialFilled = false; // check whether fill is complete, to avoid repeated hover completions
        private float _timer; //used to check timing of hover completion


        private Vector3 _originalScale; // initial scale of reticle so it can be rescaled and scale restored

        private Quaternion
            _originalRotation; // initial rotation of reticle so it can be rotated and rotation later restored

        public bool IsNormalUsed
        {
            get { return _isNormalUsed; }
            set { _isNormalUsed = value; }
        }

        public Transform ReticleTransform
        {
            get { return _reticleTransform; }
        }

        #endregion

        private void Awake()
        {
            // Store initial scale and rotation.
            _originalScale = _reticleTransform.localScale;
            _originalRotation = _reticleTransform.localRotation;
        }

        public void ShowRadialImage(bool isActive)
        {
            _radialImage.gameObject.SetActive(isActive);
        }

        // when no targets are hit -> set default distance of reticle
        public void SetPosition()
        {
            // Set position of reticle to default distance in front of camera
            _reticleTransform.position = _camera.position + (_camera.forward / 2);

            // Set scale based on the original and distance from the camera
            _reticleTransform.localScale = _originalScale * _defaultDistance;

            // rotation is default
            _reticleTransform.localRotation = _originalRotation;
        }

        // when target is hit -> reticle is moved to target position
        public void SetPosition(RaycastResult hit)
        {
            _reticleTransform.localPosition = new Vector3(0f, 0f, hit.distance * 2 + 1f);
            _reticleTransform.localScale = _originalScale * _defaultDistance;

            if (_isNormalUsed)
                // set rotation based on forward vector along the normal of hit point
                _reticleTransform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.worldNormal);
            else
                // use original rotation
                _reticleTransform.localRotation = _originalRotation;
        }

        // fill progress of radial image
        public void StartProgress()
        {
            _isRadialFilled = false;
        }

        public bool ProgressRadialImage()
        {
            if (_isRadialFilled == false)
            {
                // advance timer
                _timer += Time.deltaTime;
                _radialImage.fillAmount = _timer / _radialDuration;

                // if timer exceeds duration, complete progress and reset
                if (_timer >= _radialDuration)
                {
                    ResetProgress();
                    _isRadialFilled = true;
                    return true;
                }
            }

            return false;
        }

        public void ResetProgress()
        {
            _timer = 0f;
            _radialImage.fillAmount = 0f;
        }
    }
}