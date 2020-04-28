using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ANR
{
    public class VRTargetItem : MonoBehaviour
    {
        #region Properties

        // events invoked when gaze pointer enters or exits object
        public UnityEvent gazeEnterEvent;
        public UnityEvent gazeExitEvent;

        // events that are invoked once we select this target item
        public UnityEvent completionEvent;
        private Selectable _selectable;
        private ISubmitHandler _submit;

        // slider-specific variables
        public Slider progressSlider;
        private bool _isGazeOverTarget = false;
        private float _sliderTimer; //how many seconds until slider is set to full
        private float _tempTimer;

        public RadialReticle radialReticle;

        #endregion
        
        public void Start()
        {
            _sliderTimer = radialReticle._radialDuration;
            _tempTimer = _sliderTimer;
        }

        public void Awake()
        {
            _selectable = GetComponent<Selectable>();
            _submit = GetComponent<ISubmitHandler>();
        }

        public void GazeEnter(PointerEventData pointer)
        {
            // When the user looks at the rendering of the scene, show the radial.
            if (_selectable)
                _selectable.OnPointerEnter(pointer);
            else
                gazeEnterEvent.Invoke();

            _isGazeOverTarget = true;
        }

        public void GazeExit(PointerEventData pointer)
        {
            // When the user looks away from the rendering of the scene, hide the radial.
            if (_selectable)
                _selectable.OnPointerExit(pointer);
            else
                gazeExitEvent.Invoke();

            ResetSlider();
        }

        public void GazeComplete(PointerEventData pointer)
        {
            // invoke events that are set up in the inspector
            if (_submit != null)
                _submit.OnSubmit(pointer);
            else
                completionEvent.Invoke();
        }

        private void Update()
        {
            if (_isGazeOverTarget && progressSlider)
                ProgressSlider();
        }

        private void ProgressSlider()
        {
            _tempTimer -= Time.deltaTime;
            progressSlider.value = (_sliderTimer - _tempTimer) / _sliderTimer;
            //if (m_tempTimer <= 0f)
            //  GazeComplete(null);
        }

        private void ResetSlider()
        {
            _isGazeOverTarget = false;
            if (progressSlider)
                progressSlider.value = 0f;
            _tempTimer = _sliderTimer;
        }
    }
}