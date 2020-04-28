using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ANR
{
    public class GazeSlider : MonoBehaviour
    {
        #region Properties

        public Button mainButton;
        public Slider mainSlider;
        public int var;

        #endregion
        
        public void Start()
        {
            mainButton.onClick.AddListener(TaskOnClick);
        }

        public void TaskOnClick()
        {
            mainSlider.value = var;
        }
    }
}