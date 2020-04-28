using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ANR
{
    public class ShowVolume : MonoBehaviour
    {
        #region Properties
        
        public Slider slider;
        public Text valueOfSlider;
        
        #endregion

        // Update is called once per frame
        void Update()
        {
            valueOfSlider.text = slider.value.ToString("0");
        }
    }
}