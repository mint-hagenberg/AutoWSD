using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ANR
{
    public class GazeScrollbar : MonoBehaviour
    {
        #region Properties

        public Button mainButton;
        public Scrollbar mainScrollbar;
        public float value;

        #endregion

        public void Start()
        {
            // TaskOnClick method is called when main button is clicked
            mainButton.onClick.AddListener(TaskOnClickEvent);
        }

        public void TaskOnClickEvent()
        {
            // set slider value
            mainScrollbar.value = value;
        }
    }
}