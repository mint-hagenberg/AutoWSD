using UnityEngine;
using System.Collections;
using Leap;

namespace ANR {

    public class SwipingForwardGesture : BaseGesture {

        // Use this for initialization
        protected override void Awake() {
            base.Awake();
            currentType = GestureManager.GestureType.SwipingForward;
            // gesture event
            SpecificEvent = GestureEvent;
        }

        protected override bool CheckConditionGesture() {
            Hand hand = GetCurrent1Hand();
            if (hand != null) {
                if (IsOpenFullHand(hand) && IsHandMoveForward(hand) && IsPalmNormalSameDirectionWith(hand, Vector3.forward)) {
                    return true;
                }
            }
            return false;
        }

        void GestureEvent() {
            EventManager.TriggerEvent("Gesture_SwipingForward");
        }
    }
}