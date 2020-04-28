using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Leap.Unity;
using UnityEngine.UI;

namespace ANR
{
    public class GestureManager : MonoBehaviour
    {
        #region Properties

        public enum GestureType
        {
            SwipingLeft,
            SwipingRight,
            SwipingUp,
            SwipingDown,
            SwipingForward,
            SwipingBackward,
            ThumbUp,
            ThumbDown,
            Fist,
            FaceUp,
            FaceDown,
        }

        public Transform player;
        public LeapProvider leapProvider;
        public float timeBetween2Gestures;
        public Dictionary<GestureType, object> activeGestures;
        private GestureType _currentGestureType;
        
        #endregion

        public GestureType GteCurrentGestureType()
        {
            return _currentGestureType;
        }

        public LeapProvider GetLeapHand()
        {
            return leapProvider;
        }

        public Dictionary<GestureType, object> GetCurrentActiveGestures()
        {
            return activeGestures;
        }

        // Use this for initialization
        void Start()
        {
            Invoke("InitGestures", 3f);
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void InitGestures()
        {
            activeGestures = new Dictionary<GestureType, object>();
            foreach (Transform t in transform)
            {
                BaseGesture gesture = t.GetComponent<BaseGesture>();
                if (gesture != null)
                {
                    gesture.SetPlayerTransform(player);
                    foreach (GestureType type in Enum.GetValues(typeof(GestureType)))
                    {
                        if (gesture.GetCurrentType() == type && !activeGestures.ContainsKey(type))
                        {
                            activeGestures.Add(type, t.GetComponent<BaseGesture>() as object);
                        }
                    }

                    t.GetComponent<BaseGesture>().Init(this);
                }
            }
        }

        public virtual bool ReceiveEvent(GestureType type)
        {
            //Debug.Log("ReceiveEvent " + type.ToString());
            _currentGestureType = type;
            Invoke("UnblockCurrentGesture", timeBetween2Gestures);
            return true;
        }

        protected void UnblockCurrentGesture()
        {
            BaseGesture gesture = (BaseGesture) activeGestures[_currentGestureType];
            gesture.UnblockGesture();
        }

        protected void UnBlockGesture(GestureType type)
        {
            BaseGesture gesture = (BaseGesture) activeGestures[type];
            gesture.UnblockGesture();
        }

        public virtual void LoadingGestureProgress(GestureType type, float percent)
        {
        }
    }
}