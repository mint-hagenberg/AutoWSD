using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANR
{
    public class FingerInteractionManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        // Specify the buttons/sliders interactions here:

        public void OnHorizontalSlider(float value)
        {
            Debug.Log("horizontal slider value: " + value);
        }

        public void OnVolumeSlider(float value)
        {
            Debug.Log("volume slider value: " + value);
        }

        public void OnLeftButtonPress()
        {
            Debug.Log("left button press");
        }

        public void OnRightButtonPress()
        {
            Debug.Log("right button press");
        }
    }
}