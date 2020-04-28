using UnityEngine;
using System.Collections;
using Leap;

namespace ANR {

    public class SwipingLeftGesture : BaseGesture {

        // Use this for initialization
        protected override void Awake() {
            base.Awake();
            currentType = GestureManager.GestureType.SwipingLeft;
            // gesture event
            SpecificEvent = GestureEvent;
        }

        protected override bool CheckConditionGesture() {
            Hand hand = GetCurrent1Hand();
            if (hand != null) {
                if (IsOpenFullHand(hand) && IsMoveLeft(hand)) {
                    return true;
                }
            }
            return false;
        }

        void GestureEvent()
        {
            EventManager.TriggerEvent("Gesture_SwipingLeft");
        }
    }
}