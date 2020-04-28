using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ANR
{
    public class GestureHelper : MonoBehaviour
    {
        #region Properties

        // selected object
        [SerializeField] private TMP_Text label;

        // event listeners
        private UnityAction eventListenerGestureSwipeRight;
        private UnityAction eventListenerGestureSwipeLeft;
        private UnityAction eventListenerGestureSwipeUp;
        private UnityAction eventListenerGestureSwipeDown;
        private UnityAction eventListenerGestureSwipeForward;
        private UnityAction eventListenerGestureSwipeBackward;
        private UnityAction eventListenerGestureThumbUp;
        private UnityAction eventListenerGestureThumbDown;
        private UnityAction eventListenerGestureFaceUp;
        private UnityAction eventListenerGestureFaceDown;
        private UnityAction eventListenerGestureFist;

        #endregion

        private void Awake()
        {
            // listen to events
            eventListenerGestureSwipeRight = new UnityAction(SwipeRight);
            eventListenerGestureSwipeLeft = new UnityAction(SwipeLeft);
            eventListenerGestureSwipeUp = new UnityAction(SwipeUp);
            eventListenerGestureSwipeDown = new UnityAction(SwipeDown);
            eventListenerGestureSwipeForward = new UnityAction(SwipeForward);
            eventListenerGestureSwipeBackward = new UnityAction(SwipeBackward);
            eventListenerGestureThumbUp = new UnityAction(ThumbUp);
            eventListenerGestureThumbDown = new UnityAction(ThumbDown);
            eventListenerGestureFaceUp = new UnityAction(FaceUp);
            eventListenerGestureFaceDown = new UnityAction(FaceDown);
            eventListenerGestureFist = new UnityAction(Fist);
        }

        // Update is called once per frame
        void Update()
        {
        }

        void OnEnable()
        {
            EventManager.StartListening("Gesture_SwipingRight", eventListenerGestureSwipeRight);
            EventManager.StartListening("Gesture_SwipingLeft", eventListenerGestureSwipeLeft);
            EventManager.StartListening("Gesture_SwipingUp", eventListenerGestureSwipeUp);
            EventManager.StartListening("Gesture_SwipingDown", eventListenerGestureSwipeDown);
            EventManager.StartListening("Gesture_SwipingForward", eventListenerGestureSwipeForward);
            EventManager.StartListening("Gesture_SwipingBackward", eventListenerGestureSwipeBackward);
            EventManager.StartListening("Gesture_ThumbUp", eventListenerGestureThumbUp);
            EventManager.StartListening("Gesture_ThumbDown", eventListenerGestureThumbDown);
            EventManager.StartListening("Gesture_FaceUp", eventListenerGestureFaceUp);
            EventManager.StartListening("Gesture_FaceDown", eventListenerGestureFaceDown);
            EventManager.StartListening("Gesture_Fist", eventListenerGestureFist);
        }

        void OnDisable()
        {
            EventManager.StopListening("Gesture_SwipingRight", eventListenerGestureSwipeRight);
            EventManager.StopListening("Gesture_SwipingLeft", eventListenerGestureSwipeLeft);
            EventManager.StopListening("Gesture_SwipingUp", eventListenerGestureSwipeUp);
            EventManager.StopListening("Gesture_SwipingDown", eventListenerGestureSwipeDown);
            EventManager.StopListening("Gesture_SwipingForward", eventListenerGestureSwipeForward);
            EventManager.StopListening("Gesture_SwipingBackward", eventListenerGestureSwipeBackward);
            EventManager.StopListening("Gesture_ThumbUp", eventListenerGestureThumbUp);
            EventManager.StopListening("Gesture_ThumbDown", eventListenerGestureThumbDown);
            EventManager.StartListening("Gesture_FaceUp", eventListenerGestureFaceUp);
            EventManager.StartListening("Gesture_FaceDown", eventListenerGestureFaceDown);
            EventManager.StartListening("Gesture_Fist", eventListenerGestureFist);
        }

        #region EventManager

        public void SwipeRight()
        {
            label.SetText("Swipe Right");
        }

        public void SwipeLeft()
        {
            label.SetText("Swipe Left");
        }

        public void SwipeUp()
        {
            label.SetText("Swipe Up");
        }
        
        public void SwipeDown()
        {
            label.SetText("Swipe Down");
        }
        
        public void SwipeForward()
        {
            label.SetText("Swipe Forward");
        }
        
        public void SwipeBackward()
        {
            label.SetText("Swipe Backward");
        }
        
        public void ThumbUp()
        {
            label.SetText("Thumb Up");
        }
        
        public void ThumbDown()
        {
            label.SetText("Thumb Down");
        }
        
        public void FaceUp()
        {
            label.SetText("Palm Up");
        }
        
        public void FaceDown()
        {
            label.SetText("Palm Down");
        }
        
        public void Fist()
        {
            label.SetText("Fist");
        }

        #endregion
    }
}