using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Leap;

namespace ANR {

    public class FistGesture : BaseGesture {

        // Use this for initialization
        protected override void Awake() {
            base.Awake();
            currentType = GestureManager.GestureType.Fist;
            // gesture event
            SpecificEvent = GestureEvent;
        }

        protected override bool CheckConditionGesture() {
            Hand hand = GetCurrent1Hand();
            if (hand != null) {
                if (IsCloseHand(hand) && IsStationary(hand)) {
                    return true;
                }
            }
            return false;
        }

        void GestureEvent() {
            EventManager.TriggerEvent("Gesture_Fist");
        }
    }
}