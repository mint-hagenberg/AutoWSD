using UnityEngine;
using System.Collections;
using Leap;

namespace ANR {

    public class ThumbDownGesture : BaseGesture {

        // Use this for initialization
        protected override void Awake() {
            base.Awake();
            currentType = GestureManager.GestureType.ThumbDown;
            // gesture event
            SpecificEvent = GestureEvent;
        }

        protected override bool CheckConditionGesture() {
            Hand hand = GetCurrent1Hand();
            if (hand != null) {
				if (CheckPalmNormalInXZPlane(hand) && CheckFingerCloseToHand(hand) && IsThumbDirection(hand, -Vector3.up)) {
                    return true;
                }
            }
            return false;
        }
        
        void GestureEvent() {
            EventManager.TriggerEvent("Gesture_ThumbDown");
        }
    }
}