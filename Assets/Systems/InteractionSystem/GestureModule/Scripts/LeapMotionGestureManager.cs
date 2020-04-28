using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ANR
{
    public class LeapMotionGestureManager : MonoBehaviour
    {
        #region Properties

        public GestureManager gestureManager;

        #endregion

        // Use this for initialization
        void Awake()
        {
            gestureManager.InitGestures();
        }
    }
}